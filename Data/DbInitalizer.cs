
using Core.Constants.Enums;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Data
{
    public static class DbInitalizer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            AddRoles(roleManager);
            AddAdmin(userManager, roleManager);
        }
        private static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    _ = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    }).Result;
                }
            }
        }

        private static void AddAdmin(UserManager<User> userManager,RoleManager<IdentityRole>roleManager)
        {
            if (userManager.FindByEmailAsync("admin@gmail.com").Result is null)
            {
                var user = new User
                {
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    Name="Admin",
                    Surname="Admin"
                };

                var userResult = userManager.CreateAsync(user,"Salam123!").Result;
                if (!userResult.Succeeded) throw new Exception("admin yaradıla bilinmədi");

                var role= roleManager.FindByNameAsync("Admin").Result;
                if (role is null) throw new Exception(" admin rolu tapılmadı");

                var addRoleToUserResult=userManager.AddToRoleAsync(user,role.Name).Result;
                if (!addRoleToUserResult.Succeeded) throw new Exception("istifadəçiyə admin rolu əlavə etmək mümkün olmadı");
                user.EmailConfirmed = true;
                var updateResult=userManager.UpdateAsync(user).Result;
                if (!updateResult.Succeeded) throw new Exception("yenilənmə uğursuz oldu");
            }
        }
    }
}
