using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using todoList.Data;
using todoList.Models;

namespace todoList.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaskController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: TaskController
        public ActionResult Index(DateTime date)
        {
            if (date.ToString("dd.MM.yyyy") == DateTime.MinValue.Date.ToString("dd.MM.yyyy"))
            {
                date = DateTime.Today;
            }
            date = date.Date;
            DateTime thisDay24 = date.AddDays(1);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasksList = _db.Tasks.Where(x => x.UserId == userId && x.DateTime > date && x.DateTime < thisDay24).OrderBy(x => x.DateTime).ToList();

            var priorityList = _db.Priorities.ToList();

            var dateForIndex = date.ToString("dddd, dd MMMM yyyy");

            var vm = new TaskPriorityViewModel();
            vm.Priorities = priorityList;
            vm.Tasks = tasksList;
            vm.Date = dateForIndex;

            return View(vm);
        }
        public ActionResult PickDate(DateTime datePick)
        {

            return RedirectToAction("Index", new { date = datePick });
        }
        // GET: TaskController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            var vm = new TaskPriorityViewModel();
            vm.Priorities = _db.Priorities.ToList();
            return View(vm);
        }
        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Task task, TaskPriorityViewModel model)
        {
                try
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    task.Name = model.Task.Name;
                    task.Description = model.Task.Description;
                    task.DateTime = model.Task.DateTime;
                    task.PriorityId = model.Task.PriorityId;
                    task.UserId = userId;

                    _db.Tasks.Add(task);
                    _db.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    throw;
                }
            }

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
