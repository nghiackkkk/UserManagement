using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Models.Viewsmodel
{
    public class ReportViewModel : Controller
    {
        public int Total { get; set; } = 0;
        public int TotalMale { get; set; } = 0;
        public int TotalFemale { get; set; } = 0;
        public int NewToday { get; set; } = 0;
        public Dictionary<string, int> Data7 { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DataProvince { get; set; } = new Dictionary<string, int>();
    }
}
