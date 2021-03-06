﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;

namespace SuperBob.Service.Contract
{
    public  interface IGenreService
    {
        IEnumerable<Genre> GetGenreList();
        Genre GetById(int id);
    }
}
