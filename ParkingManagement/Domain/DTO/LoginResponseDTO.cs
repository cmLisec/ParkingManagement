using ParkingManagement.Domain.Dtos;

namespace ParkingManagement.Domain.DTO
{
    public class LoginResponseDTO
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
