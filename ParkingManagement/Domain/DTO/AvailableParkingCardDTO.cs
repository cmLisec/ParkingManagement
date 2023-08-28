namespace ParkingManagement.Domain.DTO
{
    public class AvailableParkingCardDTO
    {
        /// <summary>
        /// AvailableParkingCards
        /// </summary>
        public Dictionary<int, List<TimeSlotDTO>> AvailableParkingCards { get; set; }

    }
}
