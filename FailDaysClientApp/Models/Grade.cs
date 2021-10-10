using System.ComponentModel;

namespace FailDaysClientApp.Models
{
    public class Grade
    {
        private double number;
        private string category;
        public int Id { get; set; }
        public int StudentId { get; set; }


        public decimal Number { get; set; } = 0m;
        //public double GradeDouble { get; set; }
        
        
        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged("Category");
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