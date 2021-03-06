﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBob.Model;
using SuperBob.Models;
using SuperBob.Service;
using SuperBob.Service.Contract;
// required for upload method
using System.Net.Http;
using System.IO;
using System.Net;
// *****


namespace SuperBob.Controllers
{
    public class VideoLibraryController : ReactBaseController<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel>
    {
        // GET: Video
        private IVideoLibraryService _videoLibraryService;
        private IGenreService _genreService;
        private ICatagoryService _catagoryService;
        private IReactFrameWorkService<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel> _reactFrameWorkService;

        public VideoLibraryController(IVideoLibraryService videoLibraryService,
            IGenreService genreService, ICatagoryService catagoryService,
            IReactFrameWorkService<VideoLibrary, VideoLibraryListViewModel, VideoLibraryEditViewModel> reactFrameWorkService)
        {
            _videoLibraryService = videoLibraryService;
            _genreService = genreService;
            _catagoryService = catagoryService;
            _reactFrameWorkService = reactFrameWorkService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult EditData(int id)
        {
            var createModel = CreateEditDataModel(id);
            SetEditPropertyTagType(createModel.PopupDisplayModel, "FileName");
            var genreInputList = _genreService.GetGenreList();

            var genreList = genreInputList.Select(x => new ReactDropDownModel { Text = x.Name, Value = x.Id }).ToList();
            createModel.Model.GenreList = genreList;
            return Json(createModel, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult DisplayList(DisplayDataListPostModel postModel)
        {
            //var model = GetListData(postModel);
            var model = _reactFrameWorkService.GetListData(postModel);
            foreach (var m in model.DataList)
            {
                m.GenreType = _genreService.GetById(m.GenreId).Name.Trim();
            }
            foreach(var m in model.DataList)
            {
                var catData = _catagoryService.GetById(m.CatagoryId);
                 
                if ( catData != null)
                {
                    m.CatagoryName = catData.Name;
                }
                
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

        public ActionResult Upload(Upload file)
        {
            var guid = Guid.NewGuid().ToString();
            if (file.fileData == null)
            {
                return Json(new { success = false, writeSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var bytes = Convert.FromBase64String(file.fileData);
                // var savedFile = @"c:/upload/" + file.fileName;
                var imagesDir = System.Web.HttpContext.Current.Server.MapPath("~/Videos/");
                var savedFile = imagesDir + file.fileName.Replace(" ", "");

                if (file.EndOfFile)
                {
                    var hlkj = 0;
                }

                if (System.IO.File.Exists(savedFile) == false)
                {
                    using (var currentFile = new FileStream(savedFile, FileMode.Create))
                    {
                        currentFile.Dispose();
                    }
                }

                if (System.IO.File.Exists(savedFile))
                {

                    var addToFile = new FileStream(savedFile, FileMode.Append);
                    addToFile.Write(bytes, 0, bytes.Length);
                    addToFile.Flush();
                    addToFile.Dispose();
                }


                return Json(new { success = true, writeSuccess = true}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetGenreList()
        {
            List<GerneListModel> model = new List<GerneListModel>();
            var gerneList = _genreService.GetGenreList();
            foreach (var item in gerneList)
            {
                model.Add(new GerneListModel
                {
                    GenreId = item.Id,
                    GenreType = item.Name
                });
            }
            var catagoryList = _catagoryService.GetCatagoryList();
            VideoLibraryEditDropdownModel rModel = new VideoLibraryEditDropdownModel();
            rModel.GenreList = model;
            rModel.CatagoryList = catagoryList;
            return Json(rModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditVideoLibrary(VideoLibraryAddModel model)
        {
            try
            {
                VideoLibrary addModel = new VideoLibrary()
                {
                    Id = model.Id,
                    FileName = model.FileName.Replace(" ", ""),
                    Name = model.VideoName,
                    Description = model.VideoDescription,
                    GenreId = model.GenreType,
                    CatagoryId = model.CatagoryName
                };
                _videoLibraryService.AddVideoLibrary(addModel);
                VideoLibraryListViewModel addToListModel = new VideoLibraryListViewModel()
                {
                    Id = addModel.Id,
                    GenreType = addModel.GenreId.ToString(),
                    Name = addModel.Name
                };
                return Json(new { success = true, displayListModel = addToListModel });
            }
            catch
            {
                return Json(new { success = false });
            }



        }
        
    }
}
