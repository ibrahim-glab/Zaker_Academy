namespace Zaker_Academy.infrastructure.Entities
{
    public class EnrollmentCourses
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string StudentId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int CourseId { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public bool CompleteIt { get; set; }
    }
}