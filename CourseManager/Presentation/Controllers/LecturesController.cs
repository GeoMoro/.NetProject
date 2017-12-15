using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Extensions;

namespace Presentation.Controllers
{
    public class LecturesController : Controller
    {

        private readonly IHostingEnvironment _env;
        private readonly ILectureRepository _repository;
        
        public LecturesController(ILectureRepository repository, IHostingEnvironment env)
        {
            _env = env;
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
        public async Task<IActionResult> Create([Bind("Title,Description,File")] LectureCreateModel lectureCreateModel)
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
            foreach(var file in lectureCreateModel.File)
            {
                if (file.Length > 0)
                {
                    string path = Path.Combine(_env.WebRootPath, "Lectures/" + lectureCreateModel.Title);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // string extension = lectureCreateModel.Title + "." + Path.GetExtension(file.FileName).Substring(1);

                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

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

            var oldTitle = lectureEdited.Title;

            lectureEdited.Title = lectureModel.Title;
            lectureEdited.Description = lectureModel.Description;
            
            try
            {
                _repository.EditLecture(lectureEdited);

                string searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + oldTitle);

                if (Directory.Exists(searchedPath))
                {
                    Directory.Delete(searchedPath, true);
                }
                
                foreach (var file in lectureModel.File)
                {
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(_env.WebRootPath, "Lectures/" + lectureModel.Title);

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
            
            string searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + lecture.Title);
            Directory.Delete(searchedPath, true);

            _repository.DeleteLecture(lecture);

            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(Guid id)
        {
            return _repository.GetAllLectures().Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult DeleteFile(string title, string fileName, Guid? givenId)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + title + "/" + fileName);
                if ((System.IO.File.Exists(searchedPath)))
                {
                    System.IO.File.Delete(searchedPath);
                }
            }
            
            return RedirectToAction("Delete", "Lectures", new { id = givenId });
        }

        [HttpPost]
        public IActionResult Download(string title, string fileName)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + title + "/" + fileName);
                Stream file = new FileStream(searchedPath, FileMode.Open);
                string content_type = "application/octet-stream";

                return File(file, content_type, fileName);
            }
        }
    }
}
