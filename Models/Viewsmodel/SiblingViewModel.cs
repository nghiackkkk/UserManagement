namespace UserManagement.Models.Viewsmodel
{
    public class SiblingViewModel
    {
        public int? Id { get; set; }

        public string Realtionship { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public int YearOfBirth { get; set; }

        public string CurrentResident { get; set; } = null!;

        public string CurrentOcupation { get; set; } = null!;

        public string WorkingAgency { get; set; } = null!;
    }
}
