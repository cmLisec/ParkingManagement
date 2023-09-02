using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Domain.Dtos
{
    public class UserDto
    {
        /// <summary>
        /// Specify Id
        /// </summary>
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
        /// <summary>
        /// Specify Device Id
        /// </summary>
        public string DeviceId { get; set;}
    }
}
