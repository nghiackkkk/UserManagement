namespace UserManagement.Models.Viewsmodel
{
    public class CardViewModel
    {
        public string? CoverImage { get; set; }
        public string Fullname { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? IdCard { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ExpireDate { get; set; }
    }
}
