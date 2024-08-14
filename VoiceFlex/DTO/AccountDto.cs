using VoiceFlex.Models;

namespace VoiceFlex.DTO;

public class AccountDto
{
    public Guid Id { get; set; }

    public string Description { get; set; }

    public AccountStatus Status { get; set; }

    public List<PhoneNumberDto> PhoneNumbers { get; set; }
}
