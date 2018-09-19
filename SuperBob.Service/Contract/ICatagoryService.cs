using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Models;
using SuperBob.Model;

namespace SuperBob.Service.Contract
{
    public interface ICatagoryService
    {
        SharedServiceAjaxModel AddCatagory(Catagory model);
        List<CatagoryServiceListModel> GetCatagoryList();
        Catagory GetById(int id);
    }
}
