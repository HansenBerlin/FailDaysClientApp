using System;
using System.Collections.Generic;
using System.Linq;
using FailDaysClientApp.Models;

namespace FailDaysClientApp.Controller
{
    public class GradeCalculationController
    {
        public decimal CalculateAverageGrade(List<Grade> grades)
        {
            decimal divisor = 0m;
            decimal aggregated = 0m;
            
            foreach (var grade in grades.Where(grade => grade.Number > 0))
            {
                if (grade.Category.Equals("Aktive Mitarbeit"))
                    continue;
                divisor += grade.WeightPercent;
                aggregated += grade.Number * grade.WeightPercent;
            }

            var special = grades.Where(grade => grade.Category.Equals("Aktive Mitarbeit")).ElementAt(0);
            decimal adjusted = AdjustParticipationGrade(special, aggregated, divisor);
            return Math.Round(adjusted, 2, MidpointRounding.ToEven);
        }

        private decimal AdjustParticipationGrade(Grade grade, decimal aggregated, decimal divisor)
        {
            if (aggregated/divisor >= grade.Number && grade.Number >= 1)
            {
                decimal special = grade.Number * grade.WeightPercent;
                aggregated += special;
                divisor += grade.WeightPercent;
            }
            var retutnvalue = aggregated / divisor;
            return retutnvalue;
        }
    }
}