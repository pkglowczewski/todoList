using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using todoList.Data;
using todoList.Models;

namespace todoList.Controllers
{  [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasksList = new List<Models.Task>();
            var priorityList = _db.Priorities.ToList();
            foreach (var priority in priorityList)
            {
                var items = _db.Tasks
                    .Where(e => e.UserId == userId && e.PriorityId == priority.PriorityId && e.DateTime > DateTime.Now)
                    .OrderBy(e => e.DateTime).Take(3).ToList();

                tasksList.AddRange(items);
            }
            var vm = new TaskPriorityViewModel()
            {
                Tasks = tasksList,
                Priorities = priorityList
            };
            return View(vm);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}