using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tbl_tr_overtime_requests")]
    public class OvertimeRequest
    {
        [Key, Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        [Column("account_id", TypeName = "char(36)")]
        public Guid AccountId { get; set; }
        [Column("overtime_id", TypeName = "char(36)")]
        public Guid OvertimeId { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
        [Column("status", TypeName = "varchar(20)")]
        public string Status { get; set; } = string.Empty;
        [Column("comment", TypeName = "varchar(255)")]
        public string? Comment { get; set; }

        // Cardinality
        public virtual Overtime? Overtime { get; set; }
        public virtual Account? Account { get; set; }
    }
}
