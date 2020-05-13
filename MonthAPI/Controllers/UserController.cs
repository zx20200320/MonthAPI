using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MonthAPI.Models;

namespace MonthAPI.Controllers
{
    public class UserController : ApiController
    {
        JWTHelper helper = new JWTHelper();
        [HttpPost]
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Login(UserInfo user)
        {
            string str = $"select * from UserInfo where Name='{user.Name}' and Pass='{user.Pass}'";
            //List<UserInfo> sel = SqlDbHelper.GetList<UserInfo>(str);
            //var u = sel.Where(p => p.Name == user.Name && p.Pass == user.Pass).FirstOrDefault();
            //string token = "";
            //if (u != null)
            //{
            //    Dictionary<string, object> dic = new Dictionary<string, object>();
            //    dic.Add("Name", u.Name);
            //    dic.Add("Id", u.Id);
            //    token = helper.GetToken(dic, 1200);
            //}
            return Convert.ToInt32(SqlDbHelper.ExecuteScalar(str));
        }


    }
}
