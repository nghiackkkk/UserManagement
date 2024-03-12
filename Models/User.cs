using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public string? Number { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public string? Image { get; set; }
}
