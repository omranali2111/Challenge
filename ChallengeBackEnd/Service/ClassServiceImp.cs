using ChallengeBackEnd.Data;
using ChallengeBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBackEnd.Service
{
    public class ClassServiceImp : IClassService
    {
        private readonly MyDatabaseContext _context;

        public ClassServiceImp(MyDatabaseContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ClassDto>> GetAllClasses()
        {
            return await _context.Classes
                .Include(c => c.Students)               
                .ThenInclude(s => s.Class)             
                .Include(c => c.Students)               
                .ThenInclude(s => s.Country)            
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        DateOfBirth = s.DateOfBirth,
                        ClassId = s.Class.Id,            
                        ClassName = s.Class.ClassName,   
                        CountryId = s.Country.Id,        
                        CountryName = s.Country.Name    
                    }).ToList()
                })
                .ToListAsync();
        }


        public async Task<ClassDto> GetClassById(int id)
        {
            return await _context.Classes
                .Include(c => c.Students)               
                .ThenInclude(s => s.Country)           
                .Where(c => c.Id == id)                 
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        DateOfBirth = s.DateOfBirth,
                        ClassId = s.Class.Id,            
                        ClassName = s.Class.ClassName,   
                        CountryId = s.Country.Id,        
                        CountryName = s.Country.Name     
                    }).ToList()
                })
                .FirstOrDefaultAsync();  
        }



        public async Task AddClass(Class schoolClass)
        {
            await _context.Classes.AddAsync(schoolClass);
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateClass(Class schoolClass)
        {
            _context.Classes.Update(schoolClass);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteClass(int id)
        {
            var schoolClass = await _context.Classes.FindAsync(id);
            if (schoolClass != null)
            {
                _context.Classes.Remove(schoolClass);
                await _context.SaveChangesAsync();
            }
        }

    }
}
