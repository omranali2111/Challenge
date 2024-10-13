using ChallengeBackEnd.Model;

namespace ChallengeBackEnd.Service
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDto>> GetAllCountries();
        Task<CountryDto> GetCountryById(int id);
        Task AddCountry(Country country);
        Task UpdateCountry(Country country);
        Task DeleteCountry(int id);
    }
}
