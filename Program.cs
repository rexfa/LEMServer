using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LEMServer.TcpServer;

namespace LEMServer
{
    /// <summary>
    /// ������
    /// ע��Linux��װ��ҪICU��
    /// yum install icu -y
    /// ����ʹ��΢���ṩ��dotnet-install.sh -c  5.0 ��װ����
    /// ���뻷��
    /// export DOTNET_ROOT=$HOME/.dotnet
    /// export PATH =$PATH:$HOME/.dotnet
    /// ע��LEMServer.XML��λ�úͷ���ǽ��
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //var configurationBuilder = new ConfigurationBuilder().AddJsonFile("config.json",true,true);
            //var configuration = configurationBuilder.Build();
            ////configuration.GetSection("TCPServerConfig");
            //Console.WriteLine("Port is " + configuration.GetSection("TCPServerConfig").GetValue<int>("Port").ToString());
            //DMServer dMServer = new DMServer(configuration.GetSection("TCPServerConfig").GetValue<int>("Port"));
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration(config =>
                {
                    //��ȡurl����
                    config.AddJsonFile("config.json", true, true);
                });
    }
}
