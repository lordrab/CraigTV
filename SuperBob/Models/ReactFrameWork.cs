using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;


namespace SuperBob.Models
{
    public class PopupSaveResultModel
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public bool AddRecord { get; set; }
    }

    public class PopupDeleteResultModel
    {
        public bool Success { get; set; }
    }

    public class ReactListResultModel<T>
    {      
        
        public List<T> DataList { get; set; }
        public List<ReactPopupModel> PropertyNames { get; set; }
             
    }
   
    public class ReactCreateEditDataModel<T> : PopupDeleteResultModel
    {
        public int Id { get; set; }
        public T Model { get; set; }       
        public List<ReactPopupModel> PopupDisplayModel { get; set; }

    }

    public class ReactPopupModel
    {
        public string DisplayName { get; set; }
        public string PropertyName { get; set; }
        public int TagType { get; set; }
        public bool DisplayProperty { get; set; }
        public string DropDownListName { get; set; }

    }

    public class ReactDropDownModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}