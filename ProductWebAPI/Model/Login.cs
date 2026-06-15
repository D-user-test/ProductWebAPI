using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductWebAPI.Model
{
    public class Login
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(60)")]
        public string username { get; set; }

        [Required]
        [Column(TypeName = "varchar(60)")]
        public string password { get; set; }

    }
}
