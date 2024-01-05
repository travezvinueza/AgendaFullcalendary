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

        //nuevos metodos
        List<RegistrationModel> GetAll();
        Task<UpdateProfileModel> GetProfileAsync(string userId);
        Task<Status> UpdateProfileAsync(string userId, UpdateProfileModel model);

        //metodos para el admin
        Status DeleteUser(string username);
        Task<EditUserModel> GetEditUserModelAsync(string username);
        Task<Status> UpdateUserAsync(EditUserModel model);
    }
}
