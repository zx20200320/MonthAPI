using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MonthAPI.Models;
using Newtonsoft.Json;
using System.Data;

namespace MonthAPI.Controllers
{
    public class LoanController : ApiController
    {
        JWTHelper helper = new JWTHelper();
        /// <summary>
        /// 获取贷款列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Loan> Loans()
        {
            //string token
            //string json = helper.GetPayload(token);
            //??UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
            string str = "select * from Loan";
            var list = SqlDbHelper.GetList<Loan>(str);
            return list;
        }
        [HttpPost]
        public int UptMoney(int id )
        {


            List<Loan> dt = SqlDbHelper.GetList<Loan>($"select * from Loan");
            var lonemoney = dt.Where(p=>p.Id==id).FirstOrDefault().LoanMoney;
            var q = SqlDbHelper.GetList<Loan>($"select * from Loan");
            int uid = q.Where(p=>p.Id==id).FirstOrDefault().UId;
            var w = SqlDbHelper.GetList<MoneyBag>($"select * from  MoneyBag");
            decimal umoney = w.Where(p=>p.UId==uid).FirstOrDefault().Moneyy;
            if (umoney>lonemoney)
            {
                string str = $"update MoneyBag set Moneyy=Moneyy-{lonemoney}";
                SqlDbHelper.ExecuteNonQuery(str);
                return Convert.ToInt32(SqlDbHelper.ExecuteNonQuery("update Loan set Stata='已还'"));
            }
            else
            {
                return -1;      //钱包余额不足，提示充值
            }
        }
    }
}
