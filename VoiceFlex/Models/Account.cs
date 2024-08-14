using System.ComponentModel.DataAnnotations.Schema;
using VoiceFlex.DTO;

namespace VoiceFlex.Models;

public class Account
{
    public Guid Id { get; set; }

    [Column(TypeName = "varchar(1023)")]
    public string Description { get; set; }

    public AccountStatus Status { get; set; }

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

    public Account() { }

    public Account(AccountDto account)
        => (Id, Description, Status) = (account.Id, account.Description, account.Status);
}
