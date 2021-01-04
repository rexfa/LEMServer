using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LEMServer.TcpServer
{
    public class DMServer
    {
        DMServerThread dmServerThread;
        Thread serverThread;
        public DMServer(int port)
        {
            dmServerThread = new DMServerThread(port);
            serverThread = new Thread(dmServerThread.Run);
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        public DMServer()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            if (serverThread == null)
            {
                dmServerThread = new DMServerThread(configuration.GetSection("TCPServerConfig").GetValue<int>("Port"));
                serverThread = new Thread(dmServerThread.Run);
                serverThread.IsBackground = true;
                serverThread.Start();
            }
            else
            {
                Console.WriteLine("DMServer is runing!");
            }
        }
        public string GetServerStat()
        {
            if (serverThread == null)
            {
                return "Service has not been started.";
            }
            return serverThread.ThreadState.ToString();
        }
    }
    public class DMServerThread
    {
        
        int port;
        public DMServerThread(int port)
        {
            this.port = port;
        }
        public void Run()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("DMServer start on " + port.ToString());
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            //将 Socket 置于侦听状态,自动确定挂起的连接队列的最大长度。
            serverSocket.Listen(1000);
            //while (true)
            //{
            //开始接受连接，异步
            serverSocket.BeginAccept(new AsyncCallback(AcceptReceiveCallback), serverSocket);
            //}
        }
        public static void AcceptReceiveCallback(IAsyncResult ar)
        {
            //初始化一个SOCKET，用于其它客户端的连接
            Socket serverSocket = (Socket)ar.AsyncState;
            byte[] Buffer;
            int bytesTransferred;
            Socket clientSocket = serverSocket.EndAccept(out Buffer, out bytesTransferred, ar);
            //将要发送给连接上来的客户端的提示字符串
            //string strDateLine = "Welcome here";
            //Byte[] byteDateLine = System.Text.Encoding.ASCII.GetBytes(strDateLine);
            //将提示信息发送给客户端
            //clientSocket.Send(byteDateLine, byteDateLine.Length, 0);
            StateObject state = new StateObject();
            state.workSocket = clientSocket;
            clientSocket.BeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0,new AsyncCallback(ReadCallback), state);
            serverSocket.BeginAccept(new AsyncCallback(AcceptReceiveCallback), serverSocket);
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket clientSocket = state.workSocket;
            int count=0;
            try
            {
                //count = clientSocket.EndReceive(ar);
                //string msg = Encoding.UTF8.GetString(state.buffer, 0, count);
                //Console.WriteLine(msg);
                try
                {
                    //导入数据处理器
                    ProtocolProcessor.DeviceInformationProcessing(ar);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (System.Net.Sockets.SocketException se)
            {
                Console.WriteLine(se.Message);
            }
            
            if (count > 0)
            {
                state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, count));
                clientSocket.BeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0,
                                         new AsyncCallback(ReadCallback), state);
            }
            else
            {
                if (state.stringBuilder.Length > 1)
                {
                    //All of the data has been read, so displays it to the console
                    string strContent;
                    strContent = state.stringBuilder.ToString();
                    Console.WriteLine(String.Format("Read {0} byte from socket" + "data = {1} ", strContent.Length, strContent));

                }
                clientSocket.Close();
            }
        }


    }
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BUFFER_SIZE = 1024;
        public byte[] buffer = new byte[BUFFER_SIZE];
        public StringBuilder stringBuilder = new StringBuilder();
    }
}
