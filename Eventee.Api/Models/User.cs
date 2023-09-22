using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventee.Api.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public ICollection<GetTogether> HostedGetTogethers { get; set; } = null!;
    public ICollection<GetTogether> SubscribedGetTogethers { get; set; } = null!;
}
