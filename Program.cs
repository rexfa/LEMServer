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
    /// 主程序
    /// 注意Linux安装需要ICU包
    /// yum install icu -y
    /// 可以使用微软提供的dotnet-install.sh -c  5.0 安装环境
    /// 导入环境
    /// export DOTNET_ROOT=$HOME/.dotnet
    /// export PATH =$PATH:$HOME/.dotnet
    /// 注意LEMServer.XML的位置和防火墙等
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
                    //读取url配置
                    config.AddJsonFile("config.json", true, true);
                });
    }
}
