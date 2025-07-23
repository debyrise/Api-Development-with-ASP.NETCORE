using WebApiDemo.Model;
using WebApiDemo.Model.Dto;

namespace WebApiDemo.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegisterRequestDto registerRequestDto);
    }
}
