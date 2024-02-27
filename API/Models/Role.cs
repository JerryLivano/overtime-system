using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tbl_m_roles")]
    public class Role
    {
        [Key, Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        [Column("name", TypeName = "varchar(25)")]
        public string Name { get; set; } = string.Empty;

        // Cardinality
        public virtual ICollection<AccountRole>? AccountRoles { get; set; }
    }
}
