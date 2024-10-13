namespace ChallengeBackEnd.Model
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
