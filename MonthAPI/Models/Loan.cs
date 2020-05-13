using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonthAPI.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal LoanMoney { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Dayss { get; set; }
        public bool IsYearRote { get; set; }
        public decimal Rote { get; set; }
        public decimal RealMoney { get; set; }
        public string Stata { get; set; }
        public int UId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}