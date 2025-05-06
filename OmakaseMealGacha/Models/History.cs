using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmakaseMealGacha.Models
{
    [Table("history")]
    public class History
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_name")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [Column("menu_name")]
        public string MenuName { get; set; }

        [Column("rolled_at")]
        public DateTime RolledAt { get; set; }
    }
}
