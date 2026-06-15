using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.DTOs
{
    public class logindto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }


       
    }
}
