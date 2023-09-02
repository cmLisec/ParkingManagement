using ParkingManagement.Domain.Dtos;

namespace ParkingManagement.Domain.DTO
{
    public class SettleUpDTO
    {
        public UserDto User { get; set; }
        public decimal AmountToSettle { get; set; }
    }
}
