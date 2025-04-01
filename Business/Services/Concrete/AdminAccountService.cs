using Business.Models.Admin.Account;
using Business.Services.Abstract;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly ModelStateDictionary _modelState;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AdminAccountService(IActionContextAccessor contextAccessor,
            UserManager<User> userManager,
             SignInManager<User> signInManager)
        {
            _modelState = contextAccessor.ActionContext.ModelState;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public async Task<bool> LoginAsync(AccountLoginVM model)
        {
            if (!_modelState.IsValid) return false;

            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user is null)
            {
                _modelState.AddModelError(string.Empty, "email vəya şifrə yanlışdır!!");
                return false;
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                _modelState.AddModelError(string.Empty, "email vəya şifrə yanlışdır!!");
                return false;
            }
            var result =await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                _modelState.AddModelError(string.Empty, "email vəya şifrə yanlışdır!!");
                return false;
            }

           return true;
        }
    }
}