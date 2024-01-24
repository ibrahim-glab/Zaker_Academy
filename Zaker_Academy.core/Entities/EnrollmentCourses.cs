namespace Zaker_Academy.infrastructure.Entities
{
    public class EnrollmentCourses
    {
        public int id { get; set; }
        public applicationUser applicationUser { get; set; }
        public Course Course { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public bool CompleteIt { get; set; }
    }
}