using ProductWebAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.DTOs
{
    public class Addproductdto
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public List<Item> itemlist { get; set; }

    }

    public class updateproductdto
    {

        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }


    }
}
