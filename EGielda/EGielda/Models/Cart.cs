using System.ComponentModel.DataAnnotations;
namespace EGielda.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CartItem> CartItems { get; set; }
    }
}
