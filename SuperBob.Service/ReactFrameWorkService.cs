using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Contract;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using SuperBob.Model;
using System.Web.Mvc;


namespace SuperBob.Service
{
    public class ReactFrameWorkService<Table, ViewListModel, EditViewModel> : IReactFrameWorkService<Table,ViewListModel,EditViewModel> where EditViewModel : class, new()
        where ViewListModel : class, new() where Table : class, new()
    {
        SuperBobEntities db = new SuperBobEntities();

        public ReactCreateEditDataModel<EditViewModel> CreateEditDataModel(int id)
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
                    
                    var editModel = db.Set<EditViewModel>().Find(id);
                    var TableProps = typeof(EditViewModel).GetProperties();

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

        public ReactListResultModel<ViewListModel> GetListData()
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

            var modelData = db.Set<Table>().ToList(); ;


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




    }
}
