using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagement.Domain.Models
{
    public class ParkingCard
    {
        /// <summary>
        /// Specify Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

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
        //[InverseProperty(nameof(User.ParkingCard))]

        //[InverseProperty("ParkingCardIdNavigation")]
        //public virtual ICollection<ParkingCardHistory> ParkingCardHistory { get; set; }
        [ForeignKey("CardDetails")]

        public int CardId { get; set; }
        public CardDetails CardDetails { get; set; }
    }
}
