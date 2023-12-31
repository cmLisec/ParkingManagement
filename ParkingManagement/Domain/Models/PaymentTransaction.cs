﻿using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Domain.Models
{
    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }
        public int PayerId { get; set; }
        public int PayeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
