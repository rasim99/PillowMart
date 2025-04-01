
using Business.Models.Admin.User;
using Business.Services.Abstract;
using Core.Constants.Enums;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ModelStateDictionary _modelState;
        public UserService(UserManager<User>userManager,
            RoleManager<IdentityRole> roleManager,
            IActionContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _modelState=contextAccessor.ActionContext.ModelState;
        }

     

        public async Task<UserIndexVM> GetUsersAsync()
        {   
            var users = new List<UserVM>();
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if(!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    users.Add(new UserVM
                    {
                      Id = user.Id,
                      Email=user.Email,
                      Name=user.Name,
                      Surname=user.Surname,
                      PhoneNumber=user.PhoneNumber,
                      Roles= _userManager.GetRolesAsync(user).Result.ToList()
                    });
                }
            }
            var model = new UserIndexVM
            {
                 Users= users
            };
            return model;
        }

        public async Task<UserCreateVM> CreateAsync()
        {
            var model = new UserCreateVM
            {
               Roles= await _roleManager.Roles.Where(r=>r.Name!="Admin").Select(r=>new SelectListItem
               {
                   Text=r.Name,
                   Value=r.Id
               }).ToListAsync()
            };

            return model;
        }

        public async Task<bool> CreateAsync(UserCreateVM model)
        {
            if(!_modelState.IsValid) return false;
            var user = new User
            {
                Email=model.EmailAddress,
                UserName=model.EmailAddress,
                Name=model.Name,
                Surname=model.Surname,
                PhoneNumber=model.PhoneNumber
            };
            var userCreateResult =await _userManager.CreateAsync(user,model.Password);
            if (!userCreateResult.Succeeded)
            {
                foreach (var error in userCreateResult.Errors)
                {
                    _modelState.AddModelError(string.Empty, error.Description);
                }
                    return false;
            }
            foreach (var roleId in model.RolesIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null)
                {
                    _modelState.AddModelError("RolesIds", "Bu id-li rol tapılmadı ");
                    return false;
                }
                var addRoleToResult = await _userManager.AddToRoleAsync(user, role.Name);
                if (!addRoleToResult.Succeeded)
                {
                    _modelState.AddModelError("RolesIds", "istifadəçiyə rolu əlavə etmək mümkün olmadı");
                    return false;
                }
            }
            return true;
        }

        public async Task<UserUpdateVM> UpdateAsync(string id)
        {
         var user =await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            List<string> rolesIds = new List<string>();
            var userRoles =await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var role =await _roleManager.FindByNameAsync(userRole);
                rolesIds.Add(role.Id);
            }
            var  model = new UserUpdateVM
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                EmailAddress = user.Email,
                Roles = _roleManager.Roles.Where(x => x.Name != UserRoles.Admin.ToString()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToList(),
                RolesIds = rolesIds
            };

            return model;
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateVM model)
        {
            if (!_modelState.IsValid) return false;
            
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _modelState.AddModelError(string.Empty, "istifadəçi tapılmadı");
                return false;
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.PhoneNumber = model.PhoneNumber;
            if (model.NewPassword is not null)
            {
                var passwordValidationResult = _userManager.PasswordValidators
               .Select(v => v.ValidateAsync(_userManager, user, model.NewConfirmPassword)).ToList();

                foreach (var validationTask in passwordValidationResult)
                {
                    var result = validationTask.Result;
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            _modelState.AddModelError(string.Empty, error.Description);
                        }
                        return false;
                    }
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            }
            if (model.EmailAddress is not null) user.Email = model.EmailAddress;

            List<string> rolesIds = new List<string>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var role =await _roleManager.FindByNameAsync(userRole);
                rolesIds.Add(role.Id);
            }
            var mustBeAddedRoleIds = model.RolesIds.Except(rolesIds).ToList();
            var mustBeDeletedRoleIds = rolesIds.Except(model.RolesIds).ToList();
            foreach (var roleId in mustBeAddedRoleIds)
            {
                var role = _roleManager.FindByIdAsync(roleId).Result;
                if (role is null)
                {
                    _modelState.AddModelError("RolesIds", "rol tapılmadı");
                    return false;
                }
                var result =await  _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) _modelState.AddModelError(string.Empty, error.Description);
                    return false;
                }
            }

            foreach (var roleId in mustBeDeletedRoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null)
                {
                    _modelState.AddModelError("RolesIds", " rol tapılmadı");
                    return false;
                }
                var result =await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) _modelState.AddModelError(string.Empty, error.Description);

                    return false;
                }
            }
           
            var updateResult =await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors) _modelState.AddModelError(string.Empty, error.Description);

                return false;
            }
            return true;
        }

        public async Task<UserDetailVM> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;
            var model = new UserDetailVM
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            var roleResult = await _userManager.GetRolesAsync(user);
            model.Roles = roleResult.ToList();
            return model;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return false;
            var deleteResult =await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)  return false;
            return true;
        }
    }
}
