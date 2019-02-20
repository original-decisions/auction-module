using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Auction
{
    public class AuctionBid
    {
     //   [Key, Column(Order = 0)]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        
    //    [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public User.User User { get; set; }

        [Required]
        public decimal Value { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        /// <summary>
        /// Estimated Start date for auctions like tenders
        /// </summary>
        public DateTime? EstimatedStartDate { get; set; }
        /// <summary>
        /// Estimated End date for auctions like tenders
        /// </summary>
        public DateTime? EstimatedEndDate { get; set; }
        public bool IsSelected { get; set; }
    }
}