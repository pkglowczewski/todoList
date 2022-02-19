using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

                    return RedirectToAction("Index", new { date = task.DateTime });
            }
                catch
                {
                    throw;
                }
            }

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Models.Task task = _db.Tasks.Find(id);
            var priorityList = _db.Priorities.ToList();
            var vm = new TaskPriorityViewModel();
            vm.Task = task;
            vm.Priorities = priorityList;
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }
    

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskPriorityViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vm = model.Task;
            vm.UserId = userId;
            _db.Entry(vm).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index", new { date = vm.DateTime });
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Models.Task task = _db.Tasks.Find(id);
            var priority = _db.Priorities.Find(task.PriorityId);
            var vm = new TaskPriorityViewModel();
            vm.Task = task;
            vm.Priority = priority;
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Models.Task task = _db.Tasks.Find(id);
                _db.Tasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction("Index", new { date = task.DateTime });
            }
            catch
            {
                return View();
            }
        }
    }
}
