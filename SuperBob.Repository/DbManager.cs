using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperBob.Repository;


namespace SuperBob.DPInjection
{
    public class DbManager
    {
        private static IUnitOfWork _unitOfWork;

        public static void RegisterFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }


}
