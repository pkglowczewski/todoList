using System.ComponentModel.DataAnnotations;

namespace todoList.Models
{
    public class TaskPriorityViewModel
    {
        public Priority Priority { get; set; }
        public List<Priority> Priorities { get; set; }
        public Task Task { get; set; }
        public List<Task> Tasks { get; set; }
        [Display(Name = "Data")]
        public DateTime DatePick { get; set; }
        public string Date { get; set; }
    }
}
