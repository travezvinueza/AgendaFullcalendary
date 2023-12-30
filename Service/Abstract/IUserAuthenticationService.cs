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

        //traer una lista de usuarios
        List<RegistrationModel> GetAll();

    }
}
