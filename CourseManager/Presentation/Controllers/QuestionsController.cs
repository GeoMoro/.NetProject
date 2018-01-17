using System;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _service;
        
        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }
        
        // GET: Questions
        public IActionResult Index()
        {
            foreach(var item in _service.GetAllQuestions())
            {
               item.Answers = _service.GetAllAnswersForQuestion(item.Id);
            }

            return View(_service.GetAllQuestions());
        }

        // GET: Questions/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _service.GetQuestionById(id.Value);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/AnswerList/5
        public IActionResult Answers(Guid? id)
        {
            return RedirectToAction("Index", "Answers", new { questionId = id });
        }

        // GET: Questions/Create
        public IActionResult Create(Guid? uid)
        {
            TempData["UId"] = uid;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid? uid, [Bind("UserId,CreatedDate,Topic,Text")] QuestionCreateModel questionCreateModel)
        {
            TempData["UId"] = uid;
            if (!ModelState.IsValid)
            {
                return View(questionCreateModel);
            }

            _service.CreateQuestion(questionCreateModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _service.GetQuestionById(id.Value);

            if (question == null)
            {
                return NotFound();
            }

            var questionEditModel = new QuestionEditModel(
                question.UserId,
                question.Topic,
                question.Text
            );

            return View(questionEditModel);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("UserId,CreatedDate,Topic,Text")] QuestionEditModel questionEditModel)
        {
            var questionToBeEdited = _service.GetQuestionById(id);

            if (questionToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(questionEditModel);
            }

            questionToBeEdited.UserId = questionEditModel.UserId;
            questionToBeEdited.CreatedDate = questionEditModel.CreatedDate;
            questionToBeEdited.Topic = questionEditModel.Topic;
            questionToBeEdited.Text = questionEditModel.Text;


            try
            {
                _service.EditQuestion(questionToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(_service.GetQuestionById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _service.GetQuestionById(id.Value);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        
        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var question = _service.GetQuestionById(id);

            _service.DeleteQuestion(question);

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(Guid id)
        {
            return _service.CheckIfQuestionExists(id);
        }
        
    }
}
