﻿using Business;
using Business.ServicesInterfaces;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesProvider;

namespace Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BusinessConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<IUserStatusRepository, UserStatusRepository>();
            services.AddTransient<IPresenceRepository, PresenceRepository>();
            services.AddTransient<ILectureRepository, LectureRepository>();
            services.AddTransient<IAnswerRepository, AnswerRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IKataRepository, KataRepository>();
            services.AddTransient<ILectureService, LectureService>();
            services.AddTransient<IKataService, KataService>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<IRecordService, RecordService>();
            services.AddTransient<IUploadService, UploadService>();
	        services.AddTransient<IUserStatusService, UserStatusService>();
            services.AddTransient<IAttendanceRepository, AttendanceRepository>();
            services.AddTransient<IUserAttendanceService, UserAttendanceService>();
            services.AddTransient<IPresenceService, PresenceService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();

            /*
                        const string connection = @"Server = .\SQLEXPRESS; Database = Project.Development; Trusted_Connection = true;";

                        services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
                        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            */


            // ATTENTION services.AddSingleton<UserManager<ApplicationUser>>(); 

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            MyIdentityDataInitializer.SeedData(roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // DON'T REMOVE THIS WITHOUT RADU'S PERMISSION v

            //IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            //using (IServiceScope scope = scopeFactory.CreateScope())
            //{
            //    using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            //    {
            //        // seed database code goes here

            //        context.Seed(roleManager);
            //    }
            //}
        }
    }
}