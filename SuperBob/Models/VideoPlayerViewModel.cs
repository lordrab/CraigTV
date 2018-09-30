using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SuperBob.Service;

namespace SuperBob.Models
{
    public class VideoPlayerListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class VideoPlayerEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Video Name")]
        public string Name { get; set; }
        public int LoginId { get; set; }
       
    }

    public class VideoPLayerVideoListModel
    {
        public int Id { get; set; }
        public int PlayListId { get; set; }
        public int VideoLibraryId { get; set; }
        public string VideoName { get; set; }
    }
    
    public class VideoPLayerVideoEditModel
    {
        public int Id { get; set; }
        public int PlayListId { get; set; }
        //public List<ReactDropDownModel> PlayList { get; set; }
        public int VideoLibraryId { get; set; }
        public List<ReactDropDownModel> VideoLibrary { get; set; }
    }

    public class VideoPlayerListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Gerne { get; set; }
        public string FileName { get; set; }
        public int GerneId { get; set; }
    }

    public class VideoPlayerPLayLists
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class VideoPLayerAddLibrary
    {
        public int PlayListId { get; set; }
        public int LibraryId { get; set; }
    }
}