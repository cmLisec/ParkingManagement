namespace ParkingManagement.Domain.DTO
{
    public class TimeSlotDTO
    {


        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss.FFFZ}")]
        public DateTime StartDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss.FFFZ}")]

        public DateTime EndDate { get; set; }
    }
}
