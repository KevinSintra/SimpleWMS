using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WmsForWeb.IdentityInfrastructure;

namespace WmsForWeb
{
    /// <summary>
    /// Identity 的配置檔類別
    /// </summary>
    public class IdentityConfig
    {
        /// <summary>
        /// 此類別是 <see cref="Owin"/> AND <see cref="Identity"/> 中的 APP 設定檔。
        /// 主要就是在此專案中 自定義建立的 Identity 類別
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            // 設定 IdentityDbContext 並提供返回對應型別實例的方法
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            // 設定 IdentityUserManager 並提供返回對應型別實例的方法
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            // 設定 IdentityRoleManager 並提供返回對應型別實例的方法
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            // 設定 Owin 中間層的 Cookie 認證機制
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/IdenAccount/Login"), // 若未驗證 導向於指定頁面
            });
        }
    }
}