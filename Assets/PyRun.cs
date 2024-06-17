using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core.Events;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class PyRun : MonoBehaviour
{
    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;

    // ��������·����Ϣ
    public List<List<Vector2>> agentPaths;
    public List<Vector3[]> agentPaths3D;

    //������������
    public  static PalletrobotMove[] plMoves = new PalletrobotMove[8];

    // ѡ������cbs������priaml����
    public bool isCbs;

    // ����ʵ��
    public static PyRun Instance { get; private set; }

    public static List<CellClickHandler.AgentData> agentList;

    public static List<int> agentArrivedList = new List<int>();


    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Awake()
    {

        // ���ȼ���Ƿ��Ѿ�����һ��ʵ��
        if (Instance == null)
        {
            // ��������ڣ�����ǰʵ������Ϊ����ʵ��
            Instance = this;
            // ������GameObject�ڳ����л�ʱ��������
            DontDestroyOnLoad(gameObject);
            UnityEngine.Debug.Log("Instance��ʼ��");
        }
        else
        {
            for (int i = 0; i < plMoves.Length; i++)
            {
                GameObject palletRobot = GameObject.Find("Palletrobot " + i);
                if (palletRobot != null)
                {
                    plMoves[i] = palletRobot.GetComponent<PalletrobotMove>();
                    Vector2 endPoint = agentList[i].endPoint;
                    float layer = agentList[i].layer;
                    plMoves[i].trueTargetPosition = new Vector3(endPoint.x + 0.5f, (layer-1)*1.5f, endPoint.y + 10.5f);
                    UnityEngine.Debug.Log("plMoves[i]-trueTargetPosition:" + plMoves[i].trueTargetPosition);
                }
                else
                {
                    UnityEngine.Debug.LogError("PalletrobotMove objects not found in the scene!");
                }
            }
            UnityEngine.Debug.Log("plMoves[0]-trueTarget:" + plMoves[0].trueTargetPosition);
            UnityEngine.Debug.Log("agentList.Count from awake: " + agentList.Count);
            PalletrobotMove.agentNum = plMoves.Length;

            //���Instance�Ѿ����ڣ�������Ƿ������˵�ǰ��gameObject
            if (Instance != this)
            {
                //����������õ�ǰ��gameObject�����ٵ�ǰgameObject
                UnityEngine.Debug.Log("A PyRun instance already exists. Destroying duplicate.");
                Destroy(gameObject);
            }
            else
            {
                //���Instance���õ��ǵ�ǰgameObject������Ҫ�κβ���
                UnityEngine.Debug.Log("Instance���õ��ǵ�ǰgameObject");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (agentArrivedList.Count>0&& PalletrobotMove.isPaused==false)
        {
            UnityEngine.Debug.Log("agentArrivedList.Count: in Update" + agentArrivedList.Count);
            PauseAllAgents(agentArrivedList);
            agentArrivedList.Clear();
        }
        //if(PalletrobotMove.arrivedNum >= PalletrobotMove.agentNum)
        //{
        //    PalletrobotMove.currentIndex++;
        //    PalletrobotMove.arrivedNum = 0;
        //    for (int i=0;i< PalletrobotMove.agentNum; i++)
        //    {
        //        plMoves[i].haveArrived = false;
        //    }
        //}
    }

    private void startSocket()
    {
        Kill_All_Python_Process();

        // ������ʼcbs��init�����ͨ��
        EstablishPythonCommunication("Python/cbs/init.py", 31413, "cbs");

        // ������ʼprimal��init�����ͨ��
        EstablishPythonCommunication("Python/primal/init.py", 31414, "primal");

        // ������cbs�����ͨ��
        EstablishPythonCommunication("Python/cbs/cbs.py", 31415, "cbs");

        // ����cbs��update�����ͨ��
        EstablishPythonCommunication("Python/cbs/update.py", 31416, "cbs");

        // ������primal�����ͨ��
        EstablishPythonCommunication("Python/primal/mapgenerator.py", 31417, "primal");

        // ����primal��update����ͨ��
        EstablishPythonCommunication("Python/primal/update.py", 31418, "primal");
    }

    // ����cbs��primal����
    public void startPathFinding(int method, List<CellClickHandler.AgentData> agentList)
    {
        // ����ͨ��
        startSocket();
        PyRun.agentList = agentList;

        // ��ȡ·����Ϣ
        UnityEngine.Debug.Log("agentList.Count: " + agentList.Count);
        for (int i = 0; i < plMoves.Length&& i < agentList.Count; i++)
        {
            GameObject palletRobot = GameObject.Find("Palletrobot " + i);
            if (palletRobot != null)
            {
                plMoves[i] = palletRobot.GetComponent<PalletrobotMove>();
                Vector2 endPoint = agentList[i].endPoint;
                float layer = agentList[i].layer;
                plMoves[i].trueTargetPosition = new Vector3(endPoint.x + 0.5f, layer, endPoint.y + 10.5f);
                UnityEngine.Debug.Log("plMoves[i]-trueTargetPosition:" + plMoves[i].trueTargetPosition);
            }
            else
            {
                UnityEngine.Debug.LogError("PalletrobotMove objects not found in the scene!");
            }
        }
        
        // ʹ��primal����
        if (method == 2)
        {
            isCbs = false;
            UnityEngine.Debug.Log("run primal");
        }
        // ʹ��cbs����
        else
        {
            isCbs = true;
            UnityEngine.Debug.Log("run cbs");
        }

        // ѡ������cbs����
        if (isCbs)
        {
            UnityEngine.Debug.Log("plMoves[0]: " + plMoves[0].gameObject.name);
            // ��ȡ������������ʼ��Ϣ
            string jsonMessage = JsonConvert.SerializeObject(agentList);

            // ����cbs��init���򣬳�ʼ��input
            StartCoroutine(WaitAndSendMessage(5f, jsonMessage, 31413));

            // ����Э�����ȴ���������Ϣ
            StartCoroutine(WaitAndSendMessage(7f, "Recognizing", 31415));

            UnityEngine.Debug.Log("plmoves[0].movePoints.Length in startpathfinding: " + plMoves[0].movePoints.Length);
        }
        else
        {
            // ��ȡ������������ʼ��Ϣ
            string jsonMessage = JsonConvert.SerializeObject(agentList);

            UnityEngine.Debug.Log(agentList.Count);

            // ����primal��init���򣬳�ʼ��input
            StartCoroutine(WaitAndSendMessage(5f, jsonMessage, 31414));

            // ����Э�����ȴ���������Ϣ
            StartCoroutine(WaitAndSendMessage(7f, "Recognizing", 31417));
        }
    }

    public void AddAgentArrived(int agentArrived)
    {
        agentArrivedList.Add(agentArrived);
    }

    // ��ͣ����������
    public void PauseAllAgents(List<int> agentArrivedList)
    {
        UnityEngine.Debug.Log("agentArrivedList.Count in PauseAllAgents: " + agentArrivedList.Count);
        PalletrobotMove.isPaused = true;

        List<int[]> currentPositions = new List<int[]>();

        // �������������壬��ȡ���ǵĵ�ǰλ�ò���ӵ�currentPositions
        for (int i = 0; i < plMoves.Length; i++)
        {
            if (plMoves[i] != null)
            {
                // ����ǰλ�ã��������뵽��������ӵ�currentPositions
                int x = Mathf.RoundToInt(plMoves[i].transform.position.x-0.5f);
                int y = Mathf.RoundToInt(plMoves[i].transform.position.z-10.5f);
                currentPositions.Add(new int[] { x, y });
                plMoves[i].currentIndex = 0;
            }
        }
        //PalletrobotMove.currentIndex = 0;

        // �������ã��ﵽ�������ŵ�����
        // �ú����Ĳ���Ҳ��Ҫ�޸�ΪagentArrivedList��Ȼ��Ϳ���ɾ���ò��Ա���
        // ��Ϊ������������agentArrivedListֻ�������ε���ȡ�����������ı��
        //List<int> agentArrivedList = new List<int>();
        //agentArrivedList.Add(agentArrived);

        // ���ݵ�ǰλ����Ϣ��update.py������
        var message = new
        {
            // ��ǰagentsλ����Ϣ����
            positions = currentPositions,
            // ����Ŀ��λ�õ�������ı��
            agentIndex = agentArrivedList
        };

        string jsonMessage = JsonConvert.SerializeObject(message);

        if (isCbs)
        {
            // ����cbs�������¼���·��
            SendMessageToPython(jsonMessage, 31416);
            //SendMessageToPython("Recognizing", 31415);
        }
        else
        {
            // ����primal�������¼���·��
            SendMessageToPython(jsonMessage, 31418);
            //SendMessageToPython("Recognizing", 31417);
        }

        agentArrivedList.Clear();//���agentArrivedList
    }

    public class PythonTupleTypeResolver : INodeTypeResolver
    {
        public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (nodeEvent.Tag == "tag:yaml.org,2002:python/tuple")
            {
                currentType = typeof(object[]);
                return true;
            }
            return false;
        }
    }


    // private function
    private void EstablishPythonCommunication(string pythonPath, int port, string env)
    {
        // ����UDPͨ�ŵ�Client
        udpClient = new UdpClient();
        // ����IP��ַ��˿ں�
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

        // ��ȡUnity��Ŀ������·��
        string dataPath = Application.dataPath;
        // ƴ��Python�ļ�������·��
        string fullPath = dataPath + "/" + pythonPath;
        // ���������в���
        string command = $"/c activate {env} & python \"{fullPath}\"";

        // ����ProcessStartInfo����
        startInfo = new ProcessStartInfo();
        // �趨ִ��cmd
        startInfo.FileName = "cmd.exe";
        // �����������һ����command�ַ���
        startInfo.Arguments = command;
        // ��ΪǶ��Unity�к�̨ʹ�ã��������ò���ʾ����
        startInfo.CreateNoWindow = true;
        // ������Ҫ�趨Ϊfalse
        startInfo.UseShellExecute = false;
        // �����ض���������̵ı�׼�����������ֱ�ӱ�Unity C#���񣬴Ӷ�ʵ�� Python -> Unity ��ͨ��
        startInfo.RedirectStandardOutput = true;
        // �����ض���������̵ı�׼��������������Unity��C#�н���Debug Python���bug
        startInfo.RedirectStandardError = true;

        // ����Process
        process = new Process();
        process.StartInfo = startInfo;
        process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);

        //�����ű�Process�����Ҽ������ж�ȡ����뱨��
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
    }

    // Э�̺���
    private IEnumerator WaitAndSendMessage(float time, string message, int port)
    {
        // �ȴ�������
        yield return new WaitForSeconds(time);
        SendMessageToPython(message, port);
    }

    void SendMessageToPython(string message, int port)
    {
        // ������Ϣ���ݵ��ֽ�����
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        // ������Ϣ��ָ���˿�
        udpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        UnityEngine.Debug.Log("Sent message: " + message);
        //UnityEngine.Debug.Log("PalletrobotMove.isPaused��"+ PalletrobotMove.isPaused);
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            if (e.Data == "StartRecognition")
            {
                print("Recognizing");
            }
            // �ҵ�·���󣬶�ȡoutput.yaml�ļ�
            if (e.Data.Contains("Solution found. All agents reached the goal!"))
            {
                if (isCbs)
                {
                    GetAgentPath("Python/cbs/output.yaml");
                }
                else
                {
                    GetAgentPath("Python/primal/output.yaml");
                }
                //UnityEngine.Debug.Log("plMoves[0]: "+ plMoves[0].movePoints.Length);

                // ������������������
                UnityEngine.Debug.Log("PalletrobotMove.isPaused = false" );
                PalletrobotMove.isPaused = false;
            }
            // ������ɺ����¼���·��
            if (e.Data.Contains("Update Done"))
            {
                // ����cbs��primal�������¼���·��
                if (isCbs)
                {
                    SendMessageToPython("Recognizing", 31415);
                }
                else
                {
                    SendMessageToPython("Recognizing", 31417);
                }
                
            }
        }
    }

    private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            // �������
            UnityEngine.Debug.LogError("Received error output: " + e.Data);
        }
    }

    // ��ȡ��������·��
    private void GetAgentPath(string filePath)
    {

        // ��ȡUnity��Ŀ������·��
        string dataPath = Application.dataPath;
        // ƴ���ļ�������·��
        string fullPath = Path.Combine(dataPath, filePath);

        if (File.Exists(fullPath))
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlInput = File.ReadAllText(fullPath);
            var yamlObject = deserializer.Deserialize<Dictionary<string, object>>(yamlInput);

            var schedule = yamlObject["schedule"] as Dictionary<object, object>;

            // ��ʼ����������·����Ϣ����
            agentPaths = new List<List<Vector2>>();

            foreach (var agent in schedule)
            {
                List<Vector2> path = new List<Vector2>();
                var steps = agent.Value as List<object>;

                foreach (var step in steps)
                {
                    var stepDict = step as Dictionary<object, object>;
                    float x = Convert.ToSingle(stepDict["x"]);
                    float y = Convert.ToSingle(stepDict["y"]);
                    path.Add(new Vector2(x, y));
                }

                agentPaths.Add(path);
            }

            // ���ȫ��·�����
            int agent_no = 0;
            foreach (var path in agentPaths)
            {
                UnityEngine.Debug.Log($"(agent{agent_no}��·��Ϊ��)");
                foreach (var point in path)
                {
                    UnityEngine.Debug.Log($"({point.x}, {point.y})");
                }
                agent_no++;
            }
            UnityEngine.Debug.Log("Get Agent Paths Done");

            // ��·����ȡ��ɺ����2D��3D��ת��
            ConvertAllAgentPathsTo3D();
        }
        else
        {
            UnityEngine.Debug.LogError($"File not found: {fullPath}");
        }
    }

    //2D to 3D
    public Vector3 Convert2DTo3D(Vector2 point)
    {
        return new Vector3(point.x + 0.5f, 0, point.y + 0.5f+10f);
    }

    //save 3D
    public Vector3[] GetAgentPathIn3D(int agentIndex)
    {
        if (agentIndex < 0 || agentIndex >= agentPaths.Count)
        {
            UnityEngine.Debug.LogError("Invalid agent index");
            return null;
        }

        List<Vector2> agentPath2D = agentPaths[agentIndex];
        Vector3[] agentPath3D = new Vector3[agentPath2D.Count];

        for (int i = 0; i < agentPath2D.Count; i++)
        {
            agentPath3D[i] = Convert2DTo3D(agentPath2D[i]);
        }

        return agentPath3D;
    }

    private void ConvertAllAgentPathsTo3D()
    {
        agentPaths3D = new List<Vector3[]>();

        for (int i = 0; i < agentPaths.Count; i++)
        {
            Vector3[] path3D = GetAgentPathIn3D(i);
            agentPaths3D.Add(path3D);

            // ���3D·��
            //UnityEngine.Debug.Log($"Agent {i} 3D Path:");
            //foreach (var point in path3D)
            //{
            //    UnityEngine.Debug.Log($"({i},{point.x}, {point.y}, {point.z})");
            //}
        }
        UnityEngine.Debug.Log("Convert All Agent Paths to 3D Done");

        for (int j = 0; j < agentPaths3D.Count; j++)
        {
            plMoves[j].movePoints = agentPaths3D[j];
        }
    }

    void Kill_All_Python_Process()
    {
        Process[] allProcesses = Process.GetProcesses();
        foreach (Process process_1 in allProcesses)
        {
            try
            {
                // ��ȡ���̵�����
                string processName = process_1.ProcessName;
                // ������������а���"python"������ֹ�ý���
                if (processName.ToLower().Contains("python") && process_1.Id != Process.GetCurrentProcess().Id)
                {
                    process_1.Kill();
                }
            }
            catch (Exception ex)
            {
                // �����쳣
                print(ex);
            }
        }
    }

    void OnApplicationQuit()
    {
        // ��Ӧ�ó����˳�ǰִ��һЩ����
        UnityEngine.Debug.Log("Ӧ�ó��򼴽��˳�����������Python����");
        // ��������Python����
        Kill_All_Python_Process();
    }
}
