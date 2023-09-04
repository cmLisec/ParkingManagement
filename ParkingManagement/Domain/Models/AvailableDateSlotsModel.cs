namespace ParkingManagement.Domain.Models
{
    public class AvailableDateSlotsModel
    {
        public Dictionary<DateTime, Dictionary<DateTime, DateTime>> AvailableSlot { get; set; }
    }
}
