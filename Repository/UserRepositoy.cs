using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Model.Dto;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Repository
{
    public class UserRepositoy : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private string secretkey;
        public UserRepositoy(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _dbContext.localUsers.FirstOrDefault(x => x.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.localUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower()
             && x.Password == loginRequestDto.Password );

            if(user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            //if user is found generate jwt token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return loginResponseDto;
        }

        public async Task<LocalUser> Register(RegisterRequestDto registerRequestDto)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registerRequestDto.UserName,
                Password = registerRequestDto.Password,
                Name = registerRequestDto.Name,
                Role = registerRequestDto.Role
            };
            _dbContext.localUsers.Add(user);
           await  _dbContext.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
