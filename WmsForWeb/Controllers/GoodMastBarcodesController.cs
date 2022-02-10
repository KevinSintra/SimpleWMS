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

namespace WmsForWeb.Controllers
{
    [FuncRole(FuncName = "商品主檔", seleCheck = true)]
    public class GoodMastBarcodesController : Controller
    {
        //private WebWmsModel db = new WebWmsModel();
        private IGenericService<GoodMastBarcode> _Service2 = new GenericService<GoodMastBarcode>(new WebWmsModel());

        // GET: GoodMastBarcodes
        [HttpGet]
        public ActionResult Index(int? pdId, string pdCode)
        {
            if (pdId == null || string.IsNullOrEmpty(pdCode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var goodMastBarcode = this._Service2.GetFilterListViewModel<GoodMastBarcode>(
                x => x.Pd_Id == pdId && x.PdCode == pdCode);
            ViewBag.createUsePdId = pdId;
            ViewBag.createUsePdCode = pdCode;
            return View(goodMastBarcode);
        }

        // GET: GoodMastBarcodes/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GoodMastBarcode goodMastBarcode = db.GoodMastBarcode.Find(id);
        //    if (goodMastBarcode == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(goodMastBarcode);
        //}

        // GET: GoodMastBarcodes/Create

        public ActionResult Create(int? pdId, string pdCode)
        {
            //ViewBag.Pd_Id = new SelectList(db.FangGoodsMast, "Pd_Id", "PdCode");
            if (pdId == null || string.IsNullOrEmpty(pdCode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.createUsePdId = pdId;
            ViewBag.createUsePdCode = pdCode;
            return View();
        }

        // POST: GoodMastBarcodes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pd_Id,PdCode,Barcode,Barcode_Id,EaBoxCase")] GoodMastBarcode goodMastBarcode)
        {
            var checkBarcode = this._Service2.GetSpecificDetailToViewModel<GoodMastBarcode>(x =>
                x.Pd_Id == goodMastBarcode.Pd_Id &&
                x.PdCode == goodMastBarcode.PdCode &&
                x.Barcode == goodMastBarcode.Barcode);
            if (!(checkBarcode is null))
                ModelState.AddModelError("", "條碼重複請確認");
            if (ModelState.IsValid)
            {
                goodMastBarcode.UpdTime = DateTime.Now;
                goodMastBarcode.UpdUser = HttpContext.User.Identity.Name;
                this._Service2.CreateViewModelToDatabase(goodMastBarcode);
                return RedirectToAction("Index", new { pdId = goodMastBarcode.Pd_Id, pdCode = goodMastBarcode.PdCode });
            }
            //ViewBag.Pd_Id = new SelectList(db.FangGoodsMast, "Pd_Id", "PdCode", goodMastBarcode.Pd_Id);
            ViewBag.createUsePdId = goodMastBarcode.Pd_Id;
            ViewBag.createUsePdCode = goodMastBarcode.PdCode;
            return View(goodMastBarcode);
        }

        // GET: GoodMastBarcodes/Edit/5

        public ActionResult Edit(int? pdId, string pdCode, string Barcode)
        {
            if (pdId == null || string.IsNullOrEmpty(pdCode) || string.IsNullOrEmpty(Barcode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GoodMastBarcode goodMastBarcode = this._Service2.GetSpecificDetailToViewModel<GoodMastBarcode>(
                x => x.Pd_Id == pdId && x.PdCode == pdCode && x.Barcode == Barcode);
            if (goodMastBarcode == null)
                return HttpNotFound();
            //ViewBag.Pd_Id = new SelectList(db.FangGoodsMast, "Pd_Id", "PdCode", goodMastBarcode.Pd_Id);
            return View(goodMastBarcode);
        }

        // POST: GoodMastBarcodes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pd_Id,PdCode,Barcode,Barcode_Id,EaBoxCase")] GoodMastBarcode goodMastBarcode)
        {
            var checkBarcode = this._Service2.GetSpecificDetailToViewModel<GoodMastBarcode>(x =>
                x.Pd_Id == goodMastBarcode.Pd_Id &&
                x.PdCode == goodMastBarcode.PdCode &&
                x.Barcode == goodMastBarcode.Barcode &&
                x.Barcode_Id != goodMastBarcode.Barcode_Id);
            if (!(checkBarcode is null))
                ModelState.AddModelError("", "條碼重複請確認");
            if (ModelState.IsValid)
            {
                goodMastBarcode.UpdTime = DateTime.Now;
                goodMastBarcode.UpdUser = HttpContext.User.Identity.Name;
                this._Service2.UpdateViewModelToDatabase(goodMastBarcode);
                return RedirectToAction("Index", new { pdId = goodMastBarcode.Pd_Id, pdCode = goodMastBarcode.PdCode });
            }
            //ViewBag.Pd_Id = new SelectList(db.FangGoodsMast, "Pd_Id", "PdCode", goodMastBarcode.Pd_Id);
            return View(goodMastBarcode);
        }

        // GET: GoodMastBarcodes/Delete/5
        public ActionResult Delete(int? pdId, string pdCode, string Barcode)
        {
            if (pdId == null || string.IsNullOrEmpty(pdCode) || string.IsNullOrEmpty(Barcode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GoodMastBarcode goodMastBarcode = this._Service2.GetSpecificDetailToViewModel<GoodMastBarcode>(
                x => x.Pd_Id == pdId && x.PdCode == pdCode && x.Barcode == Barcode);
            if (goodMastBarcode == null)
                return HttpNotFound();
            return View(goodMastBarcode);
        }

        // POST: GoodMastBarcodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? pdId, string pdCode, string Barcode)
        {
            if (pdId == null || string.IsNullOrEmpty(pdCode) || string.IsNullOrEmpty(Barcode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            this._Service2.Delete(x => x.Pd_Id == pdId && x.PdCode == pdCode && x.Barcode == Barcode);
            return RedirectToAction("Index", new { pdId = pdId, pdCode = pdCode });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this._Service2.Dispose();
            base.Dispose(disposing);
        }
    }
}
