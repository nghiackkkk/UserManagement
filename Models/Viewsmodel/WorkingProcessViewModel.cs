namespace UserManagement.Models.Viewsmodel
{
    public class WorkingProcessViewModel
    {
        public DateOnly StartTime { get; set; }

        public DateOnly EndTime { get; set; }

        public string WorkingAgency { get; set; } = null!;

        public string Position { get; set; } = null!;
    }
}
