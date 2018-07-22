using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Contract;
using SuperBob.Repository;

using SuperBob.Model;

namespace SuperBob.Service
{
    public class PlayListService : IPlayListService
    {
        private IRepository<PlayList> _playListRepository;

        public PlayListService(IRepository<PlayList> playListRepository)
        {
            _playListRepository = playListRepository;
        }

        public int AddPlayList(PlayList model)
        {
            try
            {
                var newId =_playListRepository.Add(model);
                return newId.Id;
            }
            catch
            {
                return 0;
            }
            
        }

        public IEnumerable<PlayList> GetPlayLists()
        {
            return _playListRepository.GetAll();

        }

        public int UpdatePLayList(PlayList model)
        {
            try
            {
                _playListRepository.Update(model);
                return model.Id;
            }
            catch
            {
                return 0;
            }
        }
    }
}
