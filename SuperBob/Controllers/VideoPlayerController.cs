using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Models;
using SuperBob.Model;
using SuperBob.Service;
using SuperBob.Service.Contract;
using System.Web.Security;
using Microsoft.AspNet.Identity;


namespace SuperBob.Controllers
{
    public class VideoPlayerController : ReactBaseController<PlayList, VideoPlayerListViewModel, VideoPlayerEditViewModel>
    {
        private IPlayListService _playListService;
        private IPlayListVideoService _playListVideoService;

        public VideoPlayerController(IPlayListService playListService, IPlayListVideoService playListVideoService)
        {
            _playListService = playListService;
            _playListVideoService = playListVideoService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVideoPLayList(int id)
        {
            var playListVideoList = _playListVideoService.GetPlayList(id);
            return Json(new { success = true });

        }

        public ActionResult AddPlayList(VideoPlayerListViewModel model)
        {
            PlayList addModel = new PlayList()
            {
                Name = model.Name,
                LoginId = Convert.ToInt32(User.Identity.GetUserId())

            };

            if ( model.Id == 0 )
            {               
                var result = _playListService.AddPlayList(addModel);
                return Json(new { id = result });
            }
            else
            {
                addModel.Id = model.Id;
                var result = _playListService.UpdatePLayList(addModel);
                return Json(new { id = addModel.Id });
            }
            
            
        }

        public override ActionResult SaveCurrentData(VideoPlayerEditViewModel model)
        {

            model.LoginId = Convert.ToInt32(User.Identity.GetUserId());
            var result = SaveData(model);
            var displayListData = MapToDisplayListModel(model);
            result.AddDisplayListData = displayListData;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}