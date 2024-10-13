using ChallengeBackEnd.Model;
using ChallengeBackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly ILogger<ClassController> _logger;

        public ClassController(IClassService classService, ILogger<ClassController> logger)
        {
            _classService = classService;
            _logger = logger; 
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetAllClasses()
        {
            try
            {
                var classes = await _classService.GetAllClasses();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all classes.");
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClassById(int id)
        {
            try
            {
                var schoolClass = await _classService.GetClassById(id);
                if (schoolClass == null)
                {
                    _logger.LogWarning("Class with ID {ClassId} not found.", id);
                    return NotFound($"Class with ID {id} not found.");
                }

                return Ok(schoolClass);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the class with ID {ClassId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddClass(Class schoolClass)
        {
            try
            {
                await _classService.AddClass(schoolClass);
                _logger.LogInformation("Class {ClassName} added successfully.", schoolClass.ClassName);
                return CreatedAtAction(nameof(GetClassById), new { id = schoolClass.Id }, schoolClass);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error occurred while adding a new class.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "An error occurred while adding a new class.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, Class schoolClass)
        {
            if (id != schoolClass.Id)
            {
                return BadRequest("Class ID mismatch.");
            }

            try
            {
                await _classService.UpdateClass(schoolClass);
                _logger.LogInformation("Class {ClassName} with ID {ClassId} updated successfully.", schoolClass.ClassName, schoolClass.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the class with ID {ClassId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClass(id);
                _logger.LogInformation("Class with ID {ClassId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the class with ID {ClassId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
