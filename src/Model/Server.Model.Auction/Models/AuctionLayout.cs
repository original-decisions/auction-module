using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attach = odec.Server.Model.Attachment.Attachment;
namespace odec.Server.Model.Auction
{
    public class AuctionLayout
    {
        [Required(AllowEmptyStrings = true)]
        public string Title { get; set; }
        
        //[Key, Column(Order = 0)]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
     //   [Key, Column(Order = 1)]
        public int LayoutId { get; set; }
        public Attach Layout { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    }
}