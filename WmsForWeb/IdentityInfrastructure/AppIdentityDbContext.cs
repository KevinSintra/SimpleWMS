using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WmsForWeb.IdentityModels;
using Microsoft.AspNet.Identity;

namespace WmsForWeb.IdentityInfrastructure
{
    /// <summary>
    /// 此類別是設定 Identity 連線至資料庫伺服的基本設定檔。等同於使用Entity中的DbContex類別
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    //該泛型中的類型就是剛剛建立的 AppUser 類別 條件約束是 where T : IdentityUser
    {
        public AppIdentityDbContext() : base("WebWmsIdentityDb")
        {
        }

        static AppIdentityDbContext()
        {
            // 該泛型 必須是自己配置的 類別 才會產生效用
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        /// <summary>
        /// 主要是在 IdentityConfig 中，需要返回實例的方法
        /// </summary>
        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }

    /// <summary>
    /// 資料庫初始化時的基本設定
    /// 若 DataModel 結構異動時資料庫的規則為以下設定。
    /// 且 EF 會洗掉資料庫資料，寫入新的資料庫
    /// 繼承自 <see cref="DropCreateDatabaseIfModelChanges{TContext}"/> 會影響 Identity 更新資料庫結構時的變化
    /// </summary>
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            // 在網站初始(第一次建立資料表時)時 需要配給一個Admin管理者 此時可以先外加工
            // 以搭配Controller的驗證機制與規則

            // 新增 使用者管理器類 並使用自定義連線
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            // 新增 權限管理器類 並使用自定義連線
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            // 建立一個權限管理者的相關資訊
            var adminInfo = new
            {
                roleName = "Administrator",
                userName = "Admin",
                password = "~ko1KO1~",
                email = "AdminAdmin@gmail.com"
            };

            AppUser user = userMgr.FindByName(adminInfo.userName);

            if (user == null)
            {
                // 創建網站管理員帳號
                userMgr.Create(new AppUser() {
                    UserName = adminInfo.userName, Email = adminInfo.email }, adminInfo.password);
                user = userMgr.FindByName(adminInfo.userName);
            }

            // 若對應的權限名稱不存在 則創建指定權限
            if (!roleMgr.RoleExists(adminInfo.roleName))
                roleMgr.Create(new AppRole(adminInfo.roleName));

            // 若使用者中無對應的權限 則賦予指定使用者指定的權限
            if (!userMgr.IsInRole(user.Id, adminInfo.roleName))
                userMgr.AddToRole(user.Id, adminInfo.roleName);
        }
    }
}