using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Models;
using SuperBob.Model;
using SuperBob.Service.Contract;
using SuperBob.Repository;
using Microsoft.AspNet.Identity;

namespace SuperBob.Controllers
{
    [Authorize] 
    public class UserController : ReactBaseController<Person, PersonListViewModel, PersonListViewModel>
    {
        // GET: User

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult SaveData(PersonListViewModel model)
        {
            // Save Data to Database
            PopupSaveResultModel resultObject = new PopupSaveResultModel();
            try
            {
                var id = GetEditId(model);

                if (id != 0)
                {
                    // get record to update
                    var updateModel = _userService.GetUserById(id);

                    // map data from popup model to table model
                    MapPopupModelUpdateTableModel(model, updateModel);
                    resultObject.Id = Convert.ToInt32(id);
                    resultObject.AddRecord = false;
                    resultObject.Success = true;

                }
                else
                {
                    // create a new table class object for new record
                    var saveModel = MapPopupModelCreateTableModel(model);
                   
                    saveModel.Id = User.Identity.GetUserId<int>();
                    var saveResult = _userService.AddUser(saveModel);

                    resultObject.Id = User.Identity.GetUserId<int>();
                    resultObject.AddRecord = true;
                    resultObject.Success = true;

                }
            }
            catch (Exception e)
            {
                return Json(new PopupSaveResultModel { Id = 0, AddRecord = true, Success = false });
            }

            return Json(resultObject, JsonRequestBehavior.AllowGet);
        }
        
    }
   
}