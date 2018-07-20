using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Contract;
using SuperBob.Repository;

namespace SuperBob.Service
{
    public class VideoLibraryService : IVideoLibraryService
    {
        private IRepository<VideoLibrary> _videoLibraryRepository;

        public VideoLibraryService(IRepository<VideoLibrary> videoLibraryRepository)
        {
            _videoLibraryRepository = videoLibraryRepository;
        }

        public bool AddVideoLibrary(VideoLibrary model)
        {
            try
            {
                _videoLibraryRepository.Add(model);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public IEnumerable<VideoLibrary> GetVideoLibrary()
        {             
            return _videoLibraryRepository.GetAll();
        }


    }
}
