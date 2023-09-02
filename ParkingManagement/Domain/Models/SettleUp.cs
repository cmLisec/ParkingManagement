using ParkingManagement.Domain.Dtos;

namespace ParkingManagement.Domain.Models
{
    public class SettleUp
    {
        public User User { get; set; }
        public decimal AmountToSettle { get; set; }
    }
}
