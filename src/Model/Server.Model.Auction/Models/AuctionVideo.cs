using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attach = odec.Server.Model.Attachment.Attachment;
namespace odec.Server.Model.Auction
{
    public class AuctionVideo
    {
        [Required(AllowEmptyStrings = true)]
        public string Title { get; set; }
        
        public Auction Auction { get; set; }
      //  [Key,Column(Order = 0)]
        public int AuctionId { get; set; }
   //     [Key, Column(Order = 1)]
        public int VideoId { get; set; }
        public Attach Video { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    }
}