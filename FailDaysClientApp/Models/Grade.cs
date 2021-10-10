namespace FailDaysClientApp.Models
{
    public class Grade
    {
        public string Category { get; set; }
        public int Id { get; set; }
        public decimal Number { get; set; }
        public decimal WeightPercent { get; set; }
        public int StudentId { get; set; }
    }
}