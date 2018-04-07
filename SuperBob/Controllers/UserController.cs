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

        
        
    }
   
}