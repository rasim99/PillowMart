using Business.Models.Admin.Account;
namespace Business.Services.Abstract
{
    public interface IAdminAccountService
    {
        Task<bool> LoginAsync(AccountLoginVM model);

    }
}
