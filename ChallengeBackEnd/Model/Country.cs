namespace ChallengeBackEnd.Model
{
    public class Country
    {
        private int _id;
        private string _name;

     
        public int Id
        {
            get { return _id; }
            private set { _id = value; } 
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Country name cannot be null or empty.");
                _name = value;
            }
        }

        
        private ICollection<Student> _students;
        public virtual ICollection<Student> Students
        {
            get { return _students ?? (_students = new List<Student>()); }
            set { _students = value; }
        }
    }

}
