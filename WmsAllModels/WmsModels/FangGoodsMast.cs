using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WmsAllModels.ALLAttributes.GoodsMastAttributes;

namespace WmsAllModels.WmsModels
{
    /// <summary>
    /// 溫層的選定
    /// </summary>
    public enum TemperatureLayer
    {
        預設, 食品, 用品, 冷藏, 冷凍
    }

    public class FangGoodsMast
    {
        [Key]
        public int Pd_Id { get; set; }

        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Index("IX_FangGoodsMast_PdCode")] // , Column(Order = 1)
        [Display(Name = "商品編號"), MinLength(4), MaxLength(length: 20), Required]
        [RegularExpression(pattern: @"[a-zA-Z0-9]{4,20}")]
        public string PdCode { get; set; }

        [Display(Name = "商品名稱"), Required, MinLength(4), MaxLength(50)]
        public string PdName { get; set; }

        [Display(Name = "刪除標記")]
        public bool? DeleteFlag { get; set; }

        [Display(Name = "允收期限"), Required, Range(90, 1825)]
        public Int16 AllowDay { get; set; }

        [Display(Name = "箱入數"), Required, Range(1, 512)]//,Compare(
        [CheckCaseBoxQytAttributes]
        public Int16 CaseQty { get; set; }

        [Display(Name = "盒入數"), Required, Range(1, 256)]
        public Int16 BoxQty { get; set; }

        [Display(Name = "一層箱數/Plt"), Required, Range(1, 200)]
        public Int16 CaseQty_Level { get; set; }

        [Display(Name = "一板幾層/Plt"), Required, Range(1, 12)]
        public Int16 HightLevel { get; set; }

        //TODO: 要依溫層來選擇固定的庫存儲位
        [Index("IX_FangGoodsMast_PickStock")]
        [Display(Name = "揀貨儲位"), Required, StringLength(maximumLength: 7)]
        public string PickStock { get; set; }

        [Display(Name = "箱長度/cm"), Required, Range(3, 135)]
        public Int16 PdLength { get; set; }

        [Display(Name = "箱寬度/cm"), Required, Range(3, 135)]
        public Int16 PdWidth { get; set; }

        [Display(Name = "箱高度/cm"), Required, Range(3, 120)]
        public Int16 PdHeight { get; set; }

        [NotMapped] // 給均實際售價 做四捨五入用
        private double _PdVolume = 0.0001D;

        [Display(Name = "材積"), Range(minimum: 0.0001D, maximum: 9999999.9999D)]
        public double PdVolume {
            get {
                return this._PdVolume;
            }
            //set { if (value.HasValue) this._PdVolume = (Double)decimal.Round((decimal)value, 6); }
            set {
                if ((this.PdLength * this.PdWidth * this.PdHeight * this.CaseQty) != 0)
                {
                    this._PdVolume = Math.Round((double)this.PdLength * this.PdWidth * this.PdHeight / (27818 * this.CaseQty), 4);
                    if (this._PdVolume == 0) this._PdVolume = 0.0001;
                }
            }
        }

        [Display(Name = "箱重量/g"), Required, Range(10, 32000)]
        public Int16 PdWeight { get; set; }

        [Display(Name = "溫層"), Required]
        [CheckTemperatureLayerAttributes]
        public TemperatureLayer TemperatureLayer { get; set; }

        [Display(Name = "商品類別"), Required, MinLength(2), MaxLength(12)]
        public string PdSort { get; set; }

        [Display(Name = "停售?")]
        public bool StopSell { get; set; }

        [Display(Name = "最低可銷售天數"), Range(30, 180)]
        public Int16? LowestSellDay { get; set; }

        [Display(Name = "供應商代號"), Required, StringLength(maximumLength: 3)]
        [RegularExpression(pattern: @"[a-zA-Z0-9]{3}")]
        public string Supplier_1 { get; set; }

        //[Display(Name = "Piece條碼"), Required, MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string EaBarcode_1 { get; set; }

        //[Display(Name = "Pice條碼2"), MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string EaBarcode_2 { get; set; }

        //[Display(Name = "盒條碼"), Required, MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string BoxBarcode_1 { get; set; }

        //[Display(Name = "盒條碼2"), MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string BoxBarcode_2 { get; set; }

        //[Display(Name = "箱條碼"), Required, MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string CaseBarcode_1 { get; set; }

        //[Display(Name = "箱條碼2"), MinLength(3), MaxLength(20)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        //public string CaseBarcode_2 { get; set; }

        //[Display(Name = "供應商代號2"), StringLength(maximumLength: 3)]
        //[RegularExpression(pattern: @"[a-zA-Z0-9]{3}")]
        //public string Supplier_2 { get; set; }

        //[Display(Name = "供應商代號3"), StringLength(maximumLength: 3)]
        //[RegularExpression(pattern: @"\W{3,20}")]
        //public string Supplier_3 { get; set; }

        [Display(Name = "即時補貨點"), Range(3, 1200)]
        public Int16? Replenish_Online { get; set; }

        [Display(Name = "預約補貨點"), Range(3, 1200)]
        public Int16? Replenish_Reserve { get; set; }

        [NotMapped] // 給平均成本 做四捨五入用
        private decimal? _CostPriceAve = 0.00M;

        [Display(Name = "平均成本"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? CostPriceAve {
            get { return this._CostPriceAve; }
            set { if (value.HasValue) this._CostPriceAve = decimal.Round((decimal)value, 2); }
        }

        [NotMapped] // 給最後進價 做四捨五入用
        private decimal? _LastInPrice = 0.00M;

        [Display(Name = "最後進價"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? LastInPrice {
            get { return this._LastInPrice; }
            set { if (value.HasValue) this._LastInPrice = decimal.Round((decimal)value, 2); }
        }

        [NotMapped] // 給建議售價 做四捨五入用
        private decimal? _SuggestPrice = 0.00M;

        [Display(Name = "建議售價"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? SuggestPrice {
            get { return this._SuggestPrice; }
            set { if (value.HasValue) this._SuggestPrice = decimal.Round((decimal)value, 2); }
        }

        [NotMapped]   // 給均建議售價 做四捨五入用
        private decimal? _SuggestPriceAve = 0.00M;

        [Display(Name = "均建議售價"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? SuggestPriceAve {
            get { return this._SuggestPriceAve; }
            set { if (value.HasValue) this._SuggestPriceAve = decimal.Round((decimal)value, 2); }
        }

        [NotMapped] // 給實際售價 做四捨五入用
        private decimal? _SellPrice = 0.00M;

        [Display(Name = "實際售價"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? SellPrice {
            get { return this._SellPrice; }
            set { if (value.HasValue) this._SellPrice = decimal.Round((decimal)value, 2); }
        }

        [NotMapped] // 給均實際售價 做四捨五入用
        private decimal? _SellPriceAve = 0.00M;

        [Display(Name = "均實際售價"), Range(minimum: 0.00, maximum: 9999999999.99)]
        public decimal? SellPriceAve {
            get { return this._SellPriceAve; }
            set { if (value.HasValue) this._SellPriceAve = decimal.Round((decimal)value, 2); }
        }

        //[NotMapped]
        //private DateTime _time = DateTime.Now;

        [Display(Name = "修改時間")] // 自動設定時間
        public DateTime UpdTime { get; set; }

        [Display(Name = "修改使用者"), MaxLength(20)]
        public string UpdUser { get; set; }

        public virtual ICollection<GoodMastBarcode> GoodMastBarcode { get; set; }
    }
}