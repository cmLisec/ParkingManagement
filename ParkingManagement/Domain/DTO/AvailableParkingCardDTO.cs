namespace ParkingManagement.Domain.DTO
{
    public class AvailableParkingCardDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Dictionary<int, Dictionary<DateTime, DateTime>> AvailableParkingCards { get; set; }
    }
}
