using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Database.Infrastructure;
using Hospital.SharedModel.Repository.Base;
using Hospital.SharedModel.Repository.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Hospital.Database.EfStructures;
using Hospital.SharedModel.Model;
using HospitalApi.HttpRequestSenders;
using HospitalApi.HttpRequestSenders.Implementation;
using Microsoft.AspNetCore.Identity;
using Hospital.Schedule.Service.ServiceInterface;
using Hospital.Schedule.Service;
using Hospital.Schedule.Service.Interfaces;
using Hospital.SharedModel.Service;
using Hospital.SharedModel.Service.Implementation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Hospital.MedicalRecords.Service;
using Hospital.MedicalRecords.Service.Interfaces;
using Hospital.EventStoring.Service.Interfaces;
using Hospital.EventStoring.Service;
using System.IO;

namespace HospitalApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("MyCorsImplementationPolicy", builder => builder.WithOrigins("*")));

            services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HospitalApi", Version = "v1" });
            });

            services.AddDbContextPool<AppDbContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("HOSPITAL_DB_PATH");
                options.UseNpgsql(connectionString);
                using var context = new AppDbContext((DbContextOptions<AppDbContext>)options.Options);
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            });

            services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.SignIn.RequireConfirmedAccount = true;
                }).AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
            });
            services.AddHostedService<ConsumeScopedServiceHostedService>();
            services.AddScoped<IPatientSurveyService, PatientSurveyService>();
            services.AddScoped<IScheduledEventService, ScheduledEventService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IScheduleAppointmentService, ScheduleAppointmentService>();
            services.AddScoped<IReportService, ReportService>();  
            services.AddScoped<IEventStoringService, EventStoringService>();

           


            var builder = new ContainerBuilder();

            builder.RegisterModule(new RepositoryModule()
            {

                RepositoryAssemblies = new List<Assembly>()
                {
                    typeof (CityReadRepository).Assembly
                },
                Namespace = "Repository"


            });

            InitializeDbForTests(services);
            
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<HttpRequestSender>().As<IHttpRequestSender>();
            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        static void InitializeDbForTests(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            ExecuteScript(db);
        }
        private static void ExecuteScript(DbContext db)
        {
            db.Database.EnsureCreated();
            try
            {
                var scriptFiles = Directory.GetFiles("../TestData/");
                var script = string.Join('\n', scriptFiles.Select(File.ReadAllText));
                db.Database.ExecuteSqlRaw(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred seeding the database with test data. Error: { " + ex.Message + " }. ");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HospitalApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyCorsImplementationPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
