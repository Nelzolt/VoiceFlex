using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceFlex.Models
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string Number { get; set; }

        public Guid? AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
