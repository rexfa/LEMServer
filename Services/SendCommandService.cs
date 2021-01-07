using LEMServer.TcpServer.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using LEMServer.TcpServer;
using System.Net;
using System.Net.Sockets;

namespace LEMServer.Services
{
    public class SendCommandService : ISendCommandService
    {
        public string SendCommandToDeivce(string id, COMMAND command, STAT stat)
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Count() > 0)
            {
                //Message = cache.Count() + " device(s) have been registered";
                //foreach (var oc in cache.Distinct())
                //{
                //    DeviceInformation deviceInformation = (DeviceInformation)oc.Value;
                //    DeviceInfoList.Add("ID: " + deviceInformation.DeviceID + "-IP: " + deviceInformation.IP + " -DeviceStat: " + deviceInformation.DeviceStat + " -PowerStat: " + deviceInformation.PowerStat.ToString("00") + " -Ver: " + deviceInformation.DeviceVer);
                //}
                DeviceInformation deviceInformation = (DeviceInformation)cache.Get(id);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceInformation.IP), deviceInformation.port);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ipEndPoint);
                switch (COMMAND)
                {
                    case COMMAND.KEEPLIVE:
                        if (cmd.Equals("111"))
                        {
                            //    0    1     2   3      4     5                                         6      7
                            msg = "SXZL,V0100,01,DM77712,3696,00," + DateTime.Now.ToString("yyyyMMddHHmmss") + ",9910";
                        }
                        try
                        {
                            clientSocket.Send(Encoding.UTF8.GetBytes(msg));
                            data = new byte[1024];
                            count = clientSocket.Receive(data);
                            msg = Encoding.UTF8.GetString(data, 0, count);
                            Console.WriteLine("Receive:" + msg);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                }
            }
            else
            {
                return "Device pool is empty";
            }
        }
    }
}
