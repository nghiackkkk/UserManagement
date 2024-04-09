namespace UserManagement.Models.Viewsmodel
{
    public class UserViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EthnicGroup { get; set; }
        public string? Religion { get; set; }
        public string? IdCard { get; set; }
        public string? CulturalStandard { get; set; }
        public string? PermanentResidenceCity { get; set; }
        public string? PermanentResidenceDistrict { get; set; }
        public string? PermanentResidenceCommune { get; set; }
        public string? PermanentResidenceAddress { get; set; }
        public string? RegularAddressCity { get; set; }
        public string? RegularAddressDistrict { get; set; }
        public string? RegularAddressCommune { get; set; }
        public string? RegularAddressAddress { get; set; }
        public IFormFile? CoverImage { get; set; }
        public string? Cover { get; set; }
        public int? Id { get; set; }

        // Additional properties for dynamic lists
        public List<SiblingViewModel> Siblings { get; set; } = new List<SiblingViewModel>();
        public List<StudyProcessViewModel> StudyProcesses { get; set; } = new List<StudyProcessViewModel>();
        public List<WorkingProcessViewModel> WorkingProcesses { get; set; } = new List<WorkingProcessViewModel>();

    }
}
