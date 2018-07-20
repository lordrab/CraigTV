using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Models;
using System.Data.Entity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using SuperBob.Repository;
using SuperBob.Model;


namespace SuperBob
{
   
    public abstract class ReactBaseController<T, ViewListModel, EditViewModel, AddViewModel> : Controller where T : class, new()
        where ViewListModel : new() where EditViewModel : new() 

    {
        SuperBobEntities db = new SuperBobEntities();

        public virtual ReactListResultModel<ViewListModel> GetListData()
        {
            ReactListResultModel<ViewListModel> model = new ReactListResultModel<ViewListModel>();
            List<ReactPopupModel> propertyList = new List<ReactPopupModel>();

            var classData = typeof(ViewListModel).GetProperties();
            foreach (var currentProperty in classData)
            {
                ReactPopupModel newProp = new ReactPopupModel();
                string displayName;

                if (currentProperty.GetCustomAttribute<DisplayAttribute>() != null)
                {
                    displayName = currentProperty.GetCustomAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    displayName = currentProperty.Name;
                }

                newProp.PropertyName = currentProperty.Name;
                newProp.DisplayName = displayName;
                if (currentProperty.PropertyType.Name == "List`1")
                {
                    newProp.DisplayProperty = false;
                }
                else
                {
                    newProp.DisplayProperty = true;
                }

                // do not display Id in view list
                if (currentProperty.Name == "Id")
                {
                    newProp.DisplayProperty = false;
                }

                if (currentProperty.Name.Contains("Id"))
                {
                    newProp.DisplayProperty = false;
                }

                propertyList.Add(newProp);
            }
            model.PropertyNames = propertyList;

           var modelData = db.Set<T>().ToList(); ;
            

            List<ViewListModel> listModel = new List<ViewListModel>();
            var ViewModelProps = typeof(ViewListModel).GetProperties();


            foreach (var currentRow in modelData)
            {
                var currentRowProps = currentRow.GetType().GetProperties();
                var addModel = new ViewListModel();



                foreach (var modelProperty in currentRowProps)
                {
                    foreach (var otherProperty in ViewModelProps)
                    {
                        if (modelProperty.Name == otherProperty.Name)
                        { 
                                    otherProperty.SetValue(addModel, modelProperty.GetValue(currentRow));                                  
                            }
                        }                
                }
                listModel.Add(addModel);
            }
            model.DataList = listModel;

            return model;

        }

        public virtual ActionResult DisplayList()
        {
            var model = GetListData();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult EditData(int id)
        {
            var createModel = CreateEditDataModel(id);
            return Json(createModel, JsonRequestBehavior.AllowGet);
        }
       
        public virtual ReactCreateEditDataModel<EditViewModel> CreateEditDataModel(int id)
        {
            // map table class model to popup create/edit model
            ReactCreateEditDataModel<EditViewModel> createModel = new ReactCreateEditDataModel<EditViewModel>();

            var ViewModelProps = typeof(EditViewModel).GetProperties();
            List<ReactPopupModel> popupDisplayModel = new List<ReactPopupModel>();

            foreach (var classProperty in ViewModelProps)
            {
                var propType = classProperty.PropertyType.Name;
                string displayName;

                if (classProperty.GetCustomAttribute<DisplayAttribute>() != null)
                {
                    displayName = classProperty.GetCustomAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    displayName = classProperty.Name;
                }
                switch (propType)
                {
                    case "List`1":          // is list

                        popupDisplayModel.Add(new ReactPopupModel
                        {
                            DisplayName = displayName,
                            PropertyName = classProperty.Name,
                            TagType = 3,
                            DisplayProperty = false
                        });

                        break;

                    case "Int32":
                        if (classProperty.Name == "Id")
                        {
                            popupDisplayModel.Add(new ReactPopupModel
                            {
                                DisplayName = displayName,
                                PropertyName = classProperty.Name,
                                TagType = 0,
                                DisplayProperty = false
                            });
                        }
                        else
                        {
                            if (classProperty.Name.Length > 2 && classProperty.Name.Contains("Id"))
                            {
                                var listName = classProperty.Name.Remove(classProperty.Name.Length - 2) + "List";
                                
                                popupDisplayModel.Add(new ReactPopupModel
                                {
                                    DisplayName = displayName,
                                    PropertyName = classProperty.Name,
                                    TagType = 2,
                                    DropDownListName = listName,
                                    DisplayProperty = true
                                });
                            }
                            else
                            {
                                popupDisplayModel.Add(new ReactPopupModel
                                {
                                    DisplayName = displayName,
                                    PropertyName = classProperty.Name,
                                    TagType = 1,
                                    DisplayProperty = true
                                });
                            }

                        }
                        break;

                    case "DateTime":
                        popupDisplayModel.Add(new ReactPopupModel
                        {
                            DisplayName = displayName,
                            PropertyName = classProperty.Name,
                            TagType = 4,
                            DisplayProperty = true
                        });
                        break;
                    default:                // is standard so pass as input type


                        popupDisplayModel.Add(new ReactPopupModel
                        {
                            DisplayName = displayName,
                            PropertyName = classProperty.Name,
                            TagType = 1,
                            DisplayProperty = true
                        });
                        break;
                }


            }

            if (id != 0)
            {
                try
                {
                    var editModel = db.Set<T>().Find(id);                    
                    var TableProps = typeof(T).GetProperties();

                    EditViewModel model = new EditViewModel();

                    foreach (var modelProperty in TableProps)
                    {
                        foreach (var otherProperty in ViewModelProps)
                        {
                            if (modelProperty.Name == otherProperty.Name)
                            {
                                otherProperty.SetValue(model, modelProperty.GetValue(editModel));
                            }
                        }
                    }
                    createModel.Success = true;
                    createModel.Model = model;

                    createModel.PopupDisplayModel = popupDisplayModel;
                    return createModel;
                }
                catch
                {
                    createModel.Success = false;
                    return createModel;
                }
            }
            else
            {
                EditViewModel newModel = new EditViewModel();
                foreach (var otherProperty in ViewModelProps)
                {
                    if (otherProperty.PropertyType.Name == "String")
                    {
                        otherProperty.SetValue(newModel, "");
                    }
                }
                createModel.Success = true;
                createModel.Model = newModel;
                createModel.PopupDisplayModel = popupDisplayModel;
                return createModel;
            }
        }

        public virtual T MapPopupModelCreateTableModel(EditViewModel model)
        {
            // map popup model to new table class model
            T saveModel = new T();
            var ViewModelProps = typeof(EditViewModel).GetProperties();
            try
            {
                var tableProps = typeof(T).GetProperties();

                foreach (var modelProperty in tableProps)
                {
                    foreach (var otherProperty in ViewModelProps)
                    {
                        if (modelProperty.Name == otherProperty.Name && modelProperty.Name != "Id")
                        {
                            modelProperty.SetValue(saveModel, otherProperty.GetValue(model));
                        }
                    }
                }
                return saveModel;

            }
            catch (Exception e)
            {
                return saveModel;
            }
        }

        public virtual T MapPopupModelUpdateTableModel(EditViewModel model, T updateModel)
        {
            // map data from popup model to table class update model
            try
            {
                var modelProperties = model.GetType().GetProperties();               
                var ViewModelProps = typeof(EditViewModel).GetProperties();              
                var updateTableProps = updateModel.GetType().GetProperties();

                foreach (var modelProperty in updateTableProps)
                {
                    foreach (var otherProperty in ViewModelProps)
                    {
                        if (modelProperty.Name == otherProperty.Name)
                        {
                            modelProperty.SetValue(updateModel, otherProperty.GetValue(model));
                            if (modelProperty.PropertyType == typeof(System.String))
                            {
                                string clean = (string)modelProperty.GetValue(updateModel);
                                modelProperty.SetValue(updateModel, clean.Trim());
                            }
                        }
                    }
                }
                return updateModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetEditId(EditViewModel model )
        {
            var modelProperties = model.GetType().GetProperties();
            var modelId = modelProperties.Where(x => x.Name == "Id").ToList().FirstOrDefault().GetValue(model).ToString();
            return Convert.ToInt32(modelId);
        }
      
        public int GetNewRecordId(T saveResult)
        {
            // get id of record just added
            var saveResultProperties = saveResult.GetType().GetProperties();
            var addId = saveResultProperties.Where(x => x.Name == "Id").ToList().FirstOrDefault().GetValue(saveResult).ToString();
            return Convert.ToInt32(addId);
        }

        public virtual PopupSaveResultModel SaveData(EditViewModel model)
        {
            // Save Data to Database
            PopupSaveResultModel resultObject  = new PopupSaveResultModel();
            try
            {                
                var id = GetEditId(model);              

                if (id != 0)
                {
                    // get record to update
                    var updateModel = db.Set<T>().Find(id);

                    // map data from popup model to table model
                    MapPopupModelUpdateTableModel(model,updateModel);
                    resultObject.Id = Convert.ToInt32(id);
                    resultObject.AddRecord = false;
                    resultObject.Success = true;
                   
                    db.SaveChanges();
                   
                }
                else
                {
                    // create a new table class object for new record
                    var saveModel = MapPopupModelCreateTableModel(model);
                    
                    var saveResult = db.Set<T>().Add(saveModel);

                    db.SaveChanges();                    
                    resultObject.Id = Convert.ToInt32(GetNewRecordId(saveResult));
                    resultObject.AddRecord = true;
                    resultObject.Success = true;
                    
                }
            }
            catch (Exception e)
            {

                return new PopupSaveResultModel { Id = 0, AddRecord = true, Success = false };
            }

            return resultObject;
        }


        public virtual List<PopupSaveListData> MapToDisplayListModel(EditViewModel model)
        {
            List<PopupSaveListData> displayListModelList = new List<PopupSaveListData>();
            var modelProperties = model.GetType().GetProperties();
            var displayListProps = typeof(ViewListModel).GetProperties();

            foreach (var mprop in modelProperties)
            {
                var value = mprop.GetValue(model);
                if (mprop.PropertyType.Name != "List`1")
                {
                    foreach (var displayProp in displayListProps)
                    {
                        if (mprop.Name == displayProp.Name)
                        {
                            displayListModelList.Add(new PopupSaveListData
                            {
                                PropertyName = mprop.Name,
                                PropertyValue = mprop.GetValue(model).ToString()
                            });
                        }
                    }
                }
            }
            return displayListModelList;
        }
        public virtual ActionResult SaveCurrentData(EditViewModel model)
        {
            
            var result = SaveData(model);
            var displayListData = MapToDisplayListModel(model);
            result.AddDisplayListData = displayListData;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Delete(int id)
        {
            try
            {
                var deleteRecord = db.Set<T>().Find(id);               
                db.Set<T>().Remove(deleteRecord);
                db.SaveChanges();
                               
                return Json(new PopupDeleteResultModel { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch ( Exception e)
            {
                return Json(new PopupDeleteResultModel { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public void SetPropertyListHidden(string propertyName, ReactListResultModel<ViewListModel> model)
        {
            var ClassProperties = typeof(ViewListModel).GetProperties();
            var classPropertyData = model.PropertyNames.Where(x => x.PropertyName == propertyName).ToList();
            classPropertyData.FirstOrDefault().DisplayProperty = false;                            
            
        }

        public void SetPropertyModalHidden(string propertyName, ReactCreateEditDataModel<EditViewModel> model)
        {
            //var modelProperties = model.PopupDisplayModel.GetType().GetProperties();
            foreach( var p in model.PopupDisplayModel)
            {
                if ( p.PropertyName == propertyName )
                {
                    p.DisplayProperty = false;
                }
            }
        }

        public void SetEditPropertyTagType(List<ReactPopupModel> model, string propertyName)
        {
            foreach( var m in model)
            {
                if ( m.PropertyName == propertyName)
                {
                    m.TagType = 15;
                }
            }
        }

        public void BuildAddModel(AddViewModel model)
        {
            ReactListResultModel<ViewListModel> returnModel = new ReactListResultModel<ViewListModel>();
            
        }
    }
}