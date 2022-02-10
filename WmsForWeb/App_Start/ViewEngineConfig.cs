using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WmsForWeb
{
    public class ViewEngineConfig
    {
        public static void RegisterViewEngines(ViewEngineCollection viewEngines)
        {
            // 清除所有 View Engine
            viewEngines.Clear();
            // 重新註冊 RazorViewEngine，如果你使用的是 WebForm ViewEngine 則是加入 WebFormViewEngine
            viewEngines.Add(new CSharpRazorViewEngine());
        }

        internal class CSharpRazorViewEngine : RazorViewEngine
        {
            public CSharpRazorViewEngine()
            {
                base.AreaViewLocationFormats = new[]{
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                };
                base.AreaMasterLocationFormats = new[]{
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                };
                base.AreaPartialViewLocationFormats = new[]{
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                };
                base.ViewLocationFormats = new[]{
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/IdentityViews/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                };
                base.MasterLocationFormats = new[]{
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/IdentityViews/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                };
                base.PartialViewLocationFormats = new[]{
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                };
                base.FileExtensions = new[]{
                    "cshtml"
                };
            }
        }
    }
}