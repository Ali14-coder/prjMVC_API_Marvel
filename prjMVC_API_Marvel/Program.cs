using Microsoft.AspNetCore.Authentication.Cookies;
using prjMVC_API_Marvel.Services;

namespace prjMVC_API_Marvel
{
    public class Program
    {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

            //Configure cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; //Redirect here if not authentication
            });

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddHttpClient<ApiService>(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:7038/swagger/index.html");
                });

            //Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

                app.Run();
            }
        }
    }
//use incognito for token