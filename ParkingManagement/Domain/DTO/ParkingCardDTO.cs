using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ParkingManagement.Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Domain.DTO
{
    public class ParkingCardDTO
    {
        /// <summary>
        /// Specify Id
        /// </summary>
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

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
        public string ParkedLocation { get; set; }

        [Required]
        public int CardId { get; set; }

        [ValidateNever]
        public UserDto User { get; set; }
    }
}
