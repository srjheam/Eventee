﻿using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Controllers.Dtos
{
    public class SignUpDto
    {
        [Required]
        [MinLength(5)]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression("([0-9]{1,})")]
        public string? Password { get; set; }
    }
}
