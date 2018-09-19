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
    public class CatagoryController : ReactBaseController<Catagory, CatagoryListViewModel, CatagoryListViewModel>
    {
        // GET: User

        private ICatagoryService _catagoryService;

        public CatagoryController(ICatagoryService catagoryService)
        {
            _catagoryService = catagoryService;
        }
        public ActionResult Index()
        {
            return View();
        }

        
        
    }
   
}