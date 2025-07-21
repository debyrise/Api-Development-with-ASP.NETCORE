using AutoMapper;
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
    [Route("api/[controller]")]
    //[Route("api/villaNumberApi")]
    [ApiController]
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

        [HttpGet]// to return all the record
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(User), 200)]

        public async Task<ActionResult<ApiResponse>> GetVillaNumber()
        {
            try
            {
                IEnumerable<VillaNumber> villaListNumber = await _dbContextNumber.GetAllAsync(includeproperties:"Villa");
                _response.result = _mapper.Map<List<VillaNumberDto>>(villaListNumber);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;

        }


        [HttpGet("{id:int}", Name = "GetVillaNumber")] //to return one record
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ApiResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }
                var villaNumber = await _dbContextNumber.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto CreateDto)
        {
            try
            {

                if (await _dbContextNumber.GetAsync(x => x.VillaNo == CreateDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa Number already exist!");

                    return BadRequest(ModelState);
                }
                if(await _dbContext.GetAsync(x => x.Id == CreateDto.VillaID ) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa ID Is Invalid!");

                    return BadRequest(ModelState);

                }



                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }


                VillaNumber villaNumber = _mapper.Map<VillaNumber>(CreateDto);

                await _dbContextNumber.CreateAsync(villaNumber);
                _response.result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _dbContextNumber.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    return NotFound(villaNumber);

                }
                await _dbContextNumber.RemoveAsync(villaNumber);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;

        }


        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto UpdateDto)
        {
            try
            {
                if (UpdateDto == null || id != UpdateDto.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbContext.GetAsync(x => x.Id == UpdateDto.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa ID Is Invalid!");

                    return BadRequest(ModelState);

                }

                VillaNumber model = _mapper.Map<VillaNumber>(UpdateDto);

                await _dbContextNumber.UpdateAsync(model);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;
        }





    }


}
