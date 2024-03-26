using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class Studyprocess
{
    public int Id { get; set; }

    public DateOnly StartTime { get; set; }

    public DateOnly EndTime { get; set; }

    public string SchoolUniversity { get; set; } = null!;

    public string ModeOfStudy { get; set; } = null!;

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
