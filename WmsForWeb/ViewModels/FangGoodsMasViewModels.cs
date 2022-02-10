using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using WmsAllModels.WmsModels;

namespace WmsForWeb.ViewModels
{
    /// <summary>
    /// Barcode 使用的 ViewModel
    /// </summary>
    public class BarcodeUse
    {
        public EaBoxCase Sort1 { get; set; }

        public string Barcode1 { get; set; }

        public EaBoxCase Sort2 { get; set; }

        public string Barcode2 { get; set; }

        public EaBoxCase Sort3 { get; set; }

        public string Barcode3 { get; set; }

        public EaBoxCase Sort4 { get; set; }

        public string Barcode4 { get; set; }

        public EaBoxCase Sort5 { get; set; }

        public string Barcode5 { get; set; }

        public EaBoxCase Sort6 { get; set; }

        public string Barcode6 { get; set; }

        private DateTime _datetime;

        private string _user;

        /// <summary>
        /// 依據當前資料回傳對應的 Barcode 集合資料
        /// </summary>
        /// <param name="inGoodMastData"> 當前新增或修改的 FangGoodMast 資料 </param>
        /// <returns> 回傳 Barcode 對應的 Model集合資料 </returns>
        public List<GoodMastBarcode> GetGoodMastBarcode(FangGoodsMast inGoodMastData)
        {
            var returnData = new List<GoodMastBarcode>();

            var pdId = inGoodMastData.Pd_Id;
            var pdCode = inGoodMastData.PdCode;
            this._datetime = DateTime.Now;
            this._user = inGoodMastData.UpdUser;

            if (this.CreateData(this.Sort1, this.Barcode1, pdId, pdCode, ref returnData) ||
            this.CreateData(this.Sort2, this.Barcode2, pdId, pdCode, ref returnData) ||
            this.CreateData(this.Sort3, this.Barcode3, pdId, pdCode, ref returnData) ||
            this.CreateData(this.Sort4, this.Barcode4, pdId, pdCode, ref returnData) ||
            this.CreateData(this.Sort5, this.Barcode5, pdId, pdCode, ref returnData) ||
            this.CreateData(this.Sort6, this.Barcode6, pdId, pdCode, ref returnData))
                return null;

            return returnData;
        }

        private bool CreateData(EaBoxCase inSort, string inBarcode, int inPdId, string inPdcode,
            ref List<GoodMastBarcode> inNewData)
        {
            if ((!string.IsNullOrEmpty(inBarcode)))// && (inSort != EaBoxCase.預設)
            {
                // 判斷條碼有無重複
                if (inNewData.Count > 0 && inNewData.Any(x => x.Barcode == inBarcode))
                    return true;

                inNewData.Add(new GoodMastBarcode {
                    Pd_Id = inPdId,
                    PdCode = inPdcode,
                    EaBoxCase = inSort,
                    Barcode = inBarcode,
                    UpdTime = this._datetime,
                    UpdUser = this._user
                });
            }
            return false;
        }
    }


}