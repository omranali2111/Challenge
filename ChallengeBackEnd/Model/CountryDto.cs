namespace ChallengeBackEnd.Model
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentDto> Students { get; set; }
    }
}
