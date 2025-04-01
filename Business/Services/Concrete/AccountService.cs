


using Business.Models.Account;
using Business.Models.Admin.Account;
using Business.Services.Abstract;

using Core.Entities;
using Core.Utilities.EmailHandler.Abstract;
using Core.Utilities.EmailHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Business.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly ModelStateDictionary _modelState;
        public AccountService(UserManager<User>userManager,IActionContextAccessor contextAccessor,
            SignInManager<User>signInManager,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _modelState =contextAccessor.ActionContext.ModelState;
        }
        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterVM model)
        {
            if(!_modelState.IsValid) return false;
            var user = new User
            {
                Email=model.EmailAddress,
                UserName=model.EmailAddress,
                PhoneNumber=model.PhoneNumber,
                 Name=model.Name,
                 Surname=model.Surname
            };
            var result=await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) _modelState.AddModelError(string.Empty,error.Description);
                return false;
            }


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var httpContext = _httpContextAccessor.HttpContext;
            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));
            var url = urlHelper.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, httpContext.Request.Scheme);
            _emailService.SendMessage(new Message(new List<string> { user.Email }, "Email Confirmation", url));
            return true;
        }

        public async Task<(bool IsSucceeded, string? returnUrl)> LoginAsync(LoginVM model)
        {
            if (!_modelState.IsValid) return (false, null);

            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user is null)
            {
                _modelState.AddModelError(string.Empty, "Email və ya  şifrə yanlışdır");
                return (false, null);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                _modelState.AddModelError(string.Empty, "Email və ya  şifrə yanlışdır");
                return (false, null);
            }

            return (true, model.ReturnUrl);
        }


        public async Task<bool> ForgetPasswordAsync(ForgetPasswordVM model)
        {
            if (!_modelState.IsValid) return false;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                _modelState.AddModelError("Email", "İstifadəçi  tapılmadı");
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var httpContext = _httpContextAccessor.HttpContext;
            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));
            var url = urlHelper.Action(nameof(ResetPassword), "Account", new { token, user.Email }, httpContext.Request.Scheme);
            _emailService.SendMessage(new Message(new List<string> { user.Email }, "Forget Password?", url));

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordVM model)
        {
            if (!_modelState.IsValid) return false;

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is null)
            {
                _modelState.AddModelError("Password", "Şifrəni yeniləmək mümkün olmadı");
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    _modelState.AddModelError(string.Empty, error.Description);

                return false;
            }

            return true;
        }

        public async Task LogoutAsync()
        {
           await _signInManager.SignOutAsync();
        }
    }
}
