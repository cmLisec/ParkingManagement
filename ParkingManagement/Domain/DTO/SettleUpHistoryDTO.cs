using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagement.Domain.DTO
{
    public class SettleUpHistoryDTO
    {
        public int Id { get; set; }
        public int PayerUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
