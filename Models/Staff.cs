using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class Staff
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public string? Department { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<AttendanceCheck> AttendanceChecks { get; set; } = new List<AttendanceCheck>();

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
}
