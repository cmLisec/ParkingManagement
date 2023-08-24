using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagement.Domain.Models
{
    public class SettleUpHistory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PayerUser")]
        public int PayerUserId { get; set; }
        [ForeignKey("ReceiverUser")]
        public int ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public User PayerUser { get; set; }
        public User ReceiverUser { get; set; }
    }
}
