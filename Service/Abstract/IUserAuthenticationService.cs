
using Agenda.Models.Domain;
using Agenda.Models.Dto;


namespace Agenda.Service.Abstract
{
    public interface IUserAuthenticationService
    {
   
        Task<IList<string>> GetRolesAsync(string userId);
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePassword model, string username);


        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<Status> CreateUserAsync(RegistrationModel model);
        Task<Status> UpdateUserAsync(string userId, RegistrationModel model);
        Task<Status> DeleteUserAsync(string userId);
    }
}