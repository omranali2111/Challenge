using ChallengeBackEnd.Model;

namespace ChallengeBackEnd.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudents();
        Task<StudentDto> GetStudentById(int id);
        Task AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);

        Task<int> GetStudentCountPerClass(int classId);
        Task<int> GetStudentCountPerCountry(int countryId);
        Task<double> GetAverageAgeOfStudents();
    }
}
