using CrudStudentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using CrudStudentProject.Hubs;

namespace CrudStudentProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<StudentHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IHubContext<StudentHub> hubContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {

            if (_httpContextAccessor.HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var students = _dbContext.Students.ToList(); // Replace 'Students' with your actual DbSet for students
            return View(students);
        }
        public IActionResult StudList()
        {
            return View();
        }

        // GET: Create
        public IActionResult Create()
        {
            Student student = new Student();
            return View(student);
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Students.Add(student);
                _dbContext.SaveChanges();
                 _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New student added: {student.FirstName+" "+student.LastName}");
                return RedirectToAction(nameof(Index));

            }
            return View(student);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {

            var student = _dbContext.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(student);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Details
        public IActionResult Detail(int id)
        {

            var student = _dbContext.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var student = _dbContext.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Delete
        [HttpPost]

        public IActionResult DeleteConfirmed(int id)
        {

            var student = _dbContext.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            model.IsSuccess = false;
            base.TempData["Message"] = "Login failed";
            if (model.UserName != null && model.UserPassword!=null)
            {
                if (model.UserName.Trim() == "admin" && model.UserPassword.Trim() == "Admin123")
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", 1);
                    model.IsSuccess = true;
                    base.TempData["Message"] = null;
                }             
            }
            return Json(model);
        }
        public IActionResult Logout()
        {
            try
            {

                _httpContextAccessor.HttpContext.Session.Remove("UserId");
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
