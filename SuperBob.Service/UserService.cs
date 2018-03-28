using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBob.Model;
using SuperBob.Service.Contract;
using SuperBob.Repository;


namespace SuperBob.Service
{
    public class UserService : IUserService
    {
        private IRepository<Person> _personRepository;

        public UserService(IRepository<Person> personRepository)
            {
            _personRepository = personRepository;
            }

        public Person AddUser(Person user)
        {
            return _personRepository.Add(user);
        }

        public IEnumerable<Person> GetUser()
        {
            return _personRepository.GetAll();
        }

        public Person GetUserById(int id)
        {
            return _personRepository.GetById(id);
        }

        public void UpdateUser(Person model)
        {
             _personRepository.Update(model);
        }
    }
}
