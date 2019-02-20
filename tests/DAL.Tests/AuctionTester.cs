using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Auction.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Auction.DAL;
using odec.Framework.Generic.Utility;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using odec.Server.Model.Auction;
using odec.Server.Model.Auction.Contexts;
using odec.Server.Model.Auction.Filters;
using odec.Server.Model.Category;
using odec.Server.Model.User;
//using IAuctionRepo = odec.Auction.DAL.Interop.IAuctionRepository<int, System.Data.Entity.DbContext, odec.Server.Model.Auction.Auction, odec.Server.Model.Auction.AuctionVideo, odec.Server.Model.Auction.AuctionLayout, odec.Server.Model.Category.Category, odec.Server.Model.User.User, odec.Server.Model.Auction.AuctionBid, odec.Server.Model.Auction.Filters.AuctionFilter>;

namespace Auction.DAL.Tests
{

    public class AuctionTester : Tester<AuctionContext>
    {
        [Test]

        public void GetAuctions()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    var repository =
                        new AuctionRepository(db);
                    AuctionTestHelper.PopulateDefaultData(db);
                    var item = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    var cats = db.Set<Category>().ToList();

                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddCategory(item, category));
                    Assert.DoesNotThrow(() => repository.AddCategory(item2, cats.First()));

                    var user = new User
                    {
                        Id = 1
                    };
                    
                    Assert.DoesNotThrow(() => repository.MakeBid(user, item, 1000));
                    Assert.DoesNotThrow(() => repository.MakeBid(new User
                    {
                        Id = 2
                    }, item, 800));
                    Assert.Throws<InvalidOperationException>(() => repository.MakeBid(user, item, 300));
                }
                using (var db = new AuctionContext(options))
                {
                    var cats = db.Set<Category>().ToList();
                    var filter = new AuctionFilter
                    {
                        UserId = 1
                    };
                    IEnumerable<odec.Server.Model.Auction.Auction> result = null;
                    var repository =
                        new AuctionRepository(db);
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.UserId = null;
                    filter.InitialPriceInterval = new Interval<decimal?> { Start = 100 };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.InitialPriceInterval.End = 400;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.InitialPriceInterval.End = 200;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result == null || !result.Any());
                    filter.InitialPriceInterval = null;
                    filter.AuctionTypeId = 1;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.AuctionTypeId = null;
                    filter.Title = "My";
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.Title = null;
                    filter.StartDateInterval = new Interval<DateTime?> { Start = DateTime.Today.AddDays(-7) };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.StartDateInterval.End = DateTime.Today;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.StartDateInterval = null;
                    filter.MaxPriceInterval = new Interval<decimal?>
                    {
                        Start = 800
                    };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.MaxPriceInterval.End = 900;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result == null || !result.Any());
                    filter.MaxPriceInterval.End = 1000;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.MaxPriceInterval = null;

                    filter.Categories = cats;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any() && result.Count() == 2);
                    filter.Categories = null;
                    filter.UserId = 1;
                    filter.AuctionTypeId = 1;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.Title = "my";
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.InitialPriceInterval = new Interval<decimal?>
                    {
                        Start = 100
                    };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.InitialPriceInterval.End = 400;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.StartDateInterval = new Interval<DateTime?>
                    {
                        Start = DateTime.Today.AddDays(-7)
                    };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.StartDateInterval.End = DateTime.Today;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.MaxPriceInterval = new Interval<decimal?> { Start = 800 };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.MaxPriceInterval.End = 1100;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Any());
                    filter.Categories = cats;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));

                    Assert.True(result != null && result.Any() && result.Count() == 1);
                    filter = new AuctionFilter
                    {
                        EndDateInterval = new Interval<DateTime?>()
                        {
                           // End = DateTime.Today,
                        },
                        StateId = 1,
                        //AuctionTypeId = (int)AuctionTypes.Crafter,
                        Rows = 1000,
                        InitialPriceInterval = new Interval<decimal?>(),
                        MaxPriceInterval = new Interval<decimal?>(),
                        Categories = new List<Category>(),
                        StartDateInterval = new Interval<DateTime?>(),
                        AutoClosePriceInterval = new Interval<decimal?>()
                    };
                    Assert.DoesNotThrow(() => result = repository.Get(filter));

                    Assert.True(result != null && result.Any() && result.Count() > 1);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void MakeBid()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = new User
                    {
                        Id = 1
                    };
                    Assert.DoesNotThrow(() => repository.MakeBid(user, item, 1000));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveBid()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                       new AuctionRepository(db);

                    var item = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = new User
                    {
                        Id = 1
                    };
                    Assert.DoesNotThrow(() => repository.MakeBid(user, item, 1000));
                    Assert.DoesNotThrow(() => repository.RemoveBid(user, item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveBids()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                          new AuctionRepository(db);

                    var item = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = new User
                    {
                        Id = 1
                    };
                    Assert.DoesNotThrow(() => repository.MakeBid(user, item, 1000));
                    Assert.DoesNotThrow(() => repository.RemoveBids(item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetAuctionBids()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = new User
                    {
                        Id = 1
                    };
                    IEnumerable<AuctionBid> result = null;
                    Assert.DoesNotThrow(() => repository.MakeBid(user, item, 1000));
                    Assert.DoesNotThrow(() => result = repository.GetAuctionBids(item.Id));
                    Assert.True(result != null && result.Any());
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddCategory()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                          new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddCategory(item, new Category
                    {
                        Id = 1
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveCategory()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                         new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddCategory(item, new Category
                    {
                        Id = 1
                    }));
                    Assert.DoesNotThrow(() => repository.RemoveCategory(item, new Category
                    {
                        Id = 1
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetAuctionCategories()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddCategory(item, new Category
                    {
                        Id = 1
                    }));
                    IEnumerable<Category> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetAuctionCategories(item.Id));
                    Assert.True(result != null && result.Any());
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddVideo()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                        new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddVideo(item, new AuctionVideo
                    {
                        Title = "title",
                        Description = "description",
                        Video = GenerateAttachment(),
                        AuctionId = item.Id
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveVideo()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var video = new AuctionVideo
                    {
                        Title = "title",
                        Description = "description",
                        Video = GenerateAttachment(),
                        AuctionId = item.Id
                    };
                    Assert.DoesNotThrow(() => repository.AddVideo(item, video));
                    Assert.DoesNotThrow(() => repository.RemoveVideo(item, video));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetAuctionVideos()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var video = new AuctionVideo
                    {
                        Title = "title",
                        Description = "description",
                        Video = GenerateAttachment(),
                        AuctionId = item.Id
                    };
                    Assert.DoesNotThrow(() => repository.AddVideo(item, video));
                    IEnumerable<AuctionVideo> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetAuctionVideos(item.Id));
                    Assert.True(result != null && result.Any());
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddLayout()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                           new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddLayout(item, new AuctionLayout
                    {
                        Title = "title",
                        Description = "description",
                        Layout = GenerateAttachment(),
                        AuctionId = item.Id
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveLayout()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                          new AuctionRepository(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var layout = new AuctionLayout
                    {
                        Title = "title",
                        Description = "description",
                        Layout = GenerateAttachment(),
                        AuctionId = item.Id
                    };
                    Assert.DoesNotThrow(() => repository.AddLayout(item, layout));
                    Assert.DoesNotThrow(() => repository.RemoveLayout(item, layout));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetAuctionLayouts()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                         new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var layout = new AuctionLayout
                    {
                        Title = "title",
                        Description = "description",
                        Layout = GenerateAttachment(),
                        AuctionId = item.Id
                    };
                    Assert.DoesNotThrow(() => repository.AddLayout(item, layout));
                    IEnumerable<AuctionLayout> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetAuctionLayouts(item.Id));
                    Assert.True(result != null && result.Any());
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        private Attachment GenerateAttachment()
        {
            return new Attachment
            {
                Name = "Test",
                Code = "TEST",
                IsActive = true,
                DateCreated = DateTime.Now,
                SortOrder = 0,
                Extension = new Extension
                {
                    Name = "Test",
                    Code = "TEST",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    SortOrder = 0
                },
                PublicUri = string.Empty,
                IsShared = false,
                Content = new byte[] { 9, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 },
                AttachmentType = new AttachmentType
                {
                    Name = "Test",
                    Code = "TEST",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    SortOrder = 0
                }
            };
        }

        private odec.Server.Model.Auction.Auction GenerateModel()
        {
            return new odec.Server.Model.Auction.Auction
            {
                AuctionTypeId = 1,
                UserId = 1,
                StateId = 1,
                Name = "My Finished Work",
                Code = "My Finished Work",
                IsActive = true,
                Description = "LONLONNGLONGLONG description",
                DateCreated = DateTime.Now,
                SortOrder = 0,
                InitialPrice = 300,
                StartDate = DateTime.Today.AddDays(-7)

            };
        }

        [Test]
        public void Save()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository =
                          new AuctionRepository(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Delete()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeleteById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item.Id));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Deactivate()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Deactivate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeactivateById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }

                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Deactivate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Activate()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }


                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);
                    var item = GenerateModel();
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Activate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void ActivateById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }
                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Activate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
                }



            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new AuctionContext(options))
                {
                    AuctionTestHelper.PopulateDefaultData(db);
                }
                using (var db = new AuctionContext(options))
                {
                    var repository = new AuctionRepository(db);

                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => item = repository.GetById(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
                }


            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
    }
}
