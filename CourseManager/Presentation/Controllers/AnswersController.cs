using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AnswersController : Controller
    {
        private readonly IAnswerRepository _repository;

        public AnswersController(IAnswerRepository repository)
        {
            _repository = repository;
        }

        // GET: Answers
        public IActionResult Index()
        {
            return View(_repository.GetAllAnswers());
        }

        // GET: Answers/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _repository.GetAnswerById(id.Value);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,QuestionId,AnswerDate,Text")] AnswerCreateModel answerCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(answerCreateModel);
            }

            _repository.CreateAnswer(
                Answer.CreateAnswer(
                    answerCreateModel.UserId,
                    answerCreateModel.QuestionId,
                    answerCreateModel.Text
                )
            );

            return RedirectToAction(nameof(Index));
            
        }

        // GET: Answers/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _repository.GetAnswerById(id.Value);
            if (answer == null)
            {
                return NotFound();
            }

            var answerEditModel = new AnswerEditModel(
                answer.UserId,
                answer.QuestionId,
                answer.Text
            );

            return View(answerEditModel);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("UserId,QuestionId,AnswerDate,Text")] AnswerEditModel answerEditModel)
        {
            var answerToBeEdited = _repository.GetAnswerById(id);

            if (answerToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(answerEditModel);
            }

            answerToBeEdited.UserId = answerEditModel.UserId;
            answerToBeEdited.QuestionId = answerEditModel.QuestionId;
            answerToBeEdited.AnswerDate = answerEditModel.AnswerDate;
            answerToBeEdited.Text = answerEditModel.Text;


            try
            {
                _repository.EditAnswer(answerToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(_repository.GetAnswerById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));

            
           
        }

        // GET: Answers/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _repository.GetAnswerById(id.Value);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var answer = _repository.GetAnswerById(id);

            _repository.DeleteAnswer(answer);

            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(Guid id)
        {
            return _repository.GetAllAnswers().Any(answer => answer.Id == id);
        }
    }
}
