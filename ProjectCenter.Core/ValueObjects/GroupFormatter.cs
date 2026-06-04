using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.ValueObjects
{
    public static class GroupFormatter
    {
        public static string GetFullName(string specialtyCode, string baseName, DateTime enrolledDate, DateTime currentDate)
        {
            var years = currentDate.Year - enrolledDate.Year;
            var course = years + 1;
            if (currentDate < enrolledDate.AddYears(years)) course--;
            course = Math.Clamp(course, 1, 4);

            return $"{specialtyCode}-{course}{baseName}";
        }
    }
}
