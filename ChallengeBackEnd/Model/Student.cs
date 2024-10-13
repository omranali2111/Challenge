using System.Diagnostics.Metrics;

namespace ChallengeBackEnd.Model
{
    public class Student
    {
        private int _id;
        private int _classId;
        private int _countryId;
        private string _name;
        private DateTime _dateOfBirth;

        // Encapsulated properties
        public int Id
        {
            get { return _id; }
            private set { _id = value; }  
        }

        public int ClassId
        {
            get { return _classId; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Class ID must be a positive number.");
                _classId = value;
            }
        }

        public int CountryId
        {
            get { return _countryId; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Country ID must be a positive number.");
                _countryId = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Student name cannot be null or empty.");
                _name = value;
            }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Date of birth cannot be in the future.");
                _dateOfBirth = value;
            }
        }

        public Class? Class { get; set; }
        public Country? Country { get; set; }
    }

}
