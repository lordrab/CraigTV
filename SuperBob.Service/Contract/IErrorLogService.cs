using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Models;

namespace SuperBob.Service.Contract
{
    public interface IErrorLogService
    {
        void AddErrorLog(ErrorLogServiceModel model);
    }
}
