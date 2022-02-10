using Microsoft.AspNet.Identity.EntityFramework;

namespace WmsForWeb.IdentityModels
{

    /// <summary>
    ///  該類別是為使用者註冊 角色(細項權限)時所需資訊 ViewModel
    ///  繼承自 <see cref="IdentityRole"/>
    /// </summary>
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        // 建立權限時會用到多載建構式
        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}