using System;
using System.Linq;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;
        private readonly IRecordService _service;

        public NewsController(INewsRepository repository, IRecordService service)
        {
            _repository = repository;
            _service = service;
        }

        // GET
        public IActionResult Index(string loadButton)
        {
            if (loadButton != null)
            {
                var value = Int32.Parse(loadButton);
                return View(_service.GetNextFiveOrTheRest(value));
            }


            return View(_service.GetNextFiveOrTheRest(0));
        }



        public IActionResult CreateNews(string createdBy)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNews(string createdBy,[Bind("Title, Description")] NewsCreateModel newsCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newsCreateModel);
            }

            _repository.CreateNews(
                News.CreateNews(
                    newsCreateModel.Title,
                    newsCreateModel.Description,
                    createdBy
                )
            );

            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateNews(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = _repository.GetNewsById(id.Value);
            if (news == null)
            {
                return NotFound();
            }

            var newsEditModel = new NewsEditModel(
                news.Title,
                news.Description
            );

            return View(newsEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateNews(Guid id, [Bind("Title,Description, CreatedBy")] NewsEditModel newsEditModel)
        {
            var newsEdited = _repository.GetNewsById(id);

            if (newsEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(newsEditModel);
            }

            newsEdited.Title = newsEditModel.Title;
            newsEdited.Description = newsEditModel.Description;

            try
            {
                _repository.UpdateNews(newsEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(_repository.GetNewsById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteNews(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = _repository.GetNewsById(id.Value);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }
        
        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var news = _repository.GetNewsById(id);

            _repository.DeleteNews(news);

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(Guid id)
        {
            return _repository.GetAllNews().Any(e => e.Id == id);
        }
    }
}