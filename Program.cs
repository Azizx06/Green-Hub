using JeddahGreenHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JeddahGreenHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();


            // Adding Session and Cookies

            // This is important for storing the session data in the server memory.
            builder.Services.AddDistributedMemoryCache();
            // This line exactly registers the session services in the application...
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(300); // 5 mins
                options.Cookie.HttpOnly = true;      // prevents JavaScript from accessing the cookies.
                options.Cookie.IsEssential = true;   // indicates session cookie is essential for application to function properly
                options.Cookie.SameSite = SameSiteMode.Strict; // cookie will only be sent in first-party contexts (same site), helping to prevent CSRF (Cross-Site Request Forgery) attacks.
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            // Maybe add app.UseAuthentication(); ?
            app.UseAuthorization();

            // ENABLE Session State management for the application...
            app.UseSession();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

           


            app.Run();
        }
    }
}
