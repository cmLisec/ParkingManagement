using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagement.Domain.Models
{
    public class ParkingCardHistory
    {
        /// <summary>
        /// Specify Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        [ForeignKey(nameof(ParkingCardId))]

        public int ParkingCardId { get; set; }

        /// <summary>
        /// Specify time
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Specify start date
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Specify end date
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Specify parked location
        /// </summary>
        [Required]
        public string ParkedLocation { get; set; }

        public DateTime CreatedAt { get; set; }

        //[ForeignKey(nameof(UserId))]
        //[InverseProperty(nameof(User.ParkingCardHistory))]
        public User User { get; set; }

        [ForeignKey(nameof(ParkingCardId))]
        //[InverseProperty(nameof(ParkingCard.ParkingCardHistory))]
        public ParkingCard ParkingCard { get; set; }


    }
}
