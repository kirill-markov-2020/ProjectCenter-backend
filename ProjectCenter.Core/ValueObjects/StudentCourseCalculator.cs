using ProjectCenter.Core.Entities;

namespace ProjectCenter.Core.ValueObjects
{
    public static class StudentCourseCalculator
    {
        public static int GetCurrentCourse(Student student, DateTime currentDate)
        {
            if (student?.DateEnrolled == null) return 1;

            var years = currentDate.Year - student.DateEnrolled.Year;
            var course = years + 1;
            if (currentDate < student.DateEnrolled.AddYears(years)) course--;
            return Math.Clamp(course, 1, 4);
        }

        public static bool IsActive(Student student, DateTime currentDate)
        {
            if (student == null) return false;
            return !student.DateGraduated.HasValue || student.DateGraduated.Value > currentDate;
        }
    }
}
