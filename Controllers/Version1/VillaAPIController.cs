using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Model.Dto;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Controllers.Version1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/villaApi")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaAPIController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IVillaRepository _dbContext;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]// to return all the record
        [ResponseCache(CacheProfileName = "Default30")] //cacheing
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(User), 200)]


        public async Task<ActionResult<ApiResponse>> GetVillas([FromQuery(Name = "filterOccupancy")]int? Occupancy, [FromQuery] string? search, int PageSize = 0, int PageNumber = 1)
        {
            try
            {
                IEnumerable<Villa> villaList;
                if(Occupancy > 0)
                {
                    villaList = await _dbContext.GetAllAsync(x => x.Occupancy == Occupancy, PageSize:PageSize, PageNumber:PageNumber);
                }
                else
                {
                    villaList = await _dbContext.GetAllAsync(PageSize: PageSize, PageNumber:PageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(x => x.Name.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = PageNumber, PageSize = PageSize };
                Response.Headers.Add("x-pagination", JsonSerializer.Serialize(pagination));
                _response.result = _mapper.Map<List<VillaDto>>(villaList);
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


        [HttpGet("{id:int}", Name = "GetVilla")] //to return one record
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //cacheing


        public async Task<ActionResult<ApiResponse>> GetAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }
                var villa = await _dbContext.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.result = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() {ex.ToString() };

            }
            return _response;
        }



        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDto CreateDto)
        {
            try
            {

                if (await _dbContext.GetAsync(x => x.Name.ToLower() == CreateDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa ID is invalid!");

                    return BadRequest(ModelState);
                }
                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }


                Villa villa = _mapper.Map<Villa>(CreateDto);

                //Villa model = new()
                //{
                //    Amenity = CreateDto.Amenity,
                //    Details = CreateDto.Details,
                //    //Id = villaDto.Id,
                //    ImageUrl = CreateDto.ImageUrl,
                //    Name = CreateDto.Name,
                //    Occupancy = CreateDto.Occupancy,
                //    Sqft = CreateDto.sqft,
                //    Rate = CreateDto.Rate,
                //};

                await _dbContext.CreateAsync(villa);
                _response.result = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbContext.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound(villa);

                }
                await _dbContext.RemoveAsync(villa);
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto UpdateDto)
        {
            try
            {
                if (UpdateDto == null || id != UpdateDto.Id)
                {
                    return BadRequest();
                }


                Villa model = _mapper.Map<Villa>(UpdateDto);

                await _dbContext.UpdateAsync(model);
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


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> PatchDto)
        {
            if (PatchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbContext.GetAsync(x => x.Id == id, tracked: false);

            VillaUpdateDto villaDTO = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null)
            {
                return BadRequest();
            }
            PatchDto.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            await _dbContext.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();




        }


    }


}
