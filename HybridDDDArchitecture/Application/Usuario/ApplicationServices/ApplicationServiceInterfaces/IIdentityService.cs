

using Application.Usuario.DataTransferObjets;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IIdentityService
    {

        Task<string> RegisterAsync(string nombre,string apellido, DateOnly fechaNac, string email,string telefono, string password);
        Task<LoginResponseDto?> LoginAsync(string email, string password);
        Task<UserDto?> FindByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);

        Task<UserDto?> FindById(string idUser);


    }
}
