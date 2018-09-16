using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Repository;
using SuperBob.Service.Contract;
using SuperBob.Model;
using SuperBob.Service.Models;

namespace SuperBob.Service
{
    public class PlayListVideoService : IPlayListVideoService
    {
        private IRepository<PlayListVideo> _playListVideoRepository;
        private IRepository<VideoLibrary> _videoLibraryRepository;
        private IRepository<Genre> _genreRepository;

        public PlayListVideoService(IRepository<PlayListVideo> playListVideoRepository, IRepository<VideoLibrary> videoLibraryRepository,
            IRepository<Genre> genreRepository)
        {
            _playListVideoRepository = playListVideoRepository;
            _videoLibraryRepository = videoLibraryRepository;
            _genreRepository = genreRepository;
        }

        public PLayListVideoAddVideo AddPlayListVideo(PlayListVideo model)
        {
            PLayListVideoAddVideo rModel = new PLayListVideoAddVideo();
            _playListVideoRepository.Add(model);
            var videoData = _videoLibraryRepository.GetById(model.VideoLibraryId);
            PlayListVideoPlayListModel videoDataModel = new PlayListVideoPlayListModel();
            videoDataModel.FileName = videoData.FileName.Trim();
            videoDataModel.Title = videoData.Name.Trim();
            videoDataModel.Gerne = _genreRepository.GetById(videoData.GenreId).Name.Trim();
            videoDataModel.Id = videoData.Id;
            videoDataModel.GerneId = videoData.GenreId;
            rModel.AddedVideo = videoDataModel;
            rModel.Success = true;
            rModel.Update = false;
            rModel.Id = model.Id;
            return rModel;
        }

        public SharedServiceAjaxModel DeletePLayListVideo(int id)
        {
            SharedServiceAjaxModel rModel = new SharedServiceAjaxModel();
            var deleteThis = _playListVideoRepository.Get(x => x.VideoLibraryId == id).FirstOrDefault().Id;
            _playListVideoRepository.Delete(deleteThis);
            rModel.Success = true;
            rModel.Update = false;
            rModel.Id = id;
            return rModel;
        }

        public IEnumerable<PlayListVideo> GetPlayList(int playlist)
        {
            var data = _playListVideoRepository.Get(x => x.PlayListId == playlist).ToList();
            return data;
        }

        public PlayListVideoGetVideoModel GetPlayListVideos(int playListId)
        {
            PlayListVideoGetVideoModel rModel = new PlayListVideoGetVideoModel();
            var data = _playListVideoRepository.Get(x => x.PlayListId == playListId).ToList();
            List<PlayListVideoPlayListModel> libraryDataList = new List<PlayListVideoPlayListModel>();
            foreach( var row in data)
            {
                PlayListVideoPlayListModel addModel = new PlayListVideoPlayListModel();
                var libraryData = _videoLibraryRepository.GetById(row.VideoLibraryId);
                addModel.FileName = libraryData.FileName;
                addModel.Gerne = _genreRepository.GetById(libraryData.GenreId).Name;
                addModel.Title = libraryData.Name;
                addModel.Id = libraryData.Id;
                libraryDataList.Add(addModel);
            }
            rModel.VideoList = libraryDataList;
            rModel.Success = true;
            rModel.Update = false;
            return rModel;
        }
    }
}
