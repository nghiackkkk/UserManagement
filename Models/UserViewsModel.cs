namespace UserManagement.Models
{
    public class UserViewsModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Gender { get; set; }

        public int? Age { get; set; }

        public string? Number { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Priority { get; set; }

        public string? Status { get; set; }

        public IFormFile Photo { get; set; }
    }

}
