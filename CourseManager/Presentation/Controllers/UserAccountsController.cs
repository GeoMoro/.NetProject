using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly IUserAccountRepository _repository;

        public UserAccountsController(IUserAccountRepository repository)
        {
            _repository = repository;
        }

        // GET: UserAccounts
        public IActionResult Index()
        {
            return View(_repository.GetAllUsers());
        }

        // GET: UserAccounts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: UserAccounts/CreateStudent
        public IActionResult Createstudent()
        {
            return View();
        }

        // POST: UserAccounts/CreateStudent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Createstudent([Bind("FirstName,LastName,RegistrationNumber,Group,Password,ConfirmPassword,Email")] UserAccountStudentCreateModel userAccountStudentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAccountStudentCreateModel);
            }

            _repository.CreateUser(
                UserAccount.CreateStudentAccount(
                    userAccountStudentCreateModel.FirstName,
                    userAccountStudentCreateModel.LastName,
                    userAccountStudentCreateModel.RegistrationNumber,
                    userAccountStudentCreateModel.Group,
                    userAccountStudentCreateModel.Password,
                    userAccountStudentCreateModel.Email
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/CreateStudent
        public IActionResult CreateAssistant()
        {
            return View();
        }

        // POST: UserAccounts/CreateAssistant
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAssistant([Bind("FirstName,LastName,RegistrationNumber,Group,Password,ConfirmPassword,Email")] UserAccountAssistantCreateModel userAccountAssistantCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAccountAssistantCreateModel);
            }

            _repository.CreateUser(
                UserAccount.CreateAssistantAccount(
                    userAccountAssistantCreateModel.FirstName,
                    userAccountAssistantCreateModel.LastName,
                    userAccountAssistantCreateModel.Password,
                    userAccountAssistantCreateModel.Email
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,FirstName,LastName,RegistrationNumber,Group,Password,Email,Rank,Validated")] UserAccount userAccount)
        {
            if (id != userAccount.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userAccount);
            }

            try
            {
                _repository.EditUser(userAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(userAccount.Id))
                {
                    return NotFound();
                }

                throw;

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var userAccount = _repository.GetUserById(id);

            _repository.DeleteUser(userAccount);

            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(Guid id)
        {
            return _repository.GetAllUsers().Any(e => e.Id == id);
        }
    }
}
