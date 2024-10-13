using ChallengeBackEnd.Model;
using ChallengeBackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            try
            {
                var countries = await _countryService.GetAllCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all countries.");
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountryById(int id)
        {
            try
            {
                var country = await _countryService.GetCountryById(id);
                if (country == null)
                {
                    _logger.LogWarning("Country with ID {CountryId} not found.", id);
                    return NotFound($"Country with ID {id} not found.");
                }

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the country with ID {CountryId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

      
        [HttpPost]
        public async Task<ActionResult> AddCountry(Country country)
        {
            try
            {
                await _countryService.AddCountry(country);
                _logger.LogInformation("Country {CountryName} added successfully.", country.Name);
                return CreatedAtAction(nameof(GetCountryById), new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new country.");
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest("Country ID mismatch.");
            }

            try
            {
                await _countryService.UpdateCountry(country);
                _logger.LogInformation("Country {CountryName} with ID {CountryId} updated successfully.", country.Name, country.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the country with ID {CountryId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                await _countryService.DeleteCountry(id);
                _logger.LogInformation("Country with ID {CountryId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the country with ID {CountryId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
