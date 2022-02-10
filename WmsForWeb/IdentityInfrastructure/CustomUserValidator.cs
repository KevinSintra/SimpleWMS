using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WmsForWeb.IdentityModels;
using System.Web;

namespace WmsForWeb.IdentityInfrastructure
{
    /// <summary>
    /// 此類別繼承 <see cref="UserValidator{TUser}"/> 類別，主要針對 ValidateAsync 原方法做複寫。
    /// 該方法主要就是執行原方法的 使用者創建帳號時 帳號與信箱或其相關驗證。後增加自定義的驗證
    /// </summary>
    public class CustomUserValidator : UserValidator<AppUser>
    {
        public CustomUserValidator(UserManager<AppUser> manager) : base(manager)
        {
        }

        /// <summary>
        /// 使用者創立帳號時，針對除了密碼以外的欄位 如信箱、帳號等做相關的驗證
        /// </summary>
        public override async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            // 先跑父類的驗證
            IdentityResult result = await base.ValidateAsync(item);
            // 若信箱的結尾不等於 example.com 這個模式的話則驗證不過
            if (!item.Email.ToLower().EndsWith("@gmail.com"))
            {
                // 先把父類別的驗證錯誤資訊拷貝起來
                var errors = result.Errors.ToList();
                errors.Add("信箱的格式錯誤 請參照 : @gmail.com");
                result = new IdentityResult(errors);
            }

            // 看是否已經認證過
            if(!(HttpContext.Current is null))
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var userName = item.UserName.ToLower().Trim();
                    // 若帳號名稱與 admin && administrator 相符時，驗證不過
                    if (userName == "admin" || userName == "administrator")
                    {
                        var errors = result.Errors.ToList();
                        errors.Add("不可使用包含 admin && administrator 等字串組合");
                        result = new IdentityResult(errors);
                    }
                }
            }

            return result;
        }
    }
}