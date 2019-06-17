using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services;
using PayCloud.Services.Contracts;
using PayCloud.Web.Mappers;
using PayCloud.Web.Services;

namespace PayCloud.Web
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

            services.AddIdentity<PayCloudUser, IdentityRole>()
                .AddEntityFrameworkStores<PayCloudDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IClientService, ClientService>();

            services.AddCustomMappers();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Admin/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Admin/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
