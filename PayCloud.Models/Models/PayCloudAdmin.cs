using System.ComponentModel.DataAnnotations;

namespace PayCloud.Data.Models
{
    public class PayCloudAdmin
    {
        [Required]
        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
