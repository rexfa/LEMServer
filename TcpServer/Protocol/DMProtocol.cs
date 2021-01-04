using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LEMServer.TcpServer.Protocol
{
    public class DMProtocol
    {
        const char Comma = ',';
        public string Header { get; set; }
        public string Version { get; set; }
        public int Command { get; set; }
        public string DeviceID { get; set; }
        public int DeviceStat { get; set; }
        public int PowerStat { get; set; }
        public string DatetimeString { get; set; }
        public string[] OtherInfo { get; set; }
        public int OtherInfoCount { get; set; }
        public DMProtocol(string message)
        {
            string[] dataString = message.Split(Comma);
            Header = dataString[0];
            Version = dataString[1];
            Command = int.Parse(dataString[2]);
            DeviceID = dataString[3];
            DeviceStat = int.Parse(dataString[4]);
            PowerStat = int.Parse(dataString[5]);
            DatetimeString = dataString[6];
            OtherInfo = new string[1024];
            //存一下其他数据的数量
            OtherInfoCount = dataString.Length - 7;
            Array.Copy(dataString, 7, OtherInfo, 0, dataString.Length - 7);
        }
        public int Port 
        {
            get
            {
                if (Command == (int)COMMAND.REGISTER)
                    return int.Parse(OtherInfo[0]);
                else
                    throw new Exception("This message is not registration information.");
            } 
        }
        public string Card
        {
            get
            {
                if (Command == (int)COMMAND.READCARD)
                    return OtherInfo[0];
                else
                    throw new Exception("This message is not registration information.");
            }
        }

    }
    public enum HEADER
    {
        SXZL,//主机到服务器
        XXZL //服务器到主机
    }
    public enum VERSION
    {
        V0100
    }
    public enum COMMAND
    {
        REGISTER = 01, //注册
        KEEPLIVE = 02, //心跳
        READCARD = 03, //读卡
        REP_REGISTER = 81, //注册响应
        REP_KEEPLIVE = 82, //心跳响应
        REP_READCARD = 83  //读卡响应


    }
    public enum STAT
    {
        IDLE = 3696,//空闲
        EXPERIMENTING = 3697,//实验中
        TRAINING = 3701,//培训中
        TEACHING = 3705, //教学中
        MAINTAINING = 3703,//维护中
        COMMISSIONEDEXPERIMENT = 3707 //委托实验中

    }
}
