using ChallengeBackEnd.Data;
using ChallengeBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBackEnd.Service
{
    public class CountryServiceImp:ICountryService
    {
        private readonly MyDatabaseContext _context;

        public CountryServiceImp(MyDatabaseContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CountryDto>> GetAllCountries()
        {
            return await _context.Countries
             
                .Include(c => c.Students)
                .ThenInclude(s => s.Class)
                .Include(c => c.Students)
                .ThenInclude(s => s.Country)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
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



        public async Task<CountryDto> GetCountryById(int id)
        {
                    return await _context.Countries
               .Include(c => c.Students)               
               .ThenInclude(s => s.Class)            
               .Include(c => c.Students)
               .ThenInclude(s => s.Country)            
               .Where(c => c.Id == id)                 
               .Select(c => new CountryDto
               {
                   Id = c.Id,
                   Name = c.Name,
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

        
        public async Task AddCountry(Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
        }

      
        public async Task UpdateCountry(Country country)
        {
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
            }
        }
    }
}
