using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service;

namespace SuperBob.Service.Contract
{
    public interface IReactFrameWorkService<Table,ViewListModel,EditViewModel> where EditViewModel : class, new() where ViewListModel : class, new()
        where Table : class, new()
    {
        ReactCreateEditDataModel<EditViewModel> CreateEditDataModel(int id);
        ReactListResultModel<ViewListModel> GetListData(int skip, int number);
    }
}
