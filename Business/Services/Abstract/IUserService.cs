
using Business.Models.Admin.User;

namespace Business.Services.Abstract
{
    public interface IUserService
    {
       Task<UserIndexVM> GetUsersAsync();
        Task<UserCreateVM> CreateAsync();
        Task<bool> CreateAsync(UserCreateVM model);
        Task<UserUpdateVM> UpdateAsync(string id );
        Task<bool> UpdateAsync(string id , UserUpdateVM model);
        Task<UserDetailVM> GetUserAsync(string id);

        Task<bool> DeleteAsync(string id);

    }
}
