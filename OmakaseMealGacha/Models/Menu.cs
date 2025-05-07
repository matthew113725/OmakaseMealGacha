using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmakaseMealGacha.Models
{
    [Table("menu")]
    public class Menu
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("menu_name")]
        public string Name { get; set; }

        [Column("is_in_gacha")]
        public bool IsInGacha { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("last_modified_on")]
        public DateTime LastModifiedOn { get; set; }
    }
}
