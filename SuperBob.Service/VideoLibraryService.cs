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
                if ( model.Id == 0)
                {
                    _videoLibraryRepository.Add(model);
                    return true;
                }
                else
                {
                    var data = _videoLibraryRepository.GetById(model.Id);
                    data.Name = model.Name;
                    data.GenreId = model.GenreId;
                    data.CatagoryId = model.CatagoryId;
                    data.Description = data.Description;
                    _videoLibraryRepository.Update(data);

                    return true;
                }
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
