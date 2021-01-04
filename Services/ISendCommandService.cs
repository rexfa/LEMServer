using LEMServer.TcpServer.Protocol;

namespace LEMServer.Services
{
    interface ISendCommandService
    {
        string SendCommandToDeivce(string id, COMMAND command,STAT stat);
    }
}
