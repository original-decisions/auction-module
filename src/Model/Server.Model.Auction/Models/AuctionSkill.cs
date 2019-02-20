using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CategoryE = odec.Server.Model.Category.Category;
namespace odec.Server.Model.Auction
{
    public class AuctionSkill
    {
      //  [Key, Column(Order = 1)]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
     //   [Key, Column(Order = 0)]
        public int CategoryId { get; set; }
        public CategoryE Category { get; set; }
    }
}