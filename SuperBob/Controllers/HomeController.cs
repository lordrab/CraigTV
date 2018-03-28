using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Repository;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using SuperBob.Service;
using SuperBob.Service.Contract;
using SuperBob.Models;
using SuperBob.Model;
using Microsoft.AspNet.Identity;


namespace SuperBob.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            
            var bob = User.Identity.GetUserId();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var bob = User.Identity.GetUserId();
            return View();
        }

        //public ActionResult GetQuestionList()
        //{
        //    var questionList = _questionService.GetQuestion().Select(x => new QuestionModel
        //    {
        //        QuestionId = x.QuestionId,
        //        QuestionBody = x.QuestionBody,
        //        Title = x.Title.Trim(),
        //        Posted = x.Posted
        //    }).ToList();

            
        //    return Json(questionList, JsonRequestBehavior.AllowGet);
        //}
        
    }
}