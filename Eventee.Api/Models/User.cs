using System.ComponentModel.DataAnnotations;

namespace Eventee.Api.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<GetTogether> HostedGetTogethers { get; set; }
    public ICollection<GetTogether> SubscribedGetTogethers { get; set; }
}
