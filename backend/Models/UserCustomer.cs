using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    /// <summary>
    /// 存放 Level 1 提供給 Level 2 的帳密資訊 (綁定關係)
    /// </summary>
    [Table("UserCustomers")]
    public class UserCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? UserProfileId { get; set; }

        [Required]
        public int ErpUserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerUid { get; set; } = string.Empty; // Level 2 的 ERP 帳號

        [Required]
        [MaxLength(100)]
        public string CustomerUpwd { get; set; } = string.Empty; // Level 2 的 ERP 密碼

        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserProfileId")]
        public UserProfile? UserProfile { get; set; }

        [ForeignKey("ErpUserId")]
        public ErpUser? ErpUser { get; set; }
    }
}
