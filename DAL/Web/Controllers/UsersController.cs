using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public IActionResult Index(int pageNumber=1, int pageSize = 10) 
        {

            return View(_accountService.GetAllTeachers(pageNumber,pageSize));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _accountService.AddTeacher(userViewModel);
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int userId)
        {
            _accountService.GetTeacherById(userId);

            return View();
        }
        [HttpPost]
        public IActionResult Delete(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _accountService.DeleteTeacher(userViewModel);
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }
    }
}
