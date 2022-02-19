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
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = new List<Models.Task>();
            var PriorityList = _db.Priorities.ToList();
            DateTime date = DateTime.Now;
            for (int i = 1; i <= PriorityList.Count; i++)
            {
                var item = _db.Tasks.Select(e => e).Where(e => e.UserId == userId && e.PriorityId == i && e.DateTime > date).OrderBy(e => e.DateTime).Take(3);
                var itemlist = item;
                if (itemlist != null)
                {
                    foreach (var itemL in itemlist)
                    {
                        list.Add(itemL);
                    }
                }
            }
            var vm = new TaskPriorityViewModel();
            vm.Tasks = list;
            vm.Priorities = PriorityList;
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}