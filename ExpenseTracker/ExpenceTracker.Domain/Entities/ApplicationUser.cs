using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public int? ImageFileId { get; set; }
        public virtual ImageFile? Image { get; set; }

        public ApplicationUser()
        {
            Notifications = [];
        }
    }
}
