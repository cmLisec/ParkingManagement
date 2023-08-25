using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Domain.Models
{
    public class CardDetails
    {
        [Key]
        public int Id { get; set; }

        public string CardName { get; set; }

        public string CardNumber { get; set; }
    }
}
