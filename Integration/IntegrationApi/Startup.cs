using Autofac;
using Autofac.Extensions.DependencyInjection;
using Integration.Database.Infrastructure;
using Integration.Partnership.Service;
using Integration.Shared.Repository.Base;
using Integration.Shared.Repository.Implementation;
using Integration.Shared.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IntegrationAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            string sourceFolder = Path.Combine(Directory.GetCurrentDirectory(), "MedicineReports");
            string targetZip = Path.Combine(Directory.GetCurrentDirectory(), "Archive", DateTime.Now.Ticks + ".zip");

            FileZipService fileZipService = new FileZipService();
            fileZipService.FileZip(sourceFolder, targetZip);
        }

        

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegrationApi", Version = "v1" });
            });
            services.AddHostedService<BenefitRabbitMqService>();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DbModule());
            builder.RegisterModule(new RepositoryModule()
            {

                RepositoryAssemblies = new List<Assembly>()
                {
                    typeof (CityReadRepository).Assembly
                },
                Namespace = "Repository"


            });
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IntegrationApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
