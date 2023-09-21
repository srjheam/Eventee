using Eventee.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Controllers.Dtos
{
    public class GetTogetherDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(50)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public DateTime ScheduleDate { get; set; }
        [Required]
        public string HosterId { get; set; }
    }
}
