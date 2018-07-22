using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
    
}