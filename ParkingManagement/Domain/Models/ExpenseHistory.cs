using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagement.Domain.Models
{
    public class ExpenseHistory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PayerUser")]
        public int PayerUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public User PayerUser { get; set; }
    }
}
