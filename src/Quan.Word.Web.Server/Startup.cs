using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Quan.Word.Web.Server
{
    public class Startup
    {
        // Called by Program.cs UseStartup
        public Startup(IConfiguration configuration)
        {
            IoCContainer.Configuration = configuration;
        }

        /// <summary>
        // This method gets called by the runtime. Use this method to add services to the container.
        /// <param name="services">The Dependency Injection container</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add ApplicationDbContext to Dependency Injection supported by EntityFrameworkCore
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(IoCContainer.Configuration.GetConnectionString("DefaultConnection")));

            // AddIdentity adds cookie based authentication
            // Adds scoped classes for things like UserManager, SignInManager, PasswordHashes etc...
            // NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
            // Return: IdentityBuilder
            // https://github.com/aspnet/Identity/blob/master/src/Identity/IdentityServiceCollectionExtensions.cs
            services.AddIdentity<ApplicationUser, IdentityRole>()

                // Adds UserStore and RoleStore from this context
                // That are consumed by the UserManager and RoleManager
                // https://github.com/aspnet/Identity/blob/master/src/EF/IdentityEntityFrameworkBuilderExtensions.cs
                .AddEntityFrameworkStores<ApplicationDbContext>()

                // Adds a provider that generates unique keys and hashes for things like
                // forgot password links, phone number verification codes etc...
                .AddDefaultTokenProviders();


            // Add JWT Authentication for api client
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = IoCContainer.Configuration["Jwt:Issuer"],
                    ValidAudience = IoCContainer.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"])),
                };
            });

            // Change password policy
            services.Configure<IdentityOptions>(option =>
            {
                // Make really weak password possible
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 5;
                option.Password.RequireLowercase = true;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
            });

            // Change login URL
            services.ConfigureApplicationCookie(options =>
            {
                // Redirect to /login
                options.LoginPath = "/login";

                // Change cookie timeout to expire in 15 seconds
                options.ExpireTimeSpan = TimeSpan.FromSeconds(1500);
            });


            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // Store instance of the DI service provider so our application can access it any where
            IoCContainer.Provider = serviceProvider;

            // Setup Identity
            app.UseAuthentication();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

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
