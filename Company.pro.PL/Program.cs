using Company.pro.BLL;
using Company.pro.BLL.Interfaces;
using Company.pro.BLL.Repositories;
using Company.pro.DAL.Data.Contexts;
using Company.pro.DAL.Models;
using Company.pro.PL.Helper;
using Company.pro.PL.Mapping;
using Company.pro.PL.Services;
using Company.pro.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.pro.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-In MVC Services 
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For CompanyDbContext  
            builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddScoped<IScopedService,ScopedService>(); //Per Requset
            builder.Services.AddScoped<ITransentService,TransentService>(); //Per Operation 
            builder.Services.AddScoped<ISengeltonService,SengeltonService>(); // Per App
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddScoped<ITwilioService, TwilioService>();
            builder.Services.ConfigureApplicationCookie(Config =>
            {
                Config.LoginPath = "/Account/SignIn";
            });

            //         builder.Services.AddAuthentication(options =>
            //{
            //             options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddGoogle( o => {
            //             o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            //             o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            //         });


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
