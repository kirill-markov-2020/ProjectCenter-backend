using ProjectCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.ValueObjects
{
    public static class StudentCourseCalculator
    {
        public static int GetCourseOnDate(Student student, DateTime targetDate)
        {
            if (student == null)
                return 1;

            var yearsSinceEnrollment = targetDate.Year - student.DateEnrolled.Year;
            var course = yearsSinceEnrollment + 1;

            if (targetDate < student.DateEnrolled.AddYears(yearsSinceEnrollment))
                course--;

            return Math.Min(4, Math.Max(1, course));
        }

        public static int? GetCurrentCourse(Student student, DateTime currentDate)
        {
            if (student == null)
                return null;

            if (student.DateGraduated.HasValue)
            {
                if (student.DateGraduated.Value > currentDate)
                {
                    return GetCourseOnDate(student, currentDate);
                }
                else
                {
                    return GetCourseOnDate(student, student.DateGraduated.Value);
                }
            }

            return GetCourseOnDate(student, currentDate);
        }


        public static string GetFullGroupName(Student student, DateTime currentDate)
        {
            if (student?.Group == null)
                return "Группа не указана";

            var course = GetCurrentCourse(student, currentDate);
            var isGraduated = student.DateGraduated.HasValue && student.DateGraduated.Value <= currentDate;

            if (isGraduated)
            {
                return $"{course} курс, {student.Group.Name}";
            }

            return $"{course} курс, {student.Group.Name}";
        }
        public static bool IsActive(Student student, DateTime currentDate)
        {
            if (student == null)
                return false;
            
            return !student.DateGraduated.HasValue || student.DateGraduated.Value > currentDate;
        }
    }
}
