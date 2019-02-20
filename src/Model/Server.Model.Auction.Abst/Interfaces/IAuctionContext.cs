using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Auction.Abst.Interfaces
{
    public interface IAuctionContext<TAuction, TAuctionType, TAuctionSkill, TAuctionVideo, TAuctionLayout, TAuctionBid>
        where TAuction : class 
        where TAuctionSkill : class 
        where TAuctionVideo : class 
        where TAuctionLayout : class 
        where TAuctionBid : class 
        where TAuctionType : class
    {
        DbSet<TAuction> Auctions { get; set; }
        DbSet<TAuctionSkill> AuctionSkills { get; set; }
        DbSet<TAuctionVideo> AuctionVideos { get; set; }
        DbSet<TAuctionLayout> AuctionLayouts { get; set; }
        DbSet<TAuctionBid> AuctionBids { get; set; }
        DbSet<TAuctionType> AuctionTypes { get; set; }
    }
}