using System.Web.Mvc;
using WmsForWeb.IdentityModels.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using WmsForWeb.IdentityModels;

namespace WmsForWeb.Controllers.IdentityControllers
{
    /// <summary>
    /// 管理使用者的Controller
    /// </summary>
    [Authorize(Roles = "Administrator")] // 每個方法都需是管理者權限才能訪問
    public class IdenAdminController : IdentityBaseController
    {
        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            // 使用父類別中的方法取得使用者管理器的實例 後取得所有User的資料
            return View(base.BaseUserManager.Users);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateModel model)
        {
            // 檢驗前端驗證是否成功
            if (ModelState.IsValid)
            {
                // 將 ViewModel 的資料帶入 DataModel 中
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                // 使用 Identity 的新增方法，新增一筆資料並加使用者密碼加密。
                var result = await base.BaseUserManager.
                    CreateAsync(user, model.Password);
                // 若新增成功
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    base.BaseAddErrorsFromResult(result);
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            // 使用 Identity 使用者管理器類 的方法，找尋到對應的使用者
            // 並回傳一個含有資料的 DataModel
            AppUser user = await base.BaseUserManager.FindByIdAsync(id);
            if (user != null)
            {
                // 使用 Identity 使用者管理器類 的方法，刪除一筆相符的資料 並回傳狀況 IdentityResult
                IdentityResult result = await base.BaseUserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    return View("Error", result.Errors);
            }
            else
                return View("Error", new string[] { "User Not Found" });
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            // 使用 Identity 使用者管理器類 的方法，刪除一筆相符的資料 並回傳狀況 IdentityResult
            AppUser user = await base.BaseUserManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string email, string pwd)
        {
            // 先透過 ID 取得 對應 User 的 DataModel
            AppUser user = await base.BaseUserManager.FindByIdAsync(id);
            if (user != null && ModelState.IsValid)
            {
                user.Email = email;
                // 新增接收 使用者管理器類 的變數
                IdentityResult validEmail = null, validPass = null;
                // 先修改了User 中的 Email 的值，後使用預設的驗證 
                validEmail = await base.BaseUserManager.UserValidator.ValidateAsync(user);
                // 先不修改原Password原因是怕原本的驗證就沒通過就直接加密後修改，所以先對原字串做驗證
                if (!string.IsNullOrEmpty(pwd)) // 針對當前的 密碼 進行驗證
                    validPass = await base.BaseUserManager.PasswordValidator.ValidateAsync(pwd);
                else
                    validPass = new IdentityResult("密碼不可為空 必填");
                // 若 信箱 驗證不通過 則透過 AddErrorsFromResult 方法新增錯誤資訊到 ModelState
                if (!validEmail.Succeeded) base.BaseAddErrorsFromResult(validEmail);
                // 若 密碼 驗證不通過 則透過 AddErrorsFromResult 方法新增錯誤資訊到 ModelState
                if (!validPass.Succeeded) base.BaseAddErrorsFromResult(validPass);
                // 若 密碼 AND 信箱 驗證通過 就透過 Identity 的方法修改其資料庫中的資料
                if (validEmail.Succeeded && validPass.Succeeded)
                {
                    // 若驗證成功且 則 將原密碼加密後修改原值
                    user.PasswordHash = base.BaseUserManager.PasswordHasher.HashPassword(pwd);
                    IdentityResult result = await base.BaseUserManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        base.BaseAddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");

            return View(user);
        }
    }
}