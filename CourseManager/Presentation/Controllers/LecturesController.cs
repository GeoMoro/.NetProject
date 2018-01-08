using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class LecturesController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly ILectureRepository _repository;
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureRepository repository, IHostingEnvironment env, ILectureService lectureService)
        {
            _env = env;
            _repository = repository;
            _lectureService = lectureService;
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
        [Authorize(Roles = "Owner, Assistant")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,File")] LectureEditModel lectureModel)
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

                //var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + id);

                //if (Directory.Exists(searchedPath) && oldTitle.Equals(lectureModel.Title) == false)
                //{
                //    Directory.Delete(searchedPath, true);
                //}

                if (lectureModel.File != null)
                {
                    foreach (var file in lectureModel.File)
                    {
                        if (file.Length > 0)
                        {
                            string path = Path.Combine(_env.WebRootPath, "Lectures/" + id);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            // string extension = lectureModel.Title + "." + Path.GetExtension(file.FileName).Substring(1);

                            using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
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
        [Authorize(Roles = "Owner, Assistant")]
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
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var lecture = _repository.GetLectureById(id);

            string searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }
            _repository.DeleteLecture(lecture);

            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(Guid id)
        {
            return _repository.GetAllLectures().Any(e => e.Id == id);
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
    }
}