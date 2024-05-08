using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystem.Data;


namespace RestaurantReservationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

      


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MenuItems}/{action=Index}/{id?}");
            app.MapRazorPages();

            //Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Staff", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            //Seed Tester User
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string adminEmail = "admin@tester.com";
                string adminPassword = "TestAdmin1;";



                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser(adminEmail);
                    user.UserName = adminEmail;
                    user.Email = adminEmail;

                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, "Staff");
                }

                string customerEmail = "customer@tester.com";
                string customerPassword = "TestCustomer1;;";

                if (await userManager.FindByEmailAsync(customerEmail) == null)
                {
                    var user = new IdentityUser(customerEmail);
                    user.UserName = customerEmail;
                    user.Email = customerEmail;

                    await userManager.CreateAsync(user, customerPassword);
                    await userManager.AddToRoleAsync(user, "Customer");
                }

         
            }

            app.Run();
        }
    }
}
