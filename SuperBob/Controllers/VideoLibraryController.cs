﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Model;
using SuperBob.Models;
using SuperBob.Service;
using SuperBob.Service.Contract;

namespace SuperBob.Controllers
{
    public class VideoLibraryController : ReactBaseController<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel>
    {
        // GET: Video
        private IVideoLibraryService _videoLibraryService;
        private IGenreService _genreService;

        public VideoLibraryController(IVideoLibraryService videoLibraryService,
            IGenreService genreService)
        {
            _videoLibraryService = videoLibraryService;
            _genreService = genreService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult EditData(int id)
        {
            var createModel = CreateEditDataModel(id);

            var genreInputList = _genreService.GetGenreList();

            var genreList = genreInputList.Select(x => new ReactDropDownModel { Text = x.Name, Value = x.Id }).ToList();
            createModel.Model.GenreList = genreList;
            return Json(createModel, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult DisplayList()
        {
            var model = GetListData();
            foreach( var m in model.DataList)
            {
                m.GenreType = _genreService.GetById(m.GenreId).Name.Trim();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult SaveCurrentData(VideoLibraryEditViewModel model)
        {

            var result = SaveData(model);
            var displayListData = MapToDisplayListModel(model);
            displayListData.Add(new PopupSaveListData
            {
                PropertyName = "GenreType",
                PropertyValue = _genreService.GetById(model.GenreId).Name
            });
            result.AddDisplayListData = displayListData;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}