namespace ParkingManagement.Domain.DTO
{
    public class TransactionDTO
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsPayment { get; set; }
        public string PayedBy { get; set; }
    }
}
