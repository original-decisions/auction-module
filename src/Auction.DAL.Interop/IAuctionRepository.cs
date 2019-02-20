using System;
using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Auction.DAL.Interop
{
    public interface IAuctionRepository<TKey, TDbContext, TAuction, TAuctionVideo, TAuctionLayout, TCategory, TUser, TAuctionBid, TAuctionFilter> :
        IContextRepository<TDbContext>,
        IActivatableEntity<TKey, TAuction>,
        IEntityOperations<TKey, TAuction> where TAuction : class where TKey : struct
    {
        IEnumerable<TAuction> Get(TAuctionFilter filter);
        void AddVideo(TAuction auction, TAuctionVideo video);
        void RemoveVideo(TAuction auction, TAuctionVideo video);
        IEnumerable<TAuctionVideo> GetAuctionVideos(TKey auctionId);
        IEnumerable<TAuctionLayout> GetAuctionLayouts(TKey auctionId);
        IEnumerable<TCategory> GetAuctionCategories(TKey auctionId);
        IEnumerable<TAuctionBid> GetAuctionBids(TKey auctionId); 
        void AddLayout(TAuction auction, TAuctionLayout layout);
        void RemoveLayout(TAuction auction, TAuctionLayout layout);
        void AddCategory(TAuction auction, TCategory category);
        void RemoveCategory(TAuction auction, TCategory category);
        
        void MakeBid(TUser user, TAuction auction, decimal value,string comment=null,DateTime? estimatedStartDate=null, DateTime? estimatedEndDate = null);
        void SelectExecutor(TUser user, TAuction auction);
        TUser GetExecutor(TAuction auction);
        void RemoveBids(TAuction auction);
        void RemoveBid(TUser user, TAuction auction);
    }
}