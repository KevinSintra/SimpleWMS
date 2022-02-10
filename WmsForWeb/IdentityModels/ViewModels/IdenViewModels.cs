using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WmsForWeb.IdentityModels.ViewModels
{
    /// <summary>
    /// 該類別是使用者創立帳號時所用的ViewModel
    /// </summary>
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// 該類別是使用者登入帳號時所用的ViewModel
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// 該類別是修改 權限管理器資料表 時，方便歸類顯示於頁面上所建立的 ViewModel
    /// </summary>
    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        /// <summary>
        /// 目前在權限中的使用者 的AppUser集合
        /// </summary>
        public IList<AppUser> Members { get; set; }
        /// <summary>
        /// 目前沒有在權限中的使用者 的AppUser集合
        /// </summary>
        public IList<AppUser> NonMnebers { get; set; }
    }

    /// <summary>
    /// 這是 真正修改 權限管理器資料表 時，所必要的資訊。
    /// </summary>
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        /// <summary>
        /// 權限欲新增的使用者
        /// </summary>
        public string[] IdsToAdd { get; set; }
        /// <summary>
        /// 權限欲刪除的使用者
        /// </summary>
        public string[] IdsToDelete { get; set; }
    }
}