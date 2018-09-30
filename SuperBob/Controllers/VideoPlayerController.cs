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
    public class VideoPlayerController : ReactBaseController<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel>
    {
        private IPlayListService _playListService;
        private IPlayListVideoService _playListVideoService;
        private IVideoLibraryService _videoLibraryService;
        private IReactFrameWorkService<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel> _reactFrameWorkService;
        private IGenreService _genreService;
        private ICatagoryService _catagoryService;

        public VideoPlayerController(IPlayListService playListService,
            IPlayListVideoService playListVideoService, IVideoLibraryService videoLibraryService,
            IReactFrameWorkService<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel> reactFrameWorkService,
            IGenreService genreService, ICatagoryService catagoryService)
        {
            _playListService = playListService;
            _playListVideoService = playListVideoService;
            _videoLibraryService = videoLibraryService;
            _reactFrameWorkService = reactFrameWorkService;
            _genreService = genreService;
            _catagoryService = catagoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public override ActionResult DisplayList(DisplayDataListPostModel postModel)
        //{
        //    //var model = GetListData(postModel);
        //    var model = _reactFrameWorkService.GetListData(postModel);

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        public override ActionResult DisplayList(DisplayDataListPostModel postModel)
        {
            //var model = GetListData(postModel);
            var model = _reactFrameWorkService.GetListData(postModel);
            foreach (var m in model.DataList)
            {
                m.GenreType = _genreService.GetById(m.GenreId).Name.Trim();
            }
            foreach (var m in model.DataList)
            {
                var catData = _catagoryService.GetById(m.CatagoryId);

                if (catData != null)
                {
                    m.CatagoryName = catData.Name;
                }

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetVideoLibrary()
        {
            var bob = _videoLibraryService.GetVideoLibrary().Select(x => new VideoPlayerListModel
            {
                Id = x.Id,
                Title = x.Name.Trim(),
                Gerne = "",
                FileName = x.FileName.Trim(),
                GerneId = x.GenreId
            }).ToList();
            foreach (var row in bob)
            {
                row.Gerne = _genreService.GetById(row.GerneId).Name;
            }
            return Json(bob, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentVideoList(int playListId)
        {
            var data = _playListVideoService.GetPlayListVideos(playListId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPlayList(string playList)
        {
            PlayList addModel = new PlayList()
            {
                LoginId = User.Identity.GetUserId<int>(),
                Name = playList,
            };
            var rData = _playListService.AddPlayList(addModel);
            return Json(new { id = rData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPLayLists()
        {
            var rData = _playListService.GetPlayLists().Where(x => x.LoginId == User.Identity.GetUserId<int>()).Select(x => new VideoPlayerPLayLists
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return Json(rData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPlayListVideo(VideoPLayerAddLibrary model)
        {
            PlayListVideo addModel = new PlayListVideo()
            {
                PlayListId = model.PlayListId,
                VideoLibraryId = model.LibraryId
            };
            var rModel = _playListVideoService.AddPlayListVideo(addModel);
            return Json(rModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFromPlayList(int Id)
        {
            var rModel = _playListVideoService.DeletePLayListVideo(Id);
            return Json(rModel, JsonRequestBehavior.AllowGet);
        }
    }
}