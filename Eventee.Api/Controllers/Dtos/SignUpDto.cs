using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Controllers.Dtos
{
    public class SignUpDto
    {
        [Required]
        [MinLength(5)]
        public string? Name { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
