namespace ChallengeBackEnd.Model
{
    public class Class
    {
        private int _id;
        private string _className;

      
        public int Id
        {
            get { return _id; }
            private set { _id = value; }  
        }

        public string ClassName
        {
            get { return _className; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Class name cannot be null or empty.");
                _className = value;
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
