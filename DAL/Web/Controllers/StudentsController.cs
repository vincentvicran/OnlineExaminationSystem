using BLL.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IQnAService _qnAService;
        private readonly IWebHostEnvironment _env;


        public StudentsController(IStudentService studentService, IExamService examService, IQnAService qnAService, IWebHostEnvironment env)
        {
            _studentService = studentService;
            _examService = examService;
            _qnAService = qnAService;
            _env = env;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize=10)
        {
            return View(_studentService.GetAll(pageNumber, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddAsync(studentViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        public IActionResult AttendExam()
        {
            var model = new AttendExamViewModel();
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                model.StudentId = Convert.ToInt32(sessionObj.Id);
                model.QnAs = new List<QnAsViewModel>();
                var todayExam = _examService.GetAllExams()
                    .Where(a => a.StartDate.Date == DateTime.Today.Date)
                    .FirstOrDefault();
                if(todayExam == null)
                {
                    model.Message = "No Exam Scheduled for Today!";
                }
                else
                {
                    if (!_qnAService.IsExamAttended(todayExam.Id, model.StudentId))
                    {
                        model.QnAs = _qnAService.GetAllQnAsByExam(todayExam.Id).ToList();
                        model.ExamName = todayExam.Title;
                        model.Message = "";
                    }
                    else
                        model.Message = "You have already attended the exam!";
                }
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult AttendExam(AttendExamViewModel attendExamViewModel)
        {
            bool result = _studentService.SetExamResult(attendExamViewModel);
            return RedirectToAction("AttendExam");
        }
        [HttpGet]
        public IActionResult Result(string studentId)
        {
            var model = _studentService.GetExamResults(Convert.ToInt32(studentId));
            return View(model);
        }
        [HttpGet]
        public IActionResult ViewResult()
        {
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                var model = _studentService.GetExamResults(Convert.ToInt32(sessionObj.Id));
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Profile()
        {
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                var model = _studentService.GetStudentDetails(Convert.ToInt32(sessionObj.Id));
                if (model.PictureFileName != null)
                {
                    model.PictureFileName = ConfigurationManager.GetFilePath() + model.PictureFileName;
                }
                model.CVFileName = ConfigurationManager.GetFilePath() + model.CVFileName;
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult Profile([FromForm]StudentViewModel studentViewModel)
        {
            if (studentViewModel.PictureFile != null)
                studentViewModel.PictureFileName = SaveStudentFile(studentViewModel.PictureFile);
            if (studentViewModel.CVFile != null)
                studentViewModel.CVFileName = SaveStudentFile(studentViewModel.CVFile);
            _studentService.UpdateAsync(studentViewModel);
            return RedirectToAction("Profile");
        }
        //IFormFile saved into wwwroot directory
        private string SaveStudentFile(IFormFile pictureFile)
        {
            if(pictureFile == null)
            {
                return string.Empty;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");
            return SaveFile(path, pictureFile);
        }

        private string SaveFile(string path, IFormFile pictureFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Guid.NewGuid().ToString() + "." + pictureFile.FileName
                .Split('.')[pictureFile.FileName.Split('.').Length - 1];
            path = Path.Combine(path, filename);
            using(Stream stream = new FileStream(path, FileMode.Create))
            {
                pictureFile.CopyTo(stream);
            }
            return filename;
        }


        [HttpGet]
        public IActionResult Delete(string studentId)
        {
            var model = _studentService.GetStudentDetails(Convert.ToInt32(studentId));
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int studentId)
        {
            var model = _studentService.GetStudentDetails(Convert.ToInt32(studentId));
            _studentService.DeleteStudents(model);
            return RedirectToAction("Index");
        }
    }
}
