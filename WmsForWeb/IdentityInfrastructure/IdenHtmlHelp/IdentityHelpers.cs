using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace WmsForWeb.IdentityInfrastructure.IdenHtmlHelp
{
    /// <summary>
    /// 新增自定義的 <see cref="HtmlHelper"/> 類的擴充方法
    /// </summary>
    public static class IdentityHelpers
    {
        /// <summary>
        /// 使用權限管理器類下的 UserId。
        /// 該方法透過 使用者管理器類 找到對應使用者的 Name 並回傳 <see cref="MvcHtmlString"/> 
        /// </summary>
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            // 取得 使用者管理器類
            AppUserManager userMgr = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            // 使用 使用者管理器類的方法 後等待回傳Result 再從回傳資料中取得 UserName
            return new MvcHtmlString(userMgr.FindByIdAsync(id).Result.UserName);
        }
    }
}