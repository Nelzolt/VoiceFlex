using VoiceFlex.Models;

namespace VoiceFlex.DTO;

public class AccountDto
{
    public Guid Id { get; set; }

    public string Description { get; set; }

    public AccountStatus Status { get; set; }

    public List<PhoneNumberDto> PhoneNumbers { get; set; }

    public AccountDto() { }

    public AccountDto(Guid id, string description, AccountStatus status, ICollection<PhoneNumber> phoneNumbers)
        => (Id, Description, Status, PhoneNumbers) = (id, description, status,
            phoneNumbers.Select(p => new PhoneNumberDto(p.Id, p.Number, id)).OrderBy(p => p.Number).ToList());
}
