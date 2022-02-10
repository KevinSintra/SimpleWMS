using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WmsAllModels.WmsModels;
using MVC_Service;
using MVC_Service.Interface;
using WmsForWeb.IdentityInfrastructure.Attributes;
using WebSystemLog;


namespace WmsForWeb.Controllers
{
    [Authorize]
    public class FangGoodsMastController : Controller
    {
        private IGenericService<FangGoodsMast> _Service = new GenericService<FangGoodsMast>(new WebWmsModel());
        private IGenericService<GoodMastBarcode> _Service2 = new GenericService<GoodMastBarcode>(new WebWmsModel());

        //public FangGoodsMastController(IGenericService<FangGoodsMast> Service, IGenericService<GoodMastBarcode> Service2)
        //{
        //    this._Service = Service;
        //    this._Service2 = Service2;
        //}

        [FuncRole(FuncName = "商品主檔", seleCheck = true)]
        public ActionResult Search(string inPdCode, string inPdName)//, TemperatureLayer inPdTemperatureLayer
        {
            var data = new List<FangGoodsMast>();
            //data = this._Service.GetFilterListViewModel<FangGoodsMast>(x => x.PdCode == inPdCode
            //  || x.PdName == inPdName || x.TemperatureLayer == inPdTemperatureLayer);
            data = new WebWmsModel().FangGoodsMast.AsNoTracking().Where(x =>
                x.PdCode.Contains(inPdCode) || x.PdName.Contains(inPdName)).ToList();
            return View("Index", data);
        }

        // GET: FangGoodsMast
        [FuncRole(FuncName = "商品主檔", seleCheck = true)]
        public ActionResult Index()//List<FangGoodsMast> inData
        {
            //var dar = this.Url;
            //if (!(inData is null))
            //    return View(inData);
            return View(this._Service.GetListToViewModel<FangGoodsMast>());
        }

        //[ActionName(name: "Index"),ChildActionOnly, FuncRole(FuncName = "商品主檔", seleCheck = true)]
        //public ActionResult SelectIndex()
        //{
        //    return View(inData);
        //}

        // GET: FangGoodsMast/Details/5
        [FuncRole(FuncName = "商品主檔", seleCheck = true)]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fangGoodsMast =
                this._Service.GetSpecificDetailToViewModel<FangGoodsMast>(x => x.Pd_Id == id);
            if (fangGoodsMast == null)
                return HttpNotFound();

            return View(fangGoodsMast);
        }

        // GET: FangGoodsMast/Create
        [FuncRole(FuncName = "商品主檔", addCheck = true)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FangGoodsMast/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([
            Bind(Include = "Pd_Id,PdCode,PdName,DeleteFlag,AllowDay,CaseQty,BoxQty,CaseQty_Level,HightLevel,PickStock,PdLength,PdWidth,PdHeight,PdVolume,PdWeight,TemperatureLayer,PdSort,StopSell,LowestSellDay,Supplier_1,Replenish_Online,Replenish_Reserve,CostPriceAve,LastInPrice,SuggestPrice,SuggestPriceAve,SellPrice,SellPriceAve,UpdTime,UpdUser")]
            FangGoodsMast fangGoodsMast,
            ViewModels.BarcodeUse mastBarcode)//[Bind(Include = "Sort,Barcode")]
        {
            var checkBarcode = mastBarcode.GetGoodMastBarcode(fangGoodsMast);
            if (checkBarcode is null)
                ModelState.AddModelError("", "商品的條碼欄位不可重複");
            if (ModelState.IsValid)
            {
                fangGoodsMast.UpdTime = DateTime.Now;
                fangGoodsMast.UpdUser = HttpContext.User.Identity.Name;
                fangGoodsMast.PdVolume = 0;
                // 新增商品主檔
                this._Service.CreateViewModelToDatabase(fangGoodsMast);

                // 取得對應的 Barcode Model 的集合資料
                var barcodeData = mastBarcode.GetGoodMastBarcode(
                    this._Service.GetSpecificDetailToViewModel<FangGoodsMast>(x =>
                    x.PdCode == fangGoodsMast.PdCode));

                // 新增條碼資料
                if (barcodeData.Count > 0)
                    barcodeData.ForEach(x => {
                        this._Service2.CreateViewModelToDatabase(x);
                    });

                return RedirectToAction("Index");
            }
            return View(fangGoodsMast);
        }

        // GET: FangGoodsMast/Edit/5
        [FuncRole(FuncName = "商品主檔", updCheck = true)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fangGoodsMast =
                this._Service.GetSpecificDetailToViewModel<FangGoodsMast>(x => x.Pd_Id == id);
            if (fangGoodsMast == null)
                return HttpNotFound();

            return View(fangGoodsMast);
        }

        // POST: FangGoodsMast/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pd_Id,PdCode,PdName,DeleteFlag,AllowDay,CaseQty,BoxQty,CaseQty_Level,HightLevel,PickStock,PdLength,PdWidth,PdHeight,PdVolume,PdWeight,TemperatureLayer,PdSort,StopSell,LowestSellDay,Supplier_1,Replenish_Online,Replenish_Reserve,CostPriceAve,LastInPrice,SuggestPrice,SuggestPriceAve,SellPrice,SellPriceAve,UpdTime,UpdUser")] FangGoodsMast fangGoodsMast)
        {
            if (ModelState.IsValid)
            {
                fangGoodsMast.UpdTime = DateTime.Now;
                fangGoodsMast.UpdUser = HttpContext.User.Identity.Name;
                fangGoodsMast.PdVolume = fangGoodsMast.PdVolume;
                this._Service.UpdateViewModelToDatabase(fangGoodsMast);
                return RedirectToAction("Index");
            }
            return View(fangGoodsMast);
        }

        // GET: FangGoodsMast/Delete/5
        [FuncRole(FuncName = "商品主檔", deleCheck = true)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fangGoodsMast =
                this._Service.GetSpecificDetailToViewModel<FangGoodsMast>(x => x.Pd_Id == id);
            if (fangGoodsMast == null)
                return HttpNotFound();

            return View(fangGoodsMast);
        }

        // POST: FangGoodsMast/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this._Service.Delete(x => x.Pd_Id == id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._Service.Dispose();
                this._Service2.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
