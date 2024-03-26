using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class Address
{
    public int Id { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Commune { get; set; }

    public string? Address1 { get; set; }

    public virtual ICollection<User> UserIdPernamentResidenceNavigations { get; set; } = new List<User>();

    public virtual ICollection<User> UserIdRegularAddressNavigations { get; set; } = new List<User>();
}
