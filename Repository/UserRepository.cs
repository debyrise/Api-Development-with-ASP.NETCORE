using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretkey;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);



            if(user == null || isvalid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            //if user is found generate jwt token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                //Role = roles.FirstOrDefault(),
            };
            return loginResponseDto;
        }

        public async Task<UserDTO> Register(RegisterRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
                NormalizedEmail = registerRequestDto.UserName.ToUpper(),
                Name = registerRequestDto.Name,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult()){
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        await _roleManager.CreateAsync(new IdentityRole("Customer"));
                    };
                    await _userManager.AddToRoleAsync(user, "Admin");
                    var userToReturn = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == registerRequestDto.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);
                   
                }
            }
            catch(Exception e)
            {

            }
            return new UserDTO();
        }
    }
}
