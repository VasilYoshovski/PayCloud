using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PayCloud.Data.DbContext;
using PayCloud.Services.Mappers;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;

namespace PayCloud.WebApp
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.Configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            this.logger.LogInformation("Added AddAuthentication() to services");

            bool useAzure = this.Configuration.GetSection("EnableAZURE").GetValue<bool>("IsEnabled");
            if (useAzure)
            {
                services.AddDbContext<PayCloudDbContext>(options =>
                    options.UseSqlServer(this.Configuration.GetConnectionString("AzureConnection")));
            }
            else
            {
                services.AddDbContext<PayCloudDbContext>(options =>
                    options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            }
            this.logger.LogInformation("Added AddDbContext() to services");

            // Add application services.

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            this.logger.LogInformation("Added Configure<CookiePolicyOptions>() to services");

            services.AddIdentityServices();

            services.AddBusinessServices();

            services.AddCustomMappers();
            services.AddDtoMappers();

            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.AreaViewLocationFormats.Clear();
            //    options.AreaViewLocationFormats.Add("/Admin/{2}/Views/{1}/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/Admin/{2}/Views/Shared/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/User/{2}/Views/{1}/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/User/{2}/Views/Shared/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

            //});

            //services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            this.logger.LogInformation("Added AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2) to services");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                this.logger.LogInformation("In Development environment");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                this.logger.LogInformation("In Debug environment");
            }

            app.UseErrorHandlingMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "adminLogin",
                    template: "adminLogin",
                    defaults: new { area = "admin", controller = "Login", action = "AdminLogin" });

                routes.MapRoute(
                    name: "error",
                    template: "error",
                    defaults: new { controller = "Home", action = "Error" });

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
