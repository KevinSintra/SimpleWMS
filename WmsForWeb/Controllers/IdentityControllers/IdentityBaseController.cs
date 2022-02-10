using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WmsForWeb.IdentityInfrastructure;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WmsForWeb.Controllers.IdentityControllers
{
    /// <summary>
    /// 此為所有Controller的共通類別
    /// </summary>
    public class IdentityBaseController : Controller
    {
        /// <summary>
        /// 取得當前 Identity 的 使用者管理器類 
        /// <see cref="AppUserManager"/>
        /// </summary>
        protected virtual AppUserManager BaseUserManager {
            get {
                //此方法是 Owin 對於 HttpContext 新增的擴充方法
                return base.HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        /// <summary>
        /// 此方法 是將 <see cref="IdentityResult"/> 中的 Errors 的資訊
        /// 新增到 ModelState 供 VIEW 使用。
        /// </summary>
        protected virtual void BaseAddErrorsFromResult(IdentityResult result)
        {
            result.Errors.ToList().ForEach(errMsg => ModelState.AddModelError("", errMsg));
        }

        /// <summary>
        /// 取得當前的 Identity 權限管理器類 <see cref="AppRoleManager"/>
        /// </summary>
        protected virtual AppRoleManager BaseRoleManager {
            get {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        /// <summary>
        /// 取得當前的 Identity 認證管理器類 <see cref="IAuthenticationManager"/>
        /// </summary>
        protected virtual IAuthenticationManager BaseAuthManager {
            get {
                // 此方法是 Owin 對於 HttpContext 新增的擴充方法
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}