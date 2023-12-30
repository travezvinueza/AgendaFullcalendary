
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

        //esto es para que me traiga una lista de los usuario
        List<RegistrationModel> GetAll();

        //todo esto es necesario para actualizar el usuario metodo crud
        Task<Status> UpdateAsync(string userId, RegistrationModel model);
        Task<RegistrationModel> GetById(string id);
        Task Add(RegistrationModel model);
        Task Delete(string id);
    }
}
