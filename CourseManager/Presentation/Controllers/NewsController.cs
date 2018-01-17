using System;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly IRecordService _service;

        public NewsController(IRecordService service)
        {
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

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult CreateNews(string createdBy)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult CreateNews(string createdBy,[Bind("Title, Description")] NewsCreateModel newsCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newsCreateModel);
            }

            _service.Create(createdBy, newsCreateModel);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult UpdateNews(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = _service.GetNewsById(id.Value);
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
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult UpdateNews(Guid id, [Bind("Title,Description, CreatedBy")] NewsEditModel newsEditModel)
        {
            var newsEdited = _service.GetNewsById(id);

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
                _service.Update(newsEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(_service.GetNewsById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteNews(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = _service.GetNewsById(id.Value);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }
        
        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var news = _service.GetNewsById(id);

            _service.Delete(news);

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(Guid id)
        {
           return _service.NewsExists(id);
        }
    }
}