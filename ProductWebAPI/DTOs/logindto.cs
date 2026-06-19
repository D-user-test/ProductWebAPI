using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.DTOs
{
    public class logindto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }


       
    }
}
