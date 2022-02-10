using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WmsForWeb.IdentityModels;
using WmsForWeb.IdentityModels.ViewModels;

using System.Collections.Generic;
using System.Linq;

namespace WmsForWeb.Controllers.IdentityControllers
{
    [Authorize(Roles = "Administrator")]
    public class IdenRoleAdminController : IdentityBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(base.BaseRoleManager.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Required]string name) // 這個參數算是小型的 ViewModel
        {
            if (ModelState.IsValid)
            {
                //base.BaseRoleManager.Roles;
                //base.BaseUserManager.GetRoles(HttpContext.User.Identity.GetUserId());                

                // 新增權限名稱
                IdentityResult result = await base.BaseRoleManager.CreateAsync(new AppRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    base.BaseAddErrorsFromResult(result);
            }
            return View(name);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            // 透過ID找尋對應的權限(腳色)
            AppRole role = await base.BaseRoleManager.FindByIdAsync(id);
            // 若找不到則跳轉到錯誤頁面
            if (role is null) return View("Error", new string[] { "Role Not Found" });
            // 刪除該權限(腳色)
            IdentityResult result = await base.BaseRoleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                return View("Error", result.Errors);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            // 透過ID找尋對應的Role(權限) 並回傳 DataModel
            AppRole role = await base.BaseRoleManager.FindByIdAsync(id);
            if (role == null) return View("Error", new string[] { "Role Not Found" });
            // 針對目前權限中的使用者，撈取使用者Id並儲存成陣列
            string[] membersId = role.Users.Select(x => x.UserId).ToArray();

            // 透過 使用者管理器 取得全部的 使用者 後與 陣列的值比對，儲存成 AppUser 集合
            IList<AppUser> members = new List<AppUser>(); // 目前權限中的使用者，儲存成集合
            IList<AppUser> nonMambers = new List<AppUser>(); // 目前不再權限中的使用者，儲存成集合
            var allUser = base.BaseUserManager.Users.ToList(); // 取得所有使用者並明確轉型成 List
            allUser.ForEach(user => {
                if (membersId.Any(arr => arr == user.Id))
                    members.Add(user);
                else
                    nonMambers.Add(user);
            });
            // -------------------------------------------------------------------------
            return View(new RoleEditModel() { Role = role, Members = members, NonMnebers = nonMambers });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleModificationModel inModel)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                // note : inModel.IdsToAdd ?? new string[] { } 代表 若陣列為 null 回傳一組 匿名陣列 
                foreach (string userId in inModel.IdsToAdd ?? new string[] { })
                {
                    // 針對權限新增成員(使用者)
                    result = await base.BaseUserManager.AddToRoleAsync(userId, inModel.RoleName);
                    // 若新增失敗回傳錯誤頁面
                    if (!result.Succeeded) return View("Error", result.Errors);
                }
                foreach (string userId in inModel.IdsToDelete ?? new string[] { })
                {
                    // 針對權限移除成員(使用者)
                    result = await base.BaseUserManager.RemoveFromRoleAsync(userId, inModel.RoleName);
                    // 若新增失敗回傳錯誤頁面
                    if (!result.Succeeded) return View("Error", result.Errors);
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
        }
    }
}