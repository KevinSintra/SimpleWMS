using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WmsForWeb.IdentityModels;

namespace WmsForWeb.IdentityInfrastructure
{
    /// <summary>
    /// 該類別是 Identity 所提供的 <see cref="RoleManager{TRole}"/>。主要就是將 權限相關的設定 寫入SQL
    /// 並提供相對應的方法。
    /// </summary>
    public class AppRoleManager : RoleManager<AppRole>
    // RoleManager<T> : 條件約束是 where : class,IRole
    {
        public AppRoleManager(IRoleStore<AppRole, string> store) : base(store)
        {
        }

        /// <summary>
        /// 此方法為 回傳 <see cref="AppRoleManager"/>實例。( 使用 <see cref="AppIdentityDbContext"/> 的連線設定 )
        /// </summary>
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> option,
            IOwinContext context)
        {
            // 創建一個帶參的 AppRoleManager 物件並回傳
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}