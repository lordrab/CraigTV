using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Contract;
using SuperBob.Repository;
using SuperBob.Service.Models;

namespace SuperBob.Service
{
    public class VideoLibraryService : IVideoLibraryService
    {
        private IRepository<VideoLibrary> _videoLibraryRepository;
        private IErrorLogService _errorLogService;

        public VideoLibraryService(IRepository<VideoLibrary> videoLibraryRepository, IErrorLogService errorLogService)
        {
            _videoLibraryRepository = videoLibraryRepository;
            _errorLogService = errorLogService;
        }

        public SharedServiceAjaxModel AddVideoLibrary(VideoLibrary model)
        {
            SharedServiceAjaxModel rModel = new SharedServiceAjaxModel();
            try
            {
                if ( model.Id == 0)
                {
                    _videoLibraryRepository.Add(model);
                    rModel.Success = true;
                    rModel.Update = false;
                    rModel.Id = model.Id;
                    return rModel;
                }
                else
                {
                    var data = _videoLibraryRepository.GetById(model.Id);
                    data.Name = model.Name;
                    data.GenreId = model.GenreId;
                    data.CatagoryId = model.CatagoryId;
                    data.Description = data.Description;
                    _videoLibraryRepository.Update(data);

                    rModel.Success = true;
                    rModel.Update = true;
                    rModel.Id = model.Id;
                    return rModel;
                }
            }
            catch(Exception error)
            {
                _errorLogService.AddErrorLog(new Models.ErrorLogServiceModel
                {
                    Error = error,
                    Location = "VideoLibraryService",
                    Method = "AddVideoLibrary",
                    OtherInfo = ""
                });
                rModel.Success = false;
                rModel.Update = false;
                rModel.Id = model.Id;
                return rModel;
            }
            
        }

        public IEnumerable<VideoLibrary> GetVideoLibrary()
        {                     
            return _videoLibraryRepository.GetAll();
        }


    }
}
