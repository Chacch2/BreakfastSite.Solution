using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.site.Models.ViewModels
{
    public class PointsDetailVm
    {
        public string OrderID { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public int EarnedPoints { get; set; }
        public int UsedPoints { get; set; }
        public int RemainingPoints { get; set; }
    }

    public class MemberPointsVm
    {
        public int CurrentPoints { get; set; }
        public List<PointsDetailVm> RankList { get; set; }
    }
}