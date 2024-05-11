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




            // Initial DB Fix
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Apply migrations and create/update the database
                    await context.Database.MigrateAsync();

                    // Seed roles
                    var roles = new[] { "Staff", "Customer" };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    // Seed users
                    await SeedUsersAsync(userManager);
                }
                catch (Exception ex)
                {
                    // Log any errors
                    Console.WriteLine("An error occurred while seeding the database: " + ex.Message);
                }
            }
            // Initial DB Fix code ends here


            app.Run();
        }

        // Method to seed users
        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            // User seeding logic here
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
    }
}
