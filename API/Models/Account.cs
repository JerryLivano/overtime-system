using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tbl_m_accounts")]
    public class Account
    {
        [Key, Column("id", TypeName = "char(36)")]
        public Guid Id {  get; set; }
        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; } = string.Empty;
        [Column("otp")]
        public int Otp { get; set; }
        [Column("expired")]
        public DateTime Expired { get; set; }
        [Column("is_used")]
        public bool IsUsed { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }

        // Cardinality
        public virtual ICollection<AccountRole>? AccountRoles { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<OvertimeRequest>? OvertimeRequests { get; set; }
    }
}
