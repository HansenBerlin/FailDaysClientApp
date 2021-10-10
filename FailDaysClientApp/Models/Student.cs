using System.ComponentModel;

namespace FailDaysClientApp.Models
{
    public class Student
    {
        public string CourseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AverageGrade { get; set; }
        
        private int matNr;
        
        public int MatNr
        {
            get => matNr;
            set
            {
                matNr = value;
                OnPropertyChanged("MatNr");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}