using System;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;

        public NewsController(INewsRepository repository)
        {
            _repository = repository;
        }

        // GET
        public IActionResult Index()
        {
            return View(_repository.GetAllNews());
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