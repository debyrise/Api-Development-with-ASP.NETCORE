using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApiDemo.Model;
using WebApiDemo.Model.Dto;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Controllers
{
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiController]
    [ApiVersionNeutral]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private ApiResponse _response;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new();
        }


        [HttpPost("Login") ]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _userRepository.Login(model);
            if(loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.statusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.result = loginResponse;
            return Ok(_response);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Username already exists");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(model);
            if(user == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Error while registering");
                return BadRequest(_response);

            }
            _response.statusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
