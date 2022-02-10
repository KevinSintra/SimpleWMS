using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WmsForWeb.IdentityModels;
using WmsForWeb.IdentityModels.ViewModels;
using MVC_Service;
using MVC_Service.Interface;
using System.Text;

namespace WmsForWeb.Controllers.IdentityControllers
{
    [Authorize(Roles = "Administrator")]
    public class IdenFuncAdminController : IdentityBaseController
    {
        private IGenericService<WmsFunucMast> _Service =
            new GenericService<WmsFunucMast>(new WmsFuncIdentityDbEntities());

        private IGenericService<WmsRoleFuncMast> _Service2 =
            new GenericService<WmsRoleFuncMast>(new WmsFuncIdentityDbEntities());

        [HttpPost, ValidateAntiForgeryToken] //ChildActionOnly
        public ActionResult Edit(string inRoleId)
        {
            // 使用權限ID找尋到權限Name
            var role = base.BaseRoleManager.FindById(inRoleId);

            // 取得所有系統功能的資料
            var funcData = this._Service.GetListToViewModel<WmsFunucMast>();

            // 查詢該權限是否有資料
            var roleFuncData = this._Service2.GetFilterListViewModel<WmsRoleFuncMast>(
                x => x.Role_Name == role.Name);

            // 回傳 ViewModel
            return View(new IdenFuncEdit { Role = role, AllFunc = funcData, RoleForFunc = roleFuncData });
        }

        [HttpPost, ValidateAntiForgeryToken] //ChildActionOnly
        public ActionResult EditRoleFunc(IdenFuncUpd inFuncData)
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //var updInserData = new List<WmsRoleFuncMast>();

            var count = inFuncData.Func_Id.Length;
            var inRoleName = (count == 0 ? "Not Data" : inFuncData.Role_Name[0]);

            // 找尋有無對應的資料 //x => x.Func_Id == inData.Func_Id && x.Func_Name == inData.Func_Name
            var roleFuncData = this._Service2.GetFilterListViewModel<WmsRoleFuncMast>(x =>
                    x.Role_Name == inRoleName);
            // TODO : 之後這裡若功能新增需要再確認邏輯是否能正確輸入資料 !!
            for (int i = 0; i < count; i++)
            {
                var inData = new WmsRoleFuncMast {
                    Func_Id = inFuncData.Func_Id[i],
                    Func_Name = inFuncData.Func_Name[i],
                    Role_Name = inFuncData.Role_Name[i],
                    AddFunc = inFuncData.AddFunc[i],
                    UpdFunc = inFuncData.UpdFunc[i],
                    DeleFunc = inFuncData.DeleFunc[i],
                    SeleFunc = inFuncData.SeleFunc[i]
                };

                if (roleFuncData.Count == 0) // 若無則新增
                    this._Service2.CreateViewModelToDatabase(inData);
                else if (roleFuncData.Count == count) // 若相同則修改
                    this._Service2.UpdateViewModelToDatabase(inData);
                else // 若筆數不同或來源資料筆數不等於0 報錯
                    throw new Exception("權限資料異常");
            }
            return RedirectToAction("Index", "IdenRoleAdmin");
            //return Redirect(@"~/IdenRoleAdmin/Index");
        }
    }
}