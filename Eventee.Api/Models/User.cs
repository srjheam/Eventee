using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventee.Api.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<GetTogether> HostedGetTogethers { get; set; }
    public ICollection<GetTogether> SubscribedGetTogethers { get; set; }
}
