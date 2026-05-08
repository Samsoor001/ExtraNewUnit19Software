using Fantasy_Encyclopedia.Core.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Fantasy_Encylopedia_App.Data;
using Microsoft.AspNetCore.Identity;
using Fantasy_Encylopedia_App.Services;
using System.Linq;

namespace Fantasy_Encylopedia_App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddTransient<CsvService>();
            builder.Services.AddSingleton<ComicQueryService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<AnalyticsService>();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 70 * 1024 * 1024;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 70 * 1024 * 1024;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeed.SeedRolesAsync(services);
            }

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("FBZStaff"))
                {
                    await roleManager.CreateAsync(new IdentityRole("FBZStaff"));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (!await roleManager.RoleExistsAsync("FBZStaff"))
                {
                    await roleManager.CreateAsync(new IdentityRole("FBZStaff"));
                }

                var staffEmail = "staff2@fbz.com";
                var staffPassword = "FbZStaff123!";

                var staffUser = await userManager.FindByEmailAsync(staffEmail);

                if (staffUser == null)
                {
                    staffUser = await userManager.FindByNameAsync(staffEmail);
                }

                if (staffUser == null)
                {
                    staffUser = new IdentityUser
                    {
                        UserName = staffEmail,
                        Email = staffEmail,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(staffUser, staffPassword);

                    if (!createResult.Succeeded)
                    {
                        throw new Exception("Failed to create staff user: " +
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    }
                }

                if (!await userManager.IsInRoleAsync(staffUser, "FBZStaff"))
                {
                    await userManager.AddToRoleAsync(staffUser, "FBZStaff");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}