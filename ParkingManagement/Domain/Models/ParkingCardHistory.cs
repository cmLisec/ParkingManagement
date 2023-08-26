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
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("ParkingCard")]

        public int ParkingCardId { get; set; }
        public ParkingCard ParkingCard { get; set; }

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

        //[InverseProperty(nameof(ParkingCard.ParkingCardHistory))]


    }
}
