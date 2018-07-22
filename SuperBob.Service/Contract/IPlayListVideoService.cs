using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;

namespace SuperBob.Service.Contract
{
    public interface IPlayListVideoService
    {
        IEnumerable<PlayListVideo> GetPlayList(int playlist);

    }
}
