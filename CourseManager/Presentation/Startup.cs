using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Data;
using Presentation.Models;
using Presentation.Services;
using Business;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesNews;
using Data.Persistance;
using RecordServiceProvider;

namespace Presentation {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BusinessConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();
            services.AddTransient<IPresenceRepository, PresenceRepository>();
            services.AddTransient<IFactionRepository, FactionRepository>();
            services.AddTransient<ILectureRepository, LectureRepository>();
            services.AddTransient<IAnswerRepository, AnswerRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<INewsRepository,  NewsRepository>();
            services.AddTransient<IRecordService, RecordService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
