using Microsoft.AspNetCore.Identity;

namespace todoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
