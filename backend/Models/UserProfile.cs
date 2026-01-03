using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("UserProfiles")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [Column("UserId")]
        public string UserId { get; set; } = string.Empty;
        
        [Column("DisplayName")]
        public string? DisplayName { get; set; }
        
        [Column("PictureUrl")]
        public string? PictureUrl { get; set; }
        
        [Column("StatusMessage")]
        public string? StatusMessage { get; set; }
        
        [Column("CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        
        [Column("UpdateTime")]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        [Column("ErpId")]
        public string? ErpId { get; set; }

        [Column("IsErpBound")]
        public bool IsErpBound { get; set; } = false;
    }

    public class DashboardData
    {
        public string? Message { get; set; }
        public string? UpdateTime { get; set; }
        public string? ServerStatus { get; set; }
        public int Notifications { get; set; }
    }
}
