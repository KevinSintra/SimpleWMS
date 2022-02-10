using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WmsForWeb.IdentityModels.ViewModels
{
    public class IdenFuncEdit
    {
        public AppRole Role { get; set; }

        public IList<WmsFunucMast> AllFunc { get; set; }

        public IList<WmsRoleFuncMast> RoleForFunc { get; set; }
    }

    public class IdenFuncUpd
    {
        public int[] Func_Id { get; set; }
        public string[] Func_Name { get; set; }
        public string[] Role_Name { get; set; }
        public bool[] AddFunc { get; set; }
        public bool[] DeleFunc { get; set; }
        public bool[] UpdFunc { get; set; }
        public bool[] SeleFunc { get; set; }
    }
}