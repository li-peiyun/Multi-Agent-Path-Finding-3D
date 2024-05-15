using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEditor;

public class PyRun : MonoBehaviour
{

    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;

    // Start is called before the first frame update
    void Start()
    {
        Kill_All_Python_Process();

        // ������cbs�����ͨ��
        EstablishPythonCommunication("Python/cbs/cbs.py", 31415, "cbs");

        // ������primal�����ͨ��
        EstablishPythonCommunication("Python/primal/mapgenerator.py", 31416, "primal");
    }

    // Update is called once per frame
    void Update()
    {
        // ����C������"cbs.py"����ʶ��ָ��
        if (Input.GetKeyDown(KeyCode.C))
        {
            SendMessageToPython("Recognizing", 31415); // ������Ϣ��cbs.py�Ķ˿ں�31415
        }

        // ����P������"mapgenerator.py"����ʶ��ָ��
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendMessageToPython("Recognizing", 31416); // ������Ϣ��mapgenerator.py�Ķ˿ں�31416
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

    void SendMessageToPython(string message, int port)
    {
        // ������Ϣ���ݵ��ֽ�����
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        // ������Ϣ��ָ���˿�
        udpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        UnityEngine.Debug.Log("Sent message: " + message);
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            UnityEngine.Debug.Log(e.Data);
            if (e.Data == "StartRecognition")
            {
                print("Recognizing");
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
