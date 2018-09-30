using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Service.Contract;
using SuperBob.Repository;
using SuperBob.Model;
using SuperBob.Service.Models;

namespace SuperBob.Service
{
    public class ErrorLogService : IErrorLogService
    {
        private IRepository<ErrorLog> _errorLogRepository;

        public ErrorLogService(IRepository<ErrorLog> errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        public void AddErrorLog(ErrorLogServiceModel model)
        {
            ErrorLog addModel = new ErrorLog()
            {
                Location = model.Location,
                Error = model.Error.Message.ToString(),
                Method = model.Method,
                OtherInfo = model.OtherInfo,
                 ErrorDate = DateTime.Now
            };
            _errorLogRepository.Add(addModel);
            
        }
    }
}
