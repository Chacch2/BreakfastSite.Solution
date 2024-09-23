using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.site.Models.ViewModels
{
    public class AddOnVm
    {
        public string Name { get; set; }   // 加选项名称
        public decimal Price { get; set; } // 加选项价格
        public string AddOnOptionName { get; internal set; }
        public int AddOnOptionPrice { get; internal set; }
    }
}