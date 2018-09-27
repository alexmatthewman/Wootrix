using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WootrixV2.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

namespace Wootrix
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("IdentityConnection")));
            
            //This allows for custom Identity attributes
            //services.AddDefaultIdentity<ApplicationUser>().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            //services.AddDefaultIdentity<ApplicationUser>().AddDefaultUI().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
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
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "companyHome",
                    template: "{controller=Company}/{action=Home}/{id}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });

            CreateUserRoles(services).Wait();
        }

        /// <summary>
        /// This creates the roles (if they don't exist) and adds some people to them 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roleCheck = true;

            IdentityResult roleResult;

            //Create the roles and seed them to the database 

            //Adding Super Admin Role  
            roleCheck = await RoleManager.RoleExistsAsync("Admin");
            
            if (!roleCheck)
            {

                var adminRole = await RoleManager.FindByNameAsync("Admin");
                var claimCheck = await RoleManager.GetClaimsAsync(adminRole);
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("Admin");
                    await RoleManager.CreateAsync(adminRole);
                    var clm = new Claim("Role", "Admin");
                    if (!claimCheck.Contains(clm))
                    {
                        await RoleManager.AddClaimAsync(adminRole, new Claim("Role", "Admin"));
                    }                    
                }                
            }
            

            //Adding Company Admin Role  
            roleCheck = await RoleManager.RoleExistsAsync("CompanyAdmin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("CompanyAdmin"));
            }

            //Adding User Role  
            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }

            //Assign Super Admin role to the main User for Admin management  
            try
            {
                
                ApplicationUser user = await UserManager.FindByEmailAsync("amazon@wootrix.com");
                var ac = await UserManager.GetClaimsAsync(user);

                
                // Main user has all roles
                await UserManager.AddToRoleAsync(await UserManager.FindByEmailAsync("amazon@wootrix.com"), "Admin");
                await UserManager.AddToRoleAsync(await UserManager.FindByEmailAsync("wootrixCompanyAdmin@wootrix.com"), "CompanyAdmin");

                

                // If the Admin claim is in the DB for the user above don't bother to re-add it
                if (ac.Where(s => s.Value == "Admin").FirstOrDefault().Value != "Admin")
                {
                    await UserManager.AddToRoleAsync(user, "Admin");
                    await UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
                }

                user = await UserManager.FindByEmailAsync("wootrixCompanyAdmin@wootrix.com");
                ac = await UserManager.GetClaimsAsync(user);

                // If the Admin claim is in the DB for the user above don't bother to re-add it
                if (ac.Where(s => s.Value == "ComanyAdmin").FirstOrDefault().Value != "ComanyAdmin")
                {
                    await UserManager.AddToRoleAsync(user, "ComanyAdmin");
                    await UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "ComanyAdmin"));
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                //Maybe the users have been deleted or something wierd - shouldn't crash in any case                             
            }



        }
    }
}
