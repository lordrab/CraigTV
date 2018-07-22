using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Repository;
using SuperBob.Service.Contract;
using SuperBob.Model;

namespace SuperBob.Service
{
    public class PlayListVideoService : IPlayListVideoService
    {
        private IRepository<PlayListVideo> _playListVideoRepository;

        public PlayListVideoService(IRepository<PlayListVideo> playListVideoRepository)
        {
            _playListVideoRepository = playListVideoRepository;
        }
        public IEnumerable<PlayListVideo> GetPlayList(int playlist)
        {
            return _playListVideoRepository.GetAll();
        }
    }
}
