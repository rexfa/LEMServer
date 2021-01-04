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
        // ��ܱ����ṩ��һ�� DI������ע�룩ϵͳ
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
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
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
                //ǿ��ʹ��https
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                //�����ڼ䶼��webapi��¶
                app.UseSwagger();//"/swagger"
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LEMServer v1"));
            }
            //ǿ��ʹ��https����ת������Api��˵û��Ҫʹ��ǿ��https
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
