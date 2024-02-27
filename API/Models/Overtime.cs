using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tbl_m_overtimes")]
    public class Overtime
    {
        [Key, Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("reason", TypeName = "varchar(255)")]
        public string Reason { get; set; } = string.Empty;
        [Column("total_hours")]
        public int TotalHours { get; set; }
        [Column("status", TypeName = "varchar(20)")]
        public string Status { get; set; } = string.Empty;
        [Column("document", TypeName = "varchar(255)")]
        public string Document { get; set; } = string.Empty;

        // Cardinality
        public virtual ICollection<OvertimeRequest>? OvertimeRequests { get; set; }
    }
}
