using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class Absence
{
    public int Id { get; set; }

    public int? IdStaff { get; set; }

    public DateOnly? DayFrom { get; set; }

    public DateOnly? DayTo { get; set; }

    public string? Reason { get; set; }

    public string? Accepted { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }
}
