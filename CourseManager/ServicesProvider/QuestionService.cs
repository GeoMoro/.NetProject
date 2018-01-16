using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesProvider
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _qrepository;

        private readonly IAnswerRepository _arepository;

        public QuestionService(IQuestionRepository qrepository, IAnswerRepository arepository)
        {
            _qrepository = qrepository;

            _arepository = arepository;

        }

        public IList<Answer> GetAllAnswersForQuestion(Guid id)
        {
            //return _databaseService.Answers.Where(answer => answer.QuestionId == id).ToList();
            return _qrepository.GetAllAnswersForQuestion(id);
        }

        public IReadOnlyList<Question> GetAllQuestions()
        {
            //return _databaseService.Questions.ToList();
            return _qrepository.GetAllQuestions();

        }

        public Question GetQuestionById(Guid id)
        {
            //return _databaseService.Questions.SingleOrDefault(answer => answer.Id == id);
            return _qrepository.GetQuestionById(id);
        }

        public void CreateQuestion(Question question)
        {
            /* _databaseService.Questions.Add(question);

             _databaseService.SaveChanges();*/

            _qrepository.CreateQuestion(question);

        }

        public void EditQuestion(Question question)
        {
            /*_databaseService.Questions.Update(question);

            _databaseService.SaveChanges();*/
            _qrepository.EditQuestion(question);
        }

        public void DeleteQuestion(Question question)
        {
            /*_databaseService.Questions.Remove(question);

            _databaseService.SaveChanges();*/

            _qrepository.DeleteQuestion(question);

        }
    }
}
