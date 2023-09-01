namespace ParkingManagement.Domain.DTO
{
    public class AvailableParkingCardDTO
    {
        /// <summary>
        /// AvailableParkingCards
        /// </summary>
        public Dictionary<int, List<DateSlotDTO>> AvailableParkingCards { get; set; }

    }
}
