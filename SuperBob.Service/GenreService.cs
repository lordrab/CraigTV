using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Contract;
using SuperBob.Model;
using SuperBob.Repository;

namespace SuperBob.Service
{
    public class GenreService : IGenreService
    {
        private IRepository<Genre> _genreRepository;

        public GenreService( IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public IEnumerable<Genre> GetGenreList()
        {
            return _genreRepository.GetAll();
        }
    }
}
