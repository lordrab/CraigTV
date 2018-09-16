using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Models;


namespace SuperBob.Service.Contract
{
    public interface IPlayListVideoService
    {
        IEnumerable<PlayListVideo> GetPlayList(int playlist);
        PLayListVideoAddVideo AddPlayListVideo(PlayListVideo model);
        PlayListVideoGetVideoModel GetPlayListVideos(int playListId);
        SharedServiceAjaxModel DeletePLayListVideo(int id);
    }
}
