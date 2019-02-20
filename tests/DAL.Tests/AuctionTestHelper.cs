using System;
using Microsoft.EntityFrameworkCore;
using odec.Framework.Logging;
using odec.Server.Model.Auction;
using odec.Server.Model.Category;
using odec.Server.Model.User;

namespace Auction.DAL.Tests
{
    static class AuctionTestHelper
    {
        public static void PopulateDefaultData(DbContext db)
        {
            try
            {
                db.Set<AuctionState>().Add(new AuctionState
                {
                    Code = "OPEN",
                    Name = "Open",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<Role>().Add(new Role
                {
                    Id = 1,
                    Name = "Crafter"

                });
                db.Set<Role>().Add(new Role
                {
                    Id = 2,
                    Name = "User",
                });
                db.Set<UserRole>().Add(new UserRole { RoleId = 1, UserId = 1 });

                db.Set<User>().Add(new User
                {
                    Id = 1,
                    UserName = "Andrew",

                });
                db.Set<User>().Add(new User
                {
                    Id = 2,
                    UserName = "Alex",
                });
                db.Set<AuctionType>().Add(new AuctionType
                {
                    Code = "CustomerAUX",
                    Name = "Customer Auction",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<AuctionType>().Add(new AuctionType
                {
                    Code = "GoodAuction",
                    Name = "Good Auction",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<Category>().Add(new Category
                {
                    Code = "FIX",
                    Name = "Blacksmith",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<Category>().Add(new Category
                {
                    Code = "FIX2",
                    Name = "Carpenter",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message,ex);
                throw;
            }
            
        }
    }
}
