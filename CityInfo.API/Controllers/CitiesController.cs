using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    //one style is matching prefix of CitiesController (Cities) and writing [Route("api/[controller]")
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    {
         private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        //inject interface
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery] string? name, 
            string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {

            //set defaults for params.
            if(pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            //now you have to change the repo.
            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery
                , pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
           

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
            //return Ok(_citiesDataStore.Cities);
        }
        /// <summary>
        /// Get a city by id 
        /// </summary>
        /// <param name="id">The id of city to get</param>
        /// <param name="includePointsOfInterest">Whether or not include points of interest</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns requested city.</response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = false) {
            {
                //Altough ActionResult is correct, we will use IActionResult
                var city = await _cityInfoRepository.GetCityAsync(cityId, includePointsOfInterest);
                if(city == null)
                {
                    return NotFound();   
                }
                if (includePointsOfInterest)
                {
                    return Ok(_mapper.Map<CityDto>(city));
                }

                return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
            }
        }
    }
}
