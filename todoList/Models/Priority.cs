using System.ComponentModel.DataAnnotations;

namespace todoList.Models
{
    public class Priority
    {
        [Key]
        public int PriorityId { get; set; }
        [Display(Name = "Priorytet")]
        public string Name { get; set; }
        public string Color { get; set; }

    }
}
