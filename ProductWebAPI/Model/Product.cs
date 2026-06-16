using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;
       
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public List<Item> Items { get; set; }

        
        public string? description { get; set; }

    }
    public class Item
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
