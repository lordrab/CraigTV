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
        private IVideoLibraryService _videoLibraryService;
        private IReactFrameWorkService<PlayListVideo, VideoPLayerVideoListModel, VideoPLayerVideoEditModel> _reactFrameWorkService;

        public VideoPlayerController(IPlayListService  playListService,
            IPlayListVideoService playListVideoService, IVideoLibraryService videoLibraryService,
            IReactFrameWorkService<PlayListVideo, VideoPLayerVideoListModel, VideoPLayerVideoEditModel> reactFrameWorkService)
        {
            _playListService = playListService;
            _playListVideoService = playListVideoService;
            _videoLibraryService = videoLibraryService;
            _reactFrameWorkService = reactFrameWorkService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVideoPLayList(int id)
        {
            var playListVideoList = _playListVideoService.GetPlayList(id);
            var libraryList = _videoLibraryService.GetVideoLibrary().ToList();
            var model = _reactFrameWorkService.GetListData();
            

            List<VideoPLayerVideoListModel> videoPlayList = new List<VideoPLayerVideoListModel>();
            foreach (var row in playListVideoList)
            {
                var videoName = libraryList.Where(x => x.Id == row.VideoLibraryId).FirstOrDefault();
                videoPlayList.Add(new VideoPLayerVideoListModel
                {
                    Id = row.Id,
                    PlayListId = id,
                    VideoLibraryId = videoName.Id,
                    VideoName = videoName.Name
                });
            }

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public  ActionResult EditPlayListData(int id)
        {
            // using manually created service to replace method in this abstract class
            var createModel = _reactFrameWorkService.CreateEditDataModel(id);
            var libraryList = _videoLibraryService.GetVideoLibrary().ToList();
            List<ReactDropDownModel> addModel = new List<ReactDropDownModel>();

            foreach ( var row in libraryList )
            {
                addModel.Add(new ReactDropDownModel
                {
                    Text = row.Name,
                    Value = row.Id
                });
            }
            createModel.Model.VideoLibrary = addModel;
            return Json(createModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPlayList(VideoPlayerListViewModel model)
        {
            PlayList addModel = new PlayList()
            {
                Name = model.Name,
                LoginId = Convert.ToInt32(User.Identity.GetUserId())

            };

            if (model.Id == 0)
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