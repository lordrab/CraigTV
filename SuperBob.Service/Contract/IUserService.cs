using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;


namespace SuperBob.Service.Contract
{
    public interface IUserService
    {
        IEnumerable<Person> GetUser();
        Person GetUserById(int id);
        Person AddUser(Person question);
        void UpdateUser(Person model);
    }
}
