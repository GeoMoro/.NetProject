using System;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class LecturesController : Controller
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        // GET: Lectures
        public IActionResult Index()
        {
            return View(_lectureService.GetAllLectures());
        }

        // GET: Lectures/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _lectureService.GetLectureById(id.Value);

            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: Lectures/Create
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public async Task<IActionResult> Create([Bind("Title,Description,File")] LectureCreateModel lectureCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(lectureCreateModel);
            }

            await _lectureService.CreateLecture(lectureCreateModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: Lectures/Edit/5
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _lectureService.GetLectureById(id.Value);

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
        [Authorize(Roles = "Owner, Assistant")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,File")] LectureEditModel lectureModel)
        {
            var lectureEdited = _lectureService.GetLectureById(id);

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
                await _lectureService.Edit(id, lectureEdited, lectureModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureExists(_lectureService.GetLectureById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Lectures/Delete/5
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = _lectureService.GetLectureById(id.Value);

            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var lecture = _lectureService.GetLectureById(id);

            _lectureService.DeleFromPath(id);

            _lectureService.DeleteLecture(lecture);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteFile(string fileName, Guid? givenId)
        {
            _lectureService.DeleteFile(fileName, givenId);

            return RedirectToAction("Delete", "Lectures", new { id = givenId });
        }

        [HttpPost]
        public IActionResult Download(Guid lectureId, string fileName)
        {
            var file = _lectureService.SearchLecture(lectureId, fileName);

            return File(file, "application/octet-stream", fileName);
        }

        private bool LectureExists(Guid id)
        {
            return _lectureService.GetAll(id);
        }
    }
}