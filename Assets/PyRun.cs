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

    // 多智能体路径信息
    public List<List<Vector2>> agentPaths;
    public List<Vector3[]> agentPaths3D;

    //多智能体数组
    public  static PalletrobotMove[] plMoves = new PalletrobotMove[8];

    // 选择运行cbs程序还是priaml程序
    public bool isCbs;

    // 单例实例
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

        // 首先检查是否已经存在一个实例
        if (Instance == null)
        {
            // 如果不存在，将当前实例设置为单例实例
            Instance = this;
            // 标记这个GameObject在场景切换时不被销毁
            DontDestroyOnLoad(gameObject);
            UnityEngine.Debug.Log("Instance初始化");
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

            //如果Instance已经存在，检查它是否引用了当前的gameObject
            if (Instance != this)
            {
                //如果不是引用当前的gameObject，销毁当前gameObject
                UnityEngine.Debug.Log("A PyRun instance already exists. Destroying duplicate.");
                Destroy(gameObject);
            }
            else
            {
                //如果Instance引用的是当前gameObject，不需要任何操作
                UnityEngine.Debug.Log("Instance引用的是当前gameObject");
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

        // 创建初始cbs的init程序的通信
        EstablishPythonCommunication("Python/cbs/init.py", 31413, "cbs");

        // 创建初始primal的init程序的通信
        EstablishPythonCommunication("Python/primal/init.py", 31414, "primal");

        // 创建与cbs程序的通信
        EstablishPythonCommunication("Python/cbs/cbs.py", 31415, "cbs");

        // 创建cbs的update程序的通信
        EstablishPythonCommunication("Python/cbs/update.py", 31416, "cbs");

        // 创建与primal程序的通信
        EstablishPythonCommunication("Python/primal/mapgenerator.py", 31417, "primal");

        // 创建primal的update程序通信
        EstablishPythonCommunication("Python/primal/update.py", 31418, "primal");
    }

    // 启动cbs或primal程序
    public void startPathFinding(int method, List<CellClickHandler.AgentData> agentList)
    {
        // 建立通信
        startSocket();
        PyRun.agentList = agentList;

        // 获取路径信息
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
        
        // 使用primal方法
        if (method == 2)
        {
            isCbs = false;
            UnityEngine.Debug.Log("run primal");
        }
        // 使用cbs方法
        else
        {
            isCbs = true;
            UnityEngine.Debug.Log("run cbs");
        }

        // 选择运行cbs程序
        if (isCbs)
        {
            UnityEngine.Debug.Log("plMoves[0]: " + plMoves[0].gameObject.name);
            // 获取补充的智能体初始信息
            string jsonMessage = JsonConvert.SerializeObject(agentList);

            // 运行cbs的init程序，初始化input
            StartCoroutine(WaitAndSendMessage(5f, jsonMessage, 31413));

            // 启动协程来等待并发送消息
            StartCoroutine(WaitAndSendMessage(7f, "Recognizing", 31415));

            UnityEngine.Debug.Log("plmoves[0].movePoints.Length in startpathfinding: " + plMoves[0].movePoints.Length);
        }
        else
        {
            // 获取补充的智能体初始信息
            string jsonMessage = JsonConvert.SerializeObject(agentList);

            UnityEngine.Debug.Log(agentList.Count);

            // 运行primal的init程序，初始化input
            StartCoroutine(WaitAndSendMessage(5f, jsonMessage, 31414));

            // 启动协程来等待并发送消息
            StartCoroutine(WaitAndSendMessage(7f, "Recognizing", 31417));
        }
    }

    public void AddAgentArrived(int agentArrived)
    {
        agentArrivedList.Add(agentArrived);
    }

    // 暂停所有智能体
    public void PauseAllAgents(List<int> agentArrivedList)
    {
        UnityEngine.Debug.Log("agentArrivedList.Count in PauseAllAgents: " + agentArrivedList.Count);
        PalletrobotMove.isPaused = true;

        List<int[]> currentPositions = new List<int[]>();

        // 遍历所有智能体，获取它们的当前位置并添加到currentPositions
        for (int i = 0; i < plMoves.Length; i++)
        {
            if (plMoves[i] != null)
            {
                // 将当前位置（四舍五入到整数）添加到currentPositions
                int x = Mathf.RoundToInt(plMoves[i].transform.position.x-0.5f);
                int y = Mathf.RoundToInt(plMoves[i].transform.position.z-10.5f);
                currentPositions.Add(new int[] { x, y });
                plMoves[i].currentIndex = 0;
            }
        }
        //PalletrobotMove.currentIndex = 0;

        // （测试用）达到智能体编号的数组
        // 该函数的参数也需要修改为agentArrivedList，然后就可以删除该测试变量
        // 作为参数传递来的agentArrivedList只包含本次到达取货点的智能体的编号
        //List<int> agentArrivedList = new List<int>();
        //agentArrivedList.Add(agentArrived);

        // 传递当前位置信息给update.py并运行
        var message = new
        {
            // 当前agents位置信息数组
            positions = currentPositions,
            // 到达目标位置的智能体的编号
            agentIndex = agentArrivedList
        };

        string jsonMessage = JsonConvert.SerializeObject(message);

        if (isCbs)
        {
            // 运行cbs程序，重新计算路径
            SendMessageToPython(jsonMessage, 31416);
            //SendMessageToPython("Recognizing", 31415);
        }
        else
        {
            // 运行primal程序，重新计算路径
            SendMessageToPython(jsonMessage, 31418);
            //SendMessageToPython("Recognizing", 31417);
        }

        agentArrivedList.Clear();//清空agentArrivedList
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
        // 创建UDP通信的Client
        udpClient = new UdpClient();
        // 设置IP地址与端口号
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

        // 获取Unity项目的数据路径
        string dataPath = Application.dataPath;
        // 拼接Python文件的完整路径
        string fullPath = dataPath + "/" + pythonPath;
        // 设置命令行参数
        string command = $"/c activate {env} & python \"{fullPath}\"";

        // 创建ProcessStartInfo对象
        startInfo = new ProcessStartInfo();
        // 设定执行cmd
        startInfo.FileName = "cmd.exe";
        // 输入参数是上一步的command字符串
        startInfo.Arguments = command;
        // 因为嵌入Unity中后台使用，所以设置不显示窗口
        startInfo.CreateNoWindow = true;
        // 这里需要设定为false
        startInfo.UseShellExecute = false;
        // 设置重定向这个进程的标准输出流，用于直接被Unity C#捕获，从而实现 Python -> Unity 的通信
        startInfo.RedirectStandardOutput = true;
        // 设置重定向这个进程的标准报错流，用于在Unity的C#中进行Debug Python里的bug
        startInfo.RedirectStandardError = true;

        // 创建Process
        process = new Process();
        process.StartInfo = startInfo;
        process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);

        //启动脚本Process，并且激活逐行读取输出与报错
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
    }

    // 协程函数
    private IEnumerator WaitAndSendMessage(float time, string message, int port)
    {
        // 等待若干秒
        yield return new WaitForSeconds(time);
        SendMessageToPython(message, port);
    }

    void SendMessageToPython(string message, int port)
    {
        // 创建消息内容的字节数组
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        // 发送消息到指定端口
        udpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        UnityEngine.Debug.Log("Sent message: " + message);
        //UnityEngine.Debug.Log("PalletrobotMove.isPaused："+ PalletrobotMove.isPaused);
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            if (e.Data == "StartRecognition")
            {
                print("Recognizing");
            }
            // 找到路径后，读取output.yaml文件
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

                // 重新启用所有智能体
                UnityEngine.Debug.Log("PalletrobotMove.isPaused = false" );
                PalletrobotMove.isPaused = false;
            }
            // 更新完成后，重新计算路径
            if (e.Data.Contains("Update Done"))
            {
                // 运行cbs或primal程序，重新计算路径
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
            // 调试语句
            UnityEngine.Debug.LogError("Received error output: " + e.Data);
        }
    }

    // 获取多智能体路径
    private void GetAgentPath(string filePath)
    {

        // 获取Unity项目的数据路径
        string dataPath = Application.dataPath;
        // 拼接文件的完整路径
        string fullPath = Path.Combine(dataPath, filePath);

        if (File.Exists(fullPath))
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlInput = File.ReadAllText(fullPath);
            var yamlObject = deserializer.Deserialize<Dictionary<string, object>>(yamlInput);

            var schedule = yamlObject["schedule"] as Dictionary<object, object>;

            // 初始化多智能体路径信息数组
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

            // 输出全部路径结果
            int agent_no = 0;
            foreach (var path in agentPaths)
            {
                UnityEngine.Debug.Log($"(agent{agent_no}的路径为：)");
                foreach (var point in path)
                {
                    UnityEngine.Debug.Log($"({point.x}, {point.y})");
                }
                agent_no++;
            }
            UnityEngine.Debug.Log("Get Agent Paths Done");

            // 在路径获取完成后进行2D到3D的转换
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

            // 输出3D路径
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
                // 获取进程的名称
                string processName = process_1.ProcessName;
                // 如果进程名称中包含"python"，则终止该进程
                if (processName.ToLower().Contains("python") && process_1.Id != Process.GetCurrentProcess().Id)
                {
                    process_1.Kill();
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                print(ex);
            }
        }
    }

    void OnApplicationQuit()
    {
        // 在应用程序退出前执行一些代码
        UnityEngine.Debug.Log("应用程序即将退出，清理所有Python进程");
        // 结束所有Python进程
        Kill_All_Python_Process();
    }
}
