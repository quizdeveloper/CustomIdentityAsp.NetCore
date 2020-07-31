using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Bsl.Dal;
using EmployeeManager.Bsl.Identity;
using EmployeeManager.Bsl.Model;
using EmployeeManager.Bsl.Utils;
using EmployeeManager.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManager
{
    public class Startup
    {
        [Obsolete]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment evm)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(evm.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{evm.EnvironmentName}.json", true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();

            AppSettings.Instance.SetConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddIdentity<AppUser, AppRole>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(90);
                options.LoginPath = "/";
                options.AccessDeniedPath = "/404";
                options.SlidingExpiration = true;
               // options.Events = new MyCookieAuthenticationEvents();

            });

            services.AddTransient<IUserStore<AppUser>, UserStoreAppService>();
            services.AddTransient<IRoleStore<AppRole>, RoleAppService>();
            services.AddScoped<IPasswordHasher<AppUser>, PasswordHasherOverride<AppUser>>();
            services.AddScoped<IEmployeeDal, EmployeeDal>();
            services.AddScoped<IDbHelper, DbHelper>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipal>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employee}/{action=Login}/{id?}");
            });
        }
    }
}
