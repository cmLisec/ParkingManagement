using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int PayerUserId { get; set; }
        public User PayerUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public ICollection<UserPayment> UserPayments { get; set; }
    }
}
