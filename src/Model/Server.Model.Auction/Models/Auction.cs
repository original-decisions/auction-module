using System;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;
using Conv = odec.Server.Model.Conversation.Conversation;
namespace odec.Server.Model.Auction
{
    public class Auction:Glossary<int>
    {
        [Required]
        public int UserId { get; set; }
        public User.User User { get; set; }

       // [Index("ix_Auction_StartDateEndDate",0)]
        public DateTime? StartDate { get; set; }
     //   [Index("ix_Auction_StartDateEndDate",1)]
        public DateTime? EndDate { get; set; }

        public decimal? Step { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? AutoClosePrice { get; set; }
        [Required]
        public int AuctionTypeId { get; set; }
        public AuctionType AuctionType { get; set; }

        public int? ConversationId { get; set; }
        public Conv Conversation { get; set; }

        [Required]
        public string Description { get; set; }

        public int StateId { get; set; }

        public AuctionState State { get; set; }
    }
}