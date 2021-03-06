﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Models;

namespace SuperBob.Service.Contract
{
    public interface IVideoLibraryService
    {
        IEnumerable<VideoLibrary> GetVideoLibrary();
        SharedServiceAjaxModel AddVideoLibrary(VideoLibrary model);
    }
}
