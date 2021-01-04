using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using LEMServer.TcpServer.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LEMServer.TcpServer
{
    public class ProtocolProcessor
    {
        const char Space = ' ';
        const char Comma = ',';

        public static void DeviceInformationProcessing(IAsyncResult ar) 
        {
            StateObject stateObject = (StateObject)ar.AsyncState;
            Socket clientSocket = stateObject.workSocket;
            int count = 0;
            DMProtocol dmProtocol;
            count = clientSocket.EndReceive(ar);
            string msg = Encoding.UTF8.GetString(stateObject.buffer, 0, count);
            //Console.WriteLine(msg);
            
            

            //string dData = stateObject.stringBuilder.ToString();
            string[] dataString = msg.Split(Comma);
            //头，版本，命令，ID，stat
            if (dataString.Count()<4)
            {
                throw new Exception("Incomplete message");
            }
            if (!dataString[0].Equals(LEMServer.TcpServer.Protocol.HEADER.SXZL.ToString()))
            {
                throw new Exception("The message does not conform to the agreement");
            }
            try
            {
                dmProtocol = new DMProtocol(msg);
                Console.WriteLine("Deal String Success : " + msg);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Deal String failure : " + msg);
                throw ex;
            }
            //Console.WriteLine("Deal String : " + msg);
            COMMAND command = (COMMAND)int.Parse(dataString[2]);
            switch (command)
            {
                case COMMAND.REGISTER:
                    //头，版本，命令，ID，devicestat,powerstat,port
                    //0   1     2      3   4          5         6
                    //   0    1    2   3      4   5                                         6          7
                    //"SXZL,V0100,01,DM77712,3696,00," + DateTime.Now.ToString("yyyyMMddHHmmss") + ",9910";
                    //DeviceRegistration(dataString[3], dataString[1],int.Parse(dataString[4]), int.Parse(dataString[5]), ((IPEndPoint)stateObject.workSocket.RemoteEndPoint).Address.ToString(), int.Parse(dataString[7]));
                    DeviceRegistration(dmProtocol.DeviceID, dmProtocol.Version, dmProtocol.DeviceStat, dmProtocol.PowerStat,((IPEndPoint)stateObject.workSocket.RemoteEndPoint).Address.ToString(), dmProtocol.Port);
                    //RepDeviceRegistration(dataString[2], dataString[1], int.Parse(dataString[4]), int.Parse(dataString[5]), clientSocket);
                    RepDeviceRegistration(dmProtocol.DeviceID, dmProtocol.Version, dmProtocol.DeviceStat, dmProtocol.PowerStat, clientSocket);
                    break;
                case COMMAND.READCARD:
                    Readcard(dmProtocol.DeviceID, dmProtocol.Version, dmProtocol.DeviceStat, dmProtocol.PowerStat, dmProtocol.Card);
                    Readcard(dmProtocol.DeviceID, dmProtocol.Version, dmProtocol.DeviceStat, dmProtocol.PowerStat, dmProtocol.Card);
                    break;
            }
            Thread.Sleep(300);
            try
            {
                clientSocket.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void DeviceRegistration(string deviceID, string deviceVer, int deviceStat,int powerStat,string ip,int port)
        {
            DeviceInformation deviceInformation = new DeviceInformation() { DeviceID = deviceID, DeviceStat = deviceStat, DeviceVer = deviceVer, IP = ip,PowerStat = powerStat, InformationUpdatedOn = DateTime.Now,port=port };
            MemoryCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60);
            cache.Set(deviceInformation.DeviceID, deviceInformation, policy);
        }
        public static void RepDeviceRegistration(string deviceID, string deviceVer, int deviceStat, int powerStat, Socket deviceSocket) 
        {
            string rep = LEMServer.TcpServer.Protocol.HEADER.XXZL.ToString() + Comma + deviceVer + Comma + (int)COMMAND.REP_REGISTER + Comma + deviceStat + Comma + powerStat.ToString("00") + Comma + DateTime.Now.ToString("yyyyMMddHHmmss");
            deviceSocket.Send(Encoding.UTF8.GetBytes(rep));
        }

        public static void Readcard(string deviceID, string deviceVer, int deviceStat, int powerStat, string card)
        {

        }
        public static void RepReadcard(string deviceID, string deviceVer, int deviceStat, int powerStat, string card)
        {

        }


    }
}
