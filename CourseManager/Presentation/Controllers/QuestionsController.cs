using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _repository;

        public readonly IAnswerRepository _answerRepository;

        public QuestionsController(IQuestionRepository repository, IAnswerRepository answerRepository)
        {
            _repository = repository;

            _answerRepository = answerRepository;

        }
        
        // GET: Questions
        public IActionResult Index()
        {
            return View(_repository.GetAllQuestions());
        }
        
        // GET: Questions/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _repository.GetQuestionById(id.Value);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/AnswerList/5
        public IActionResult AnswerList(Guid? id)
        {
            ViewData["QuestionId"] = id.Value; //id.Value;
            return RedirectToAction("Index", "Answers", new{ QuestionId=id});
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,CreatedDate,Topic,Text")] QuestionCreateModel questionCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(questionCreateModel);
            }

            _repository.CreateQuestion(
                Question.CreateQuestion(
                    questionCreateModel.UserId,
                    questionCreateModel.Topic,
                    questionCreateModel.Text//,
                   // new List<string> { "FE", "BU" }
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _repository.GetQuestionById(id.Value);
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
            var questionToBeEdited = _repository.GetQuestionById(id);

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
                _repository.EditQuestion(questionToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(_repository.GetQuestionById(id).Id))
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

            var question = _repository.GetQuestionById(id.Value);
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
            var question = _repository.GetQuestionById(id);

            _repository.DeleteQuestion(question);

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(Guid id)
        {
            return _repository.GetAllQuestions().Any(question => question.Id == id);
        }
    }
}
