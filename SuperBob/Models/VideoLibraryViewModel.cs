using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SuperBob.Service;
using SuperBob.Service.Models;

namespace SuperBob.Models
{
    public class GerneListModel
    {
        public int GenreId { get; set; }
        public string GenreType { get; set; }
    }

    public class VideoLibraryListViewModel : GerneListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CatagoryId { get; set; }
        public string CatagoryName { get; set; }
    }

    public class VideoLibraryEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Customer Name")]
        public int GenreId { get; set; }
        public List<ReactDropDownModel> GenreList { get; set; }
        public string FileName { get; set; }
    }

    public class VideoLibraryAddModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string VideoName { get; set; }
        public int GenreType { get; set; }
        public int CatagoryName { get; set; }
        public string VideoDescription { get; set; }
    }

    public class VideoLibraryEditDropdownModel
    {
        public List<GerneListModel> GenreList { get; set; }
        public List<CatagoryServiceListModel> CatagoryList { get; set; }

    }
}