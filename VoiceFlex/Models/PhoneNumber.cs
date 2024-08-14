using System.ComponentModel.DataAnnotations.Schema;
using VoiceFlex.DTO;

namespace VoiceFlex.Models
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string Number { get; set; }

        public Guid? AccountId { get; set; }
        public virtual Account Account { get; set; }

        public PhoneNumber() { }

        public PhoneNumber(PhoneNumberDto phoneNumber)
            => (Id, Number) = (phoneNumber.Id, phoneNumber.Number);
    }
}
