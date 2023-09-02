

namespace ParkingManagement.Domain.DTO
{
    public class ExpenseHistoryDTO
    {
        public int Id { get; set; }
        public int PayerUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
