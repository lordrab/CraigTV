using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SuperBob.Models
{
    public class VideoLibraryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GenreId { get; set; }
        public string GenreType { get; set; }
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
    
    
}