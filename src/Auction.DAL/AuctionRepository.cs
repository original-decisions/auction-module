using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Auction.DAL.Interop;
using odec.Entity.DAL;
using odec.Framework.Logging;
using odec.Framework.Extensions;
using odec.Server.Model.Auction;
using odec.Server.Model.Auction.Filters;
using odec.Server.Model.Category;
using odec.Server.Model.User;

namespace odec.Auction.DAL
{
    public class AuctionRepository : OrmEntityOperationsRepository<int, Server.Model.Auction.Auction, DbContext>, IAuctionRepository<int, DbContext, Server.Model.Auction.Auction, AuctionVideo, AuctionLayout, Category, User, AuctionBid, AuctionFilter>
    {
        public AuctionRepository()
        {

        }

        public AuctionRepository(DbContext db)
        {
            Db = db;
        }


        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }

        public IEnumerable<Server.Model.Auction.Auction> Get(AuctionFilter filter)
        {
            try
            {
                var query = from auction in Db.Set<Server.Model.Auction.Auction>()
                            select auction;
                if (filter.UserId.HasValue)
                    query = query.Where(it => it.UserId == filter.UserId);

                if (filter.StateId.HasValue)
                    query = query.Where(it => it.StateId == filter.StateId);

                if (filter.Categories != null && filter.Categories.Any())
                {
                    var cIds = filter.Categories.Select(it => it.Id);
                    //Union
                    //TODO: Cross
                    query = from auction in query
                            join auctionSkill in Db.Set<AuctionSkill>() on auction.Id equals auctionSkill.AuctionId
                            where cIds.Contains(auctionSkill.CategoryId)
                            select auction;
                }

                if (filter.AuctionTypeId.HasValue)
                    query = query.Where(it => it.AuctionTypeId == filter.AuctionTypeId);

                if (filter.StartDateInterval?.Start != null)
                    query = query.Where(it => it.StartDate >= filter.StartDateInterval.Start);
                if (filter.StartDateInterval?.End != null)
                    query = query.Where(it => it.StartDate <= filter.StartDateInterval.End);
                if (filter.EndDateInterval?.Start != null)
                    query = query.Where(it => it.EndDate >= filter.EndDateInterval.Start);
                if (filter.EndDateInterval?.End != null)
                    query = query.Where(it => it.EndDate <= filter.EndDateInterval.End);

                if (filter.InitialPriceInterval != null && filter.InitialPriceInterval.Start.HasValue)
                    query = query.Where(it => it.InitialPrice >= filter.InitialPriceInterval.Start);
                if (filter.InitialPriceInterval != null && filter.InitialPriceInterval.End.HasValue)
                    query = query.Where(it => it.InitialPrice <= filter.InitialPriceInterval.End);

                if (filter.AutoClosePriceInterval != null && filter.AutoClosePriceInterval.Start.HasValue)
                    query = query.Where(it => it.AutoClosePrice >= filter.AutoClosePriceInterval.Start);
                if (filter.AutoClosePriceInterval != null && filter.AutoClosePriceInterval.End.HasValue)
                    query = query.Where(it => it.AutoClosePrice <= filter.AutoClosePriceInterval.End);
                //todo: check the start end too
                if (filter.MaxPriceInterval != null && (filter.MaxPriceInterval.Start.HasValue || filter.MaxPriceInterval.End.HasValue))
                {
                    //TODO: Id Don't like it. It should be refactored 
                    var query2 = from auction in query
                                 join auctionBid in Db.Set<AuctionBid>() on auction.Id equals auctionBid.AuctionId
                                 group new { auctionBid } by
                                     new { auctionBid.AuctionId }
                        into tmp
                                 select tmp;
                    if (filter.MaxPriceInterval.Start.HasValue)
                        query2 = query2.Where(it => it.Max(it2 => it2.auctionBid.Value) >= filter.MaxPriceInterval.Start);
                    if (filter.MaxPriceInterval.End.HasValue)
                        query2 = query2.Where(it => it.Max(it2 => it2.auctionBid.Value) <= filter.MaxPriceInterval.End);

                    query = from auction in query
                            join auction2 in query2 on auction.Id equals auction2.Key.AuctionId
                            select auction;
                }
                //Case sensetive contains
                if (!string.IsNullOrEmpty(filter.Title))
                    query = query.Where(it => it.Name.ToUpper().Contains(filter.Title.ToUpper()));
                //todo:add filter bool flag to initialize includes
                if (true)
                {
                    query = query.Include(it => it.User).Include(it => it.AuctionType);
                }

                query = filter.Sord.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(filter.Sidx)
                    : query.OrderBy(filter.Sidx);

                var result = query.Skip(filter.Rows * (filter.Page - 1)).Take(filter.Rows).Distinct();
                return result;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddVideo(Server.Model.Auction.Auction auction, AuctionVideo video)
        {
            try
            {
                video.AuctionId = auction.Id;
                Db.Set<AuctionVideo>().Add(video);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveVideo(Server.Model.Auction.Auction auction, AuctionVideo video)
        {
            try
            {
                var auctionVideo = Db.Set<AuctionVideo>().Single(it => it.VideoId == video.VideoId && it.AuctionId == auction.Id);
                Db.Set<AuctionVideo>().Remove(auctionVideo);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<AuctionVideo> GetAuctionVideos(int auctionId)
        {
            try
            {
                return Db.Set<AuctionVideo>()
                     .Where(it => it.AuctionId == auctionId)
                     .Include(it => it.Video);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<AuctionLayout> GetAuctionLayouts(int auctionId)
        {
            try
            {
                return Db.Set<AuctionLayout>()
                    .Where(it => it.AuctionId == auctionId)
                    .Include(it => it.Layout);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<Category> GetAuctionCategories(int auctionId)
        {
            try
            {
                return
                    Db.Set<AuctionSkill>()
                        .Where(it => it.AuctionId == auctionId)
                        .Include(it => it.Category)
                        .Select(it => it.Category);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<AuctionBid> GetAuctionBids(int auctionId)
        {
            try
            {
                return Db.Set<AuctionBid>().Where(it => it.AuctionId == auctionId).Include(it => it.User);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddLayout(Server.Model.Auction.Auction auction, AuctionLayout layout)
        {
            try
            {
                layout.AuctionId = auction.Id;
                Db.Set<AuctionLayout>().Add(layout);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveLayout(Server.Model.Auction.Auction auction, AuctionLayout layout)
        {
            try
            {
                var auctionLayout = Db.Set<AuctionLayout>().Single(it => it.LayoutId == layout.LayoutId && it.AuctionId == auction.Id);
                Db.Set<AuctionLayout>().Remove(auctionLayout);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddCategory(Server.Model.Auction.Auction auction, Category category)
        {
            try
            {
                Db.Set<AuctionSkill>().Add(new AuctionSkill
                {
                    AuctionId = auction.Id,
                    CategoryId = category.Id
                });
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveCategory(Server.Model.Auction.Auction auction, Category category)
        {
            try
            {
                var auctionCategory = Db.Set<AuctionSkill>().Single(it => it.CategoryId == category.Id && it.AuctionId == auction.Id);
                Db.Set<AuctionSkill>().Remove(auctionCategory);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void MakeBid(User user, Server.Model.Auction.Auction auction, decimal value, string comment = null, DateTime? estimatedStartDate = null, DateTime? estimatedEndDate = null)
        {
            try
            {
                Db.Set<AuctionBid>().Add(new AuctionBid
                {
                    AuctionId = auction.Id,
                    UserId = user.Id,
                    Value = value,
                    Description = comment ?? String.Empty,
                    EstimatedEndDate = estimatedEndDate,
                    EstimatedStartDate = estimatedStartDate,
                });
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void SelectExecutor(User user, Server.Model.Auction.Auction auction)
        {
            try
            {
                var auctionBids = Db.Set<AuctionBid>().Where(it => it.AuctionId == auction.Id);
                var selectedBids = auctionBids.Where(it => it.IsSelected);
                //deselect bids
                if (selectedBids.Any())
                    foreach (var selectedBid in selectedBids)
                        selectedBid.IsSelected = false;

                var bid = auctionBids.Single(it => it.UserId == user.Id);
                bid.IsSelected = true;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public User GetExecutor(Server.Model.Auction.Auction auction)
        {
            try
            {
                return Db.Set<AuctionBid>().Include(it => it.User).Single(it => it.AuctionId == auction.Id && it.IsSelected).User;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveBids(Server.Model.Auction.Auction auction)
        {
            try
            {
                var bids = Db.Set<AuctionBid>().Where(it => it.AuctionId == auction.Id);
                Db.Set<AuctionBid>().RemoveRange(bids);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveBid(User user, Server.Model.Auction.Auction auction)
        {
            try
            {
                var bid = Db.Set<AuctionBid>().Single(it => it.UserId == user.Id && it.AuctionId == auction.Id);
                Db.Set<AuctionBid>().Remove(bid);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
