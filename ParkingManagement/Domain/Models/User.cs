﻿using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Domain.Models
{
    /// <summary>
    /// This class is a model for user table.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Specify Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Specify Name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Specify user email
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Specify user contact number
        /// </summary>
        public long ContactNumber { get; set; }
        /// <summary>
        /// Specify User Password
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Specify User Car NumberPlate
        /// </summary>
        [Required]
        public string CarNumber { get; set; }
        /// <summary>
        /// Specify user image
        /// </summary>
        public string ImageS3Link { get; set; }
        /// <summary>
        /// Specify created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Specify updated date
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        public virtual List<Payments> Payments { get; set; }
        /// <summary>
        /// Specify Device Id
        /// </summary>
        public string DeviceId { get; set; }

        //[InverseProperty("UserIdNavigation")]
        //public virtual ICollection<ParkingCard> ParkingCard { get; set; }

        //[InverseProperty("UserIdNavigation")]
        //public virtual ICollection<ParkingCardHistory> ParkingCardHistory { get; set; }
    }
}
