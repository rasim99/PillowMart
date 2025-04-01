

using Business.Models.Account;

namespace Business.Services.Abstract
{
    public interface IAccountService
    {
        Task<bool> ConfirmEmail(string email, string token);
    
        Task<bool> RegisterAsync(RegisterVM model);

        Task<(bool IsSucceeded, string? returnUrl)> LoginAsync(LoginVM model);

        Task<bool> ForgetPasswordAsync(ForgetPasswordVM model);
        Task<bool> ResetPassword(ResetPasswordVM model);
        Task LogoutAsync();
    }
}
