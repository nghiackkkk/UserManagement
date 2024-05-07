using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? CoverImage { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EthnicGroup { get; set; }

    public string? Regilion { get; set; }

    public string? IdCard { get; set; }

    public string? CulturalStandard { get; set; }

    public int? IdPernamentResidence { get; set; }

    public int? IdRegularAddress { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public string? InTrash { get; set; }

    public virtual ICollection<Family> Families { get; set; } = new List<Family>();

    public virtual Address? IdPernamentResidenceNavigation { get; set; }

    public virtual Address? IdRegularAddressNavigation { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Studyprocess> Studyprocesses { get; set; } = new List<Studyprocess>();

    public virtual ICollection<Workingprocess> Workingprocesses { get; set; } = new List<Workingprocess>();
}
