using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Models;

namespace SuperBob.Service.Models
{
    public class PlayListVideoPlayListModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Gerne { get; set; }
        public string FileName { get; set; }
        public int GerneId { get; set; }
    }

    public class PLayListVideoAddVideo :  SharedServiceAjaxModel
    {
        public PlayListVideoPlayListModel AddedVideo { get; set; }
    }

    public class PlayListVideoGetVideoModel : SharedServiceAjaxModel
    {
        public List<PlayListVideoPlayListModel> VideoList { get; set; }
    }
}
