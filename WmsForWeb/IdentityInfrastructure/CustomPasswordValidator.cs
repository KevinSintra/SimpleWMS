using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace WmsForWeb.IdentityInfrastructure
{
    /// <summary>
    /// 此類別繼承 PasswordValidator 類別，主要針對 ValidateAsync 原方法做複寫。
    /// 該方法主要就是執行原方法的 使用者創建帳號時 密碼的相關驗證。後增加自定義的驗證
    /// </summary>
    public class CustomPasswordValidator : PasswordValidator
    {
        /// <summary>
        /// 使用者創建帳號使 針對密碼所做的驗證
        /// </summary>
        public override async Task<IdentityResult> ValidateAsync(string pwd)
        {
            // 等待 父類別的非同步方法跑完病回傳 IdentityResult
            // 這個Identity方法 是依據原本規則的設定值 去跑它既有的驗證 此時 
            // 跑完後 會再跑我們自行增加的驗證
            IdentityResult result = await base.ValidateAsync(pwd);
            // 抓取原本的錯誤資訊 不管有無資料
            var errors = result.Errors.ToList();
            //if (pwd.Contains("12345"))
            //{
            //    errors.Add("密碼不能包含如 12345 這種順向排列的數字");
            //    // 當 Identity 的 IdentityResult 被覆寫掉的同時 那並不會繼續執行後續動作
            //    result = new IdentityResult(errors);
            //}
            if (string.IsNullOrEmpty(pwd))
            {
                errors.Add("密碼不能為空值");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}