namespace ParkingManagement.Domain.DTO
{
    public class DateSlotDTO
    {
        public Dictionary<DateTime, List<TimeSlotDTO>> AvailableSlot { get; set; }
    }
}
