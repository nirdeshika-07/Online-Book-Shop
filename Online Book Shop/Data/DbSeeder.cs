using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Online_Book_Shop.Services;

namespace Online_Book_Shop.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userManagers = service.GetService<UserManager<IdentityUser>>();
            var roleManagers = service.GetService<RoleManager<IdentityRole>>();
            //Adding some roles to database
            await roleManagers.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManagers.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //Creating admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email="admin@gmail.com",
                EmailConfirmed=true
            };
            var userInDB = await userManagers.FindByEmailAsync(admin.Email);
            if (userInDB is null)
            {
                await userManagers.CreateAsync(admin, "Admin@123");
                await userManagers.AddToRoleAsync(admin,Roles.Admin.ToString());
            }
        }
    }
}
