using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Contract;
using SuperBob.Service.Models;
using SuperBob.Repository;


namespace SuperBob.Service
{
    public class CatagoryService : ICatagoryService
    {
        private IRepository<Catagory> _catagoryRepository;

        public CatagoryService(IRepository<Catagory> catagoryRepository)
        {
            _catagoryRepository = catagoryRepository;
        }
        public SharedServiceAjaxModel AddCatagory(Catagory model)
        {
            SharedServiceAjaxModel rModel = new SharedServiceAjaxModel();
            _catagoryRepository.Add(model);
            rModel.Success = true;
            rModel.Update = false;
            rModel.Id = model.Id;
            return rModel;
        }

        public Catagory GetById(int id)
        {
            return _catagoryRepository.GetById(id);
        }

        public List<CatagoryServiceListModel> GetCatagoryList()
        {
            var data = _catagoryRepository.GetAll().Select(x => new CatagoryServiceListModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return data;
        }

        
    }
}
