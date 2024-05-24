using GraduateProject.Domain.Core;
using GraduateProject.Domain.Interfaces;
using GraduateProject.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Security.Claims;

namespace MentoryGraduateProject_WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connection = "Server=(localdb)\\mssqllocaldb;Database=GraduateProject;Trusted_Connection=True;";
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/HomeError";
                    options.LoginPath = "/login";
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

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

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapGet("/login", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                // html-форма для ввода логина/пароля
                string loginForm = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='utf-8'/>
                            <title> METANIT.COM </title>
                        </head>
                        <body>
                            <h2>Login Form</h2>
                            <form method='post'>
                                <p>
                                    <label>Login</label><br />
                                    <input name='login' />
                                </p>
                                <p>
                                    <label>Password</label><br />
                                    <input type='password' name='password'
                                />
                                </p>
                                <input type='submit' value='Login' />
                            </form>
                                </body>
                          </html>";
                await context.Response.WriteAsync(loginForm);
            });

            app.MapPost("/login", async (string? returnUrl, HttpContext context, IUserRepository userRepo) =>
            {
                var form = context.Request.Form;
                // если email и/или пароль не установлены, посылаем статусный код ошибки 400
                if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                    return Results.BadRequest("Email и/или пароль не установлены");

                string login = form["login"];
                string password = form["password"];

                // находим пользователя
                User? person = userRepo.GetUserByLoginAndPassword(login, password);
                if (person is null) return Results.Unauthorized();

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
                // создаем объект ClaissIdentity

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                // установка аутентификационных куки

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Results.Redirect("/");
            });

            app.Run();
        }
    }
}
