using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;
using Presentation.Models.FactionViewModels;


namespace Presentation.Controllers
{
    public class FactionsController : Controller
    {
        private readonly IFactionRepository _repository;

        public FactionsController(IFactionRepository repository)
        {
            _repository = repository;
        }

        // GET: Factions
        public IActionResult Index()
        {
            return View(_repository.GetAllFaction());
        }

        // GET: Factions/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faction = _repository.GetFactionById(id.Value);
            if (faction == null)
            {
                return NotFound();
            }

            return View(faction);
        }

        // GET: Factions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Factions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Date,Week")] FactionCreateModel factionCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(factionCreateModel);
            }

            _repository.CreateFaction(
                Faction.CreateFaction(
                    factionCreateModel.Date,
                    factionCreateModel.Week
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: Factions/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faction = _repository.GetFactionById(id.Value);
            if (faction == null)
            {
                return NotFound();
            }

            var factionEditModel = new FactionEditModel(
                faction.Date,
                faction.Week);
        
            return View(factionEditModel);
        }

        // POST: Factions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Date,Week")] FactionEditModel factionEditModel)
        {
            var factionToBeEdited = _repository.GetFactionById(id);

            if (factionToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(factionEditModel);
            }

            factionToBeEdited.Date = factionEditModel.Date;
            factionToBeEdited.Week = factionEditModel.Week;
            

            try
            {
                _repository.UpdateFaction(factionToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactionExists(_repository.GetFactionById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Factions/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faction = _repository.GetFactionById(id.Value);
            if (faction == null)
            {
                return NotFound();
            }

            return View(faction);
        }

        // POST: Factions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var faction = _repository.GetFactionById(id);

            _repository.DeleteFaction(faction);

            return RedirectToAction(nameof(Index));
        }

        private bool FactionExists(Guid id)
        {
            return _repository.GetAllFaction().Any(e => e.Id == id);
        }
    }
}
