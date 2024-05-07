using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class WorkingDay
{
    public int Id { get; set; }

    public int? IdStaff { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public int? NumberChecked { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }
}
