using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WmsAllModels.WmsModels
{
    /// <summary>
    /// 溫層的選定
    /// </summary>
    public enum EaBoxCase
    {
        箱, 盒, 個
    }

    public class GoodMastBarcode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 1)]
        public int Barcode_Id { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column(Order = 2), Required]
        public int Pd_Id { get; set; }

        [Required]//, Index("CX_GoodsMastBarcode_PdCode",IsClustered =true,Order =1)
        [MinLength(4), MaxLength(length: 20)]
        public string PdCode { get; set; }

        [Required]
        public EaBoxCase EaBoxCase { get; set; }

        [Index("IX_MastBarcode_Barcode"), Required]//, Index("CX_GoodsMastBarcode_PdCode", IsClustered = true, Order = 2)
        [MinLength(3), MaxLength(20), RegularExpression(pattern: @"[a-zA-Z0-9]{3,20}")]
        public string Barcode { get; set; }

        [Display(Name = "修改時間")] // 自動設定時間
        public DateTime UpdTime { get; set; }

        [Display(Name = "修改使用者"), MaxLength(20)]
        public string UpdUser { get; set; }

        [ForeignKey("Pd_Id")]
        public virtual FangGoodsMast GoodsMast { get; set; }
    }
}