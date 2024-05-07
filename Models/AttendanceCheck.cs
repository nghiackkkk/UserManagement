using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class AttendanceCheck
{
    public int Id { get; set; }

    public int? IdStaff { get; set; }

    public DateOnly? Day { get; set; }

    public TimeOnly? TimeIn { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public string? Reason { get; set; }

    public string? Accepted { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }
}
