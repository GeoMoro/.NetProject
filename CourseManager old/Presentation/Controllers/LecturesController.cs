using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class LecturesController : Controller
    {
        private readonly ILectureRepository _repository;

        public LecturesController(ILectureRepository repository)
        {
            _repository = repository;
        }

        // GET: Lectures
        public IActionResult Index()
        {
            return View(_repository.GetAllLectures());
        }

        // GET: Lectures/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _repository.GetLectureById(id.Value);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: Lectures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description")] LectureCreateModel lectureCreateModel)
        {
            if (!ModelState.IsValid)
            {
               return View(lectureCreateModel);
            }

            _repository.CreateLecture(
                Lecture.CreateLecture(
                    lectureCreateModel.Title,
                    lectureCreateModel.Description
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: Lectures/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _repository.GetLectureById(id.Value);
            if (lecture == null)
            {
                return NotFound();
            }

            var lectureEditModel = new LectureEditModel(
                lecture.Title,
                lecture.Description
            );

            return View(lectureEditModel);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Title,Description")] LectureEditModel lectureModel)
        {
            var lectureEdited = _repository.GetLectureById(id);

            if (lectureEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(lectureModel);
            }

            lectureEdited.Title = lectureModel.Title;
            lectureEdited.Description = lectureModel.Description;

            try
            {
                _repository.EditLecture(lectureEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureExists(_repository.GetLectureById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Lectures/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _repository.GetLectureById(id.Value);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var lecture = _repository.GetLectureById(id);

            _repository.DeleteLecture(lecture);

            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(Guid id)
        {
            return _repository.GetAllLectures().Any(e => e.Id == id);
        }
    }
}
