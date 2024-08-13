using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceFlex.Models;

public class Account
{
    public Guid Id { get; set; }

    [Column(TypeName = "varchar(1023)")]
    public string Description { get; set; }

    public AccountStatus Status { get; set; }

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
}
