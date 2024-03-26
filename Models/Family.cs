using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class Family
{
    public int Id { get; set; }

    public string Realtionship { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int YearOfBirth { get; set; }

    public string CurrentResident { get; set; } = null!;

    public string CurrentOcupation { get; set; } = null!;

    public string WorkingAgency { get; set; } = null!;

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
