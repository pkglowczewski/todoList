using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoList.Models
{
    [Table("Tasks")]
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Uzupełnij pole Nazwa")]
        [Display(Name = "Nazwa")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Uzupełnij pole Opis")]
        [Display(Name = "Opis")]
        [StringLength(300)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Uzupełnij pole Data")]
        [Display(Name = "Data")]
        public DateTime DateTime { get; set; }
        [Display(Name = "Zakończone")]
        public bool isDone { get; set; }
        [Required]
        [Range(1,int.MaxValue, ErrorMessage ="Wybierz priorytet z listy")]
        [Display(Name = "Priorytet")]
        public int PriorityId { get; set; }
        [Display(Name = "Priorytet")]
        public Priority Priority { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
