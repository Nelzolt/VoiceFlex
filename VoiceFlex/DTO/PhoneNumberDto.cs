using VoiceFlex.Models;

namespace VoiceFlex.DTO;

public class PhoneNumberDto
{
    public Guid Id { get; set; }

    public string Number { get; set; }

    public Guid? AccountId { get; set; }

    public Account Account { get; set; }

    public PhoneNumberDto(Guid id, string number)
        => (Id, Number) = (id, number);
}
