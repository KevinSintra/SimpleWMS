using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WmsForWeb.IdentityModels;

namespace WmsForWeb.IdentityInfrastructure
{
    /// <summary>
    /// 此為 Identity 所提供的 <see cref="UserManager{TUser}"/>。
    /// 主要就是 SQLSERVER 取資料提供相對應的方法 
    /// </summary>
    public class AppUserManager : UserManager<AppUser>
    // UserManager類別的泛型 的條件約束 where T : class,Identity.IUser 
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

        /// <summary>
        /// 主要是在 IdentityConfig 中，需要返回實例的方法
        /// </summary>
        public static AppUserManager Create(
             IdentityFactoryOptions<AppUserManager> options,
             IOwinContext context)
        {
            // 透過 Owin 取得對應的 DbContext 的實例
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            // 取得對應的 AppuserManger 類別
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            // 使用 Identity 的 UserManager 類別 來初始化 PasswordValidator 物件 
            manager.PasswordValidator = new CustomPasswordValidator()
            {
                RequiredLength = 6, // 密碼長度需大於六
                RequireNonLetterOrDigit = false, // 必須有特殊符號
                RequireDigit = false, // 必須要有數字
                RequireLowercase = true, // 必須要有小寫字母
                RequireUppercase = true // 必須要有大寫字母
            };

            // 使用 Identity 的 UserManager 類別 來初始化 UserValidator 物件 
            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = true, // 帳號需要有字母與數字組合
                RequireUniqueEmail = true // Email 需是未註冊過的
            };

            return manager;
        }
    }
}