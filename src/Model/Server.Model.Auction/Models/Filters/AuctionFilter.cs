using System;
using System.Collections.Generic;
using odec.Framework.Generic;
using odec.Framework.Generic.Utility;
using CategoryE = odec.Server.Model.Category.Category;
namespace odec.Server.Model.Auction.Filters
{
    public class AuctionFilter: FilterBase
    {
        public AuctionFilter()
        {
            Rows = 20;
        }
        public string Title { get; set; }
        public IEnumerable<CategoryE> Categories { get; set; }
        /// <summary>
        /// Диапазон в который попадает дата начала.
        /// </summary>
        public Interval<DateTime?> StartDateInterval { get; set; }
        public Interval<DateTime?> EndDateInterval { get; set; }
        public Interval<decimal?> InitialPriceInterval { get; set; }
        public Interval<decimal?> AutoClosePriceInterval { get; set; }
        public Interval<decimal?> MaxPriceInterval { get; set; }

        ///// <summary>
        ///// Current
        ///// </summary>
        //public Interval<decimal?> CurrentPrice { get; set; }
        public int? AuctionTypeId { get; set; }
        public int? StateId { get; set; }
        public int? UserId { get; set; }
    }
}