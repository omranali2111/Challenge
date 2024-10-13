using ChallengeBackEnd.Data;
using ChallengeBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBackEnd.Service
{
    public class StudentServiceImp : IStudentService
    {
        private readonly MyDatabaseContext _context;
        private readonly ILogger<StudentServiceImp> _logger;

        public StudentServiceImp(MyDatabaseContext context, ILogger<StudentServiceImp> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudents()
        {
            return await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Country)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DateOfBirth = s.DateOfBirth,
                    ClassId = s.ClassId,
                    ClassName = s.Class.ClassName,
                    CountryId = s.CountryId,
                    CountryName = s.Country.Name
                })
                .ToListAsync();
        }


        public async Task<StudentDto> GetStudentById(int id)
        {
                        return await _context.Students
                    .Include(s => s.Class)     
                    .Include(s => s.Country)   
                    .Where(s => s.Id == id)    
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        DateOfBirth = s.DateOfBirth,
                        ClassId = s.ClassId,
                        ClassName = s.Class.ClassName,
                        CountryId = s.CountryId,
                        CountryName = s.Country.Name
                    })
                    .FirstOrDefaultAsync();
                }


        public async Task AddStudent(Student student)
        {
            try
            {
               
                var classExists = await _context.Classes.AnyAsync(c => c.Id == student.ClassId);
                if (!classExists)
                {
                    _logger.LogWarning("Class with ID {ClassId} does not exist.", student.ClassId);
                    throw new InvalidOperationException($"Class with ID {student.ClassId} does not exist.");
                }

             
                var countryExists = await _context.Countries.AnyAsync(c => c.Id == student.CountryId);
                if (!countryExists)
                {
                    _logger.LogWarning("Country with ID {CountryId} does not exist.", student.CountryId);
                    throw new InvalidOperationException($"Country with ID {student.CountryId} does not exist.");
                }

               
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Student {StudentName} with ID {StudentId} added successfully.", student.Name, student.Id);
            }
            catch (InvalidOperationException ex)
            {
              
                _logger.LogWarning(ex, ex.Message); 
                throw;  
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "An unexpected error occurred while adding the student.");
                throw new ApplicationException("An error occurred while adding the student. Please try again later.");
            }
        }



        public async Task UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<int> GetStudentCountPerClass(int classId)
        {
            return await _context.Students.CountAsync(s => s.ClassId == classId);
        }

        
        public async Task<int> GetStudentCountPerCountry(int countryId)
        {
            return await _context.Students.CountAsync(s => s.CountryId == countryId);
        }

        
        public async Task<double> GetAverageAgeOfStudents()
        {
            var today = DateTime.Today;
            return await _context.Students.AverageAsync(s =>
                (today.Year - s.DateOfBirth.Year) - (today.DayOfYear < s.DateOfBirth.DayOfYear ? 1 : 0));
        }
    }
}
