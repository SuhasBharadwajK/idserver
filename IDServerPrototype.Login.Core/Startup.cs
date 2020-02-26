using IdentityServer4;
using IDServerPrototype.Login.Core.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace IDServerPrototype.Login.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            string connectionString = Configuration["ConnectionStrings:IdentityDbConnection"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddOperationalStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Core.Configuration.Resources.GetIdentityResources())
                .AddInMemoryApiResources(Core.Configuration.Resources.GetApiResources())
                .AddAspNetIdentity<AppUser>()
                .AddDeveloperSigningCredential();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie("cookie")
            .AddOpenIdConnect("Office365", options =>
            {
                options.ClientId = "Office365Client";
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                options.ClientId = Configuration["AuthProviders:Office365:ClientId"];
                options.Authority = Configuration["AuthProviders:Office365:Authority"];

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.ReturnUrlParameter = "/Account/ExternalLoginCallback";

                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "email",
                    RoleClaimType = "role",
                    ValidateIssuer = false
                };
            });

            services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            // app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
            });

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
