using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonthAPI.Models
{
    public class MoneyBag
    {
        public int Id		 { get; set; }
        public int UId		 { get; set; }
        public decimal Moneyy { get; set; }
        public UserInfo UserInfo { get; set; }

    }
}