using ChallengeBackEnd.Model;
using ChallengeBackEnd.Service;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChallengeBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }
       // [Authorize(AuthenticationSchemes = "oidc")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudents();
                if (students == null || !students.Any())
                {
                    _logger.LogWarning("No students found.");
                    return NotFound("No students found.");
                }
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all students.");
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                {
                    _logger.LogWarning("Student with ID {StudentId} not found.", id);
                    return NotFound($"Student with ID {id} not found.");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student with ID {StudentId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

       
        [HttpPost]
        public async Task<ActionResult> AddStudent(Student student)
        {
            try
            {
                await _studentService.AddStudent(student);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new student.");
                return StatusCode(500, "Internal server error.");
            }
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            try
            {
                await _studentService.UpdateStudent(student);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the student with ID {StudentId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _studentService.DeleteStudent(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the student with ID {StudentId}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpGet("class/{classId}/count")]
        public async Task<ActionResult<int>> GetStudentCountPerClass(int classId)
        {
            try
            {
                var count = await _studentService.GetStudentCountPerClass(classId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student count for class with ID {ClassId}.", classId);
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpGet("country/{countryId}/count")]
        public async Task<ActionResult<int>> GetStudentCountPerCountry(int countryId)
        {
            try
            {
                var count = await _studentService.GetStudentCountPerCountry(countryId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student count for country with ID {CountryId}.", countryId);
                return StatusCode(500, "Internal server error.");
            }
        }

        
        [HttpGet("average-age")]
        public async Task<ActionResult<double>> GetAverageAgeOfStudents()
        {
            try
            {
                var averageAge = await _studentService.GetAverageAgeOfStudents();
                return Ok(averageAge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the average age of students.");
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
