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
        public ActionResult Index(DateTime? date)
        {
            if (!date.HasValue || date.Value.Day == DateTime.MinValue.Day)
            {
                date = DateTime.Today;
            }

            DateTime dateAddDay = date.Value.AddDays(1);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasksList = _db.Tasks.Where(x => x.UserId == userId && x.DateTime.Day == date.Value.Date.Day)
                .OrderBy(x => x.DateTime).ToList();

            var priorityList = _db.Priorities.ToList();

            var dateForIndex = date.Value.ToString("dddd, dd MMMM yyyy");

            var vm = new TaskPriorityViewModel()
            {
                Priorities = priorityList,
                Tasks = tasksList,
                Date = dateForIndex
            };

            return View(vm);
        }
        public ActionResult PickDate(DateTime datePick)
        {

            return RedirectToAction("Index", new { date = datePick.Date });
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            var vm = new TaskPriorityViewModel()
            {
                Priorities = _db.Priorities.ToList()
            };
            return View(vm);
        }
        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskPriorityViewModel model)
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                model.Task.UserId = userId;
                _db.Tasks.Add(model.Task);
                _db.SaveChanges();

                return RedirectToAction("Index", new { date = model.Task.DateTime.Date });

            }
            return View(model);
        }

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Task? task = _db.Tasks.FirstOrDefault(x => x.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            var priorityList = _db.Priorities.ToList();
            var vm = new TaskPriorityViewModel()
            {
                Task = task,
                Priorities = priorityList

            };

            return View(vm);
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

            return RedirectToAction("Index", new { date = vm.DateTime.Date });
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Task? task = _db.Tasks.FirstOrDefault(x => x.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            var priority = _db.Priorities.FirstOrDefault(x => x.PriorityId == task.PriorityId);
            var vm = new TaskPriorityViewModel()
            {
                Task = task,
                Priority = priority
            };

            return View(vm);
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Models.Task? task = _db.Tasks.FirstOrDefault(x => x.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            _db.Tasks.Remove(task);
            _db.SaveChanges();
            return RedirectToAction("Index", new { date = task.DateTime.Date });

        }
    }
}
