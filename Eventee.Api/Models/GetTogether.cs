using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Models
{
    public class GetTogether
    {
        public int Id { get; set; }
        [Required]
        [MinLength(50)]
        public string Title { get; set; } = null!;
        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime ScheduleDate { get; set; }
        public User Hoster { get; set; } = null!;
        public ICollection<User> Subscribers { get; set; } = null!;
    }
}
