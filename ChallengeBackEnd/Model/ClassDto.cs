namespace ChallengeBackEnd.Model
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public List<StudentDto> Students { get; set; }
    }
}
