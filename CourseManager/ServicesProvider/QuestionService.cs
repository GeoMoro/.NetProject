using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;

namespace ServicesProvider
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionsRepository;

        private readonly IAnswerRepository _answersRepository;

        public QuestionService(IQuestionRepository questionsRepository, IAnswerRepository answersRepository)
        {
            _questionsRepository  = questionsRepository;

            _answersRepository = answersRepository;
        }

        public void DeleteQuestion(Question question)
        {
            var answers = _answersRepository.GetAllAnswersForGivenUserId(question.Id);

            foreach (Answer answer in answers)
            {

                _answersRepository.DeleteAnswer(answer);
            }

            _questionsRepository.Delete(question);
            
        }
    }
}
