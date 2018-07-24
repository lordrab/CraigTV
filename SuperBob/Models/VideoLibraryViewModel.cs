using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SuperBob.Service;

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
        public string fileName { get; set; }
        public string videoName { get; set; }
        public int genreType { get; set; }
        public string videoDescription { get; set; }
    }
}