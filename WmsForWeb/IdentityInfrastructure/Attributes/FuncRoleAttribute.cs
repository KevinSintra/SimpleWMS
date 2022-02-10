using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WmsForWeb.IdentityModels;
using System.Linq;
using MVC_Service;
using MVC_Service.Interface;

namespace WmsForWeb.IdentityInfrastructure.Attributes
{
    /// <summary>
    /// 針對權限來判斷功能是否開啟，請指定欲查檢的功能與當前功能名稱
    /// </summary>
    public class FuncRoleAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 功能名稱，輸入功能 Name
        /// </summary>
        public string FuncName { get; set; }

        /// <summary>
        /// 新增的查檢
        /// </summary>
        public bool addCheck { get; set; }

        /// <summary>
        /// 修改的查檢
        /// </summary>
        public bool updCheck { get; set; }

        /// <summary>
        /// 刪除的查檢
        /// </summary>
        public bool deleCheck { get; set; }

        /// <summary>
        /// 查詢的查檢
        /// </summary>
        public bool seleCheck { get; set; }


        protected IGenericService<WmsRoleFuncMast> _Service2 =
            new GenericService<WmsRoleFuncMast>(new WmsFuncIdentityDbEntities());

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // return base.AuthorizeCore(httpContext);
            // var nowRoleManager = httpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            // 透過Owin 的使用者管理器 取得當前使用者所有的權限名稱
            var userRole = httpContext.GetOwinContext().GetUserManager<AppUserManager>().
                GetRoles(httpContext.User.Identity.GetUserId());
            // 若使用者沒有任何權限
            if (userRole.Count == 0) return false;
            // 透過 存取服務取得 Controller 指定的功能，完全取回
            var data = this._Service2.GetFilterListViewModel<WmsRoleFuncMast>(x =>x.Func_Name == this.FuncName);
            // 若權限功能無該Controler指定功能的資料
            if (data.Count == 0) return false;

            var check = false;
            // 逐一比對取得的功能中，與使用者的權限有無對應
            data.ForEach(x => {
                if (userRole.Any(roleName => x.Role_Name == roleName))
                    check = this.checkFunc(x);
            });
            return check;
        }

        // 若功能權限與當前使用者對應，則比對 Controller 指定的功能是否開啟
        protected bool checkFunc(WmsRoleFuncMast inData)
        {
            bool add = true, upd = true, dele = true, select = true;

            if (this.addCheck)
                add = inData.AddFunc is true;
            if (this.updCheck)
                upd = inData.UpdFunc is true;
            if (this.deleCheck)
                dele = inData.DeleFunc is true;
            if (this.seleCheck)
                select = inData.SeleFunc is true;

            return (add && upd && dele && select);
        }
    }
}