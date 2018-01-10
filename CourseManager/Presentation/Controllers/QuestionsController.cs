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
        private readonly IQuestionService _questionService;
        //private readonly IAnswerRepository _answerRepository; unused wtf, Juanito explain yourself

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: Questions
        public IActionResult Index()
        {
            _questionService.GetQuestionsWithAnswers();

            return View(_questionService.GetAllQuestions());
        }

        // GET: Questions/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _questionService.GetQuestionById(id.Value);

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

            _questionService.Create(questionCreateModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _questionService.GetQuestionById(id.Value);

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
            if (!ModelState.IsValid)
            {
                return View(questionEditModel);
            }

            var questionToBeEdited = _questionService.GetQuestionById(id);

            if (questionToBeEdited == null)
            {
                return NotFound();
            }

            try
            {
                _questionService.Edit(questionToBeEdited, questionEditModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(_questionService.GetQuestionById(id).Id))
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

            var question = _questionService.GetQuestionById(id.Value);

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
            var question = _questionService.GetQuestionById(id);

            _questionService.Delete(question);

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(Guid id)
        {
            return _questionService.VerifyIfQuestionExists(id);
        }
    }
}
