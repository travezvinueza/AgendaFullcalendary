using Agenda.Models;
using Agenda.Models.Domain;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace Agenda.Service.Impl
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "El usuario ya existe";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Error al crear el usuario";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Te has registrado exitosamente";
            return status;
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new List<string>();
            }

            return await userManager.GetRolesAsync(user);
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Nombre Invalido";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Contraseña Invalida";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Inicio de sesión exitoso";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "El usuario está bloqueado";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error al iniciar sesión";
            }

            return status;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

        public async Task<Status> ChangePasswordAsync(ChangePassword model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "El usuario no existe";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "La contraseña se ha actualizado correctamente";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Se produjo algún error";
                status.StatusCode = 0;
            }
            return status;

        }



        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Status> CreateUserAsync(RegistrationModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Status> UpdateUserAsync(string userId, RegistrationModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Status> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

    }
}