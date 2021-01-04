using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LEMServer.TcpServer;
using LEMServer.Services;
using System.IO;

namespace LEMServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Console.WriteLine(configuration.GetSection("TCPServerConfig").GetValue<int>("Port").ToString());
            DMServer dMServer = new DMServer(configuration.GetSection("TCPServerConfig").GetValue<int>("Port"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 框架本身提供了一个 DI（依赖注入）系统
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //for webapi
            services.AddControllers();
            //services.AddSingleton(new DMServer());
            services.AddScoped<ISendCommandService, SendCommandService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LEMServer", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "LEMServer.xml");
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();//"/swagger"
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LEMServer v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //强制使用https
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                //测试期间都给webapi暴露
                app.UseSwagger();//"/swagger"
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LEMServer v1"));
            }
            //强制使用https的跳转，对于Api来说没必要使用强制https
            //app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //for webapi
                endpoints.MapControllers();
            });
        }
    }
}
