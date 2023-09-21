using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Controllers.Dtos
{
    public class SignInDto
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
