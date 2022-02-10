using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using WmsAllModels.WmsModels;

namespace WmsAllModels.ALLAttributes.GoodsMastAttributes
{
    public class CheckCaseBoxQytAttributes : ValidationAttribute//, IClientValidatable
    {
        //public override bool IsValid(object value)
        //{
        //    return base.IsValid(value);
        //}

        // validationContext 這個參數是針對當前使用者的模型資料
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            FangGoodsMast fangGoodsMast = (validationContext.ObjectInstance as FangGoodsMast);
            //var errorMsg = string.Format("箱({0}) 盒 {1}", fangGoodsMast.CaseQty, fangGoodsMast.BoxQty);
            if (fangGoodsMast.CaseQty != 0 && fangGoodsMast.BoxQty != 0)
                if ((fangGoodsMast.CaseQty % fangGoodsMast.BoxQty) != 0 && fangGoodsMast.CaseQty != 1)
                    return new ValidationResult("箱入數必須能夠被盒入數整除");

            return ValidationResult.Success;
        }
    }

    public class CheckTemperatureLayerAttributes : ValidationAttribute//, IClientValidatable
    {
        //public override bool IsValid(object value)
        //{
        //    return base.IsValid(value);
        //}

        // validationContext 這個參數是針對當前使用者的模型資料
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //FangGoodsMast fangGoodsMast = (validationContext.ObjectInstance as FangGoodsMast);
            //var errorMsg = string.Format("箱({0}) 盒 {1}", fangGoodsMast.CaseQty, fangGoodsMast.BoxQty);
            var temperatureLayer = (TemperatureLayer)value;
            
            if ((int)temperatureLayer == 0)
                return new ValidationResult("溫層必須被選擇");

            return ValidationResult.Success;
        }
    }
}