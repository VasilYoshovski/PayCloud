using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class AccountUserDto
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Username { get; set; }
    }
}
