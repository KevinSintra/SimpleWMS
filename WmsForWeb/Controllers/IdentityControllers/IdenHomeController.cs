using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WmsForWeb.Controllers.IdentityControllers
{
    public class IdenHomeController : Controller
    {
        [Authorize] // 驗證屬性: 表示只有經過認證的才可使用該方法
        public ActionResult Index()
        {
            //Dictionary<string, object> data
            //   = new Dictionary<string, object>();
            //data.Add("Placeholder", "Placeholder");
            //return View(data);

            return View(this.GetData("Index"));
        }

        [Authorize(Roles = "Users")] // 只讓聲明中有Users權限的使用者可使用該方法
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }

        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>() {
            // 取得Action名稱
            {"Action", actionName}, 
            // 取得當前使用者的名稱
            {"User",HttpContext.User.Identity.Name},
            // 取得當前使用者是否以驗證
            {"Authenticated",HttpContext.User.Identity.IsAuthenticated },
            // 取得當前使用者驗證的形式
            {"Authentication Type",HttpContext.User.Identity.AuthenticationType },
            // 判斷當前使用者是否有在 User(權限) 中
            {"In Users Role",HttpContext.User.IsInRole("Users") }
        };
            return dict;
        }
    }
}