using Agenda.Models.Domain;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Agenda.Service.Impl
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly DatabaseContext _dbContext;
        public UserAuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            DatabaseContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this._dbContext = dbContext;

        }
        //metodo para obtener el modelo para el boton editar
        public async Task<EditUserModel> GetEditUserModelAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null!;
            }

            var editUserModel = new EditUserModel
            {
                ProfilePicture = user.ProfilePicture,
                Username = user.UserName!,
                DNI = user.DNI,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Phone = user.Phone
            };
            return editUserModel;
        }

        // Método para actualizar el usuario
        public async Task<Status> UpdateUserAsync(EditUserModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return new Status { StatusCode = 0, Message = "Usuario no encontrado" };
            }

            user.DNI = model.DNI;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            try
            {
                await userManager.UpdateAsync(user);
                return new Status { StatusCode = 1, Message = "Usuario actualizado exitosamente" };
            }
            catch (DbUpdateException)
            {
                return new Status { StatusCode = 0, Message = "Error al actualizar el usuario" };
            }
        }

        //medtodo para el boton eliminar
        public Status DeleteUser(string username)
        {
            var status = new Status();
            var user = userManager.FindByNameAsync(username).Result;

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Usuario no encontrado";
                return status;
            }

            try
            {
                var result = userManager.DeleteAsync(user).Result;

                if (result.Succeeded)
                {
                    status.StatusCode = 1;
                    status.Message = "Usuario eliminado exitosamente";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error al eliminar el usuario: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.Message = "Error inesperado al eliminar el usuario: " + ex.Message;
            }
            return status;
        }

        //metodo para traer los datos del usuario logueado
        public async Task<UpdateProfileModel> GetProfileAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null!;
            }
            // Mapear los datos del usuario al modelo de actualización de perfil
            var profileModel = new UpdateProfileModel
            {
                ProfilePicture = user.ProfilePicture,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                DNI = user.DNI,
                Phone = user.Phone
                // Otros campos según sea necesario
            };
            return profileModel;
        }

        public async Task<Status> UpdateProfileAsync(string userId, UpdateProfileModel model)
        {
            var status = new Status();

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Usuario no encontrado";
                return status;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.DNI = model.DNI;
            user.Phone = model.Phone;

            // Guardar los cambios en la base de datos
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                status.StatusCode = 1;
                status.Message = "Perfil actualizado exitosamente";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error al actualizar el perfil";
            }
            return status;
        }

        //este metodo es para que me traiga la lista de los usuarios
        public List<RegistrationModel> GetAll()
        {
            var allUsers = userManager.Users.Include(u => u.CalendarEvents).ToList();
            var registrationModels = new List<RegistrationModel>();

            foreach (var user in allUsers)
            {
                var registrationModel = new RegistrationModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DNI = user.DNI,
                    Email = user.Email!,
                    Phone = user.Phone,
                    Username = user.UserName!,
                    ProfilePicture = user.ProfilePicture,
                    Role = userManager.GetRolesAsync(user).Result.FirstOrDefault()!,
                    Events = user.CalendarEvents.ToList()
                };

                registrationModels.Add(registrationModel);
            }

            return registrationModels;
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

            // Asociar información adicional al usuario
            user.ProfilePicture = model.ProfilePicture;

            // Guardar datos del usuario (incluyendo la imagen) en la base de datos
            await userManager.UpdateAsync(user);

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
                    new Claim(ClaimTypes.Name, user.UserName!),
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
    }
}