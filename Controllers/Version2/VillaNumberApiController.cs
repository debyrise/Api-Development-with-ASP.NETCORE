
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Model.Dto;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/villaNumberApi")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberApiController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IVillaNumberRepository _dbContextNumber;
        private readonly IVillaRepository _dbContext;
        private readonly IMapper _mapper;
        public VillaNumberApiController(IVillaNumberRepository dbContextNumber, IMapper mapper, IVillaRepository dbContext)
        {
            _dbContextNumber = dbContextNumber;
            _mapper = mapper;
            this._response = new();
            _dbContext = dbContext;
        }
       

        //[MapToApiVersion("2.0")]
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Bhrgen", "DotNetMastery" };
        }








    }


}
