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
    public class QuestionService : IQuestionService
    {
        private IRepository<Question> _questionRepository;

        public QuestionService(IRepository<Question> questionRepository)
            {
            _questionRepository = questionRepository;
            }

        public Question AddQuestion(Question question)
        {
            return _questionRepository.Add(question);
        }

        public IEnumerable<Question> GetQuestion()
        {
            return _questionRepository.GetAll();
        }

        public Question GetQuestionById(int id)
        {
            return _questionRepository.GetById(id);
        }
    }
}
