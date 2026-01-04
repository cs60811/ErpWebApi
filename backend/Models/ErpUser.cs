using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    /// <summary>
    /// 存放 Level 1 ERP 系統連線資訊
    /// </summary>
    [Table("ErpUsers")]
    public class ErpUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErpCode { get; set; } = string.Empty; // 系統代碼 (識別用)

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // 客戶公司名稱

        [Required]
        [MaxLength(255)]
        public string BaseUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CID { get; set; } = string.Empty; // 正航公司別

        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<UserCustomer>? Customers { get; set; }
    }
}
