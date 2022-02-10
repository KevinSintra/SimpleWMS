using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using WmsForWeb.IdentityModels;
using WmsForWeb.IdentityModels.ViewModels;

namespace WmsForWeb.Controllers.IdentityControllers
{
    /// <summary>
    /// 使用者登入時增加認證相關的控制器
    /// </summary>
    [Authorize]
    public class IdenAccountController : IdentityBaseController
    {
        [HttpGet, AllowAnonymous] //該方法可不需驗證即可使用
        public ActionResult Login(string returnUrl)
        {
            // 若使用者已取得認證
            if (HttpContext.User.Identity.IsAuthenticated)
                return View("Error", new string[] { "已登入會員", "或是無權限造訪該頁面" });
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        // 該方法不需驗證 AND 預防跨網頁攻擊
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details, string returnUrl)
        {
            // 前端的頁面是否已通過驗證??
            if (ModelState.IsValid)
            {
                // 使用 Identity 中的方法，找尋 使用者姓名 與 密碼，有的話則傳回使用者對應的 DataModel
                AppUser user = await base.BaseUserManager.FindAsync(details.Name, details.Password);
                if (user != null)
                {
                    // 依據對應的 DataModle 建立起 認證機制的物件 ， 
                    ClaimsIdentity ident = await base.BaseUserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    // 先將當前使用者的 Cookie 認證清除
                    base.BaseAuthManager.SignOut();
                    // 將當前使用者使用 Cookie 賦予相關的認證資訊。 IsPersistent = false 是讓 Cookie 不為永久性的
                    // 此時 [Authorize] 最基本的驗證已可使用
                    base.BaseAuthManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, ident);
                    // 跳轉至登入前頁面
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "使用者名稱或密碼錯誤");
            }
            ViewBag.returnUrl = returnUrl; // 儲存使用者 登入前頁面的 URL
            return View(details);
        }

        public ActionResult Logout()
        {
            // 移除當前使用者的 Cookie 驗證，若有輸入參數 則是一次清除相同類型的驗證
            base.BaseAuthManager.SignOut();
            return RedirectToAction("Index", "IdenHome");
        }
    }
}