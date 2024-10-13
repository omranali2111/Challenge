using ChallengeBackEnd.Model;

namespace ChallengeBackEnd.Service
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetAllClasses();
        Task<ClassDto> GetClassById(int id);
        Task AddClass(Class schoolClass);
        Task UpdateClass(Class schoolClass);
        Task DeleteClass(int id);
    }
}
