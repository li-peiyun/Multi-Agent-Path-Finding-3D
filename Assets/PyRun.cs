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

        // 创建与cbs程序的通信
        EstablishPythonCommunication("Python/cbs/cbs.py", 31415, "cbs");

        // 创建与primal程序的通信
        EstablishPythonCommunication("Python/primal/mapgenerator.py", 31416, "primal");
    }

    // Update is called once per frame
    void Update()
    {
        // 按下C键，向"cbs.py"发送识别指令
        if (Input.GetKeyDown(KeyCode.C))
        {
            SendMessageToPython("Recognizing", 31415); // 发送消息到cbs.py的端口号31415
        }

        // 按下P键，向"mapgenerator.py"发送识别指令
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendMessageToPython("Recognizing", 31416); // 发送消息到mapgenerator.py的端口号31416
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

    void SendMessageToPython(string message, int port)
    {
        // 创建消息内容的字节数组
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        // 发送消息到指定端口
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
            // 调试语句
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
