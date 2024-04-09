namespace UserManagement.Models.Viewsmodel
{
    public class StudyProcessViewModel
    {
       

        public DateOnly StartTime { get; set; }

        public DateOnly EndTime { get; set; }

        public string SchoolUniversity { get; set; } = null!;

        public string ModeOfStudy { get; set; } = null!;
    }
}
