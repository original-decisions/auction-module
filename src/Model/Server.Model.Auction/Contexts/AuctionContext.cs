using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using odec.Server.Model.Auction.Abst.Interfaces;
using odec.Server.Model.User;
using Usr = odec.Server.Model.User.User;
namespace odec.Server.Model.Auction.Contexts
{
    public class AuctionContext: DbContext, IAuctionContext<Auction,AuctionType,AuctionSkill,AuctionLayout,AuctionVideo,AuctionBid>
    {
        private string AuctionScheme = "auction";
        private string MembershipScheme = "AspNet";
        private string ConversationScheme = "conv";
        private string AttachmentScheme = "attach";
        private string CategoryScheme = "dbo";
        public AuctionContext(DbContextOptions<AuctionContext> options)
            : base(options)
        {

        }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionSkill> AuctionSkills { get; set; }
        public DbSet<AuctionLayout> AuctionVideos { get; set; }
        public DbSet<AuctionVideo> AuctionLayouts { get; set; }
        public DbSet<AuctionBid> AuctionBids { get; set; }
        public DbSet<AuctionType> AuctionTypes { get; set; }

        public DbSet<AuctionState> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            string scheme = String.Empty;
            //TODO: add get from configuration from context
            if (string.IsNullOrEmpty(scheme))
            {
                scheme = AuctionScheme;
            }
            modelBuilder.Entity<Usr>().ToTable("Users", MembershipScheme);
            modelBuilder.Entity<Role>().ToTable("Roles", MembershipScheme);
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles", MembershipScheme)
                .HasKey(it => new { it.UserId, it.RoleId });
            modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);

            modelBuilder.Entity<Auction>()
                .ToTable("Auctions", AuctionScheme);
            modelBuilder.Entity<AuctionSkill>()
                .ToTable("AuctionSkills", AuctionScheme)
                .HasKey(it=>new {it.AuctionId,it.CategoryId});
            modelBuilder.Entity<AuctionLayout>()
                .ToTable("AuctionLayouts", AuctionScheme)
                .HasKey(it => new { it.AuctionId, it.LayoutId }); ;

            modelBuilder.Entity<AuctionVideo>()
                .ToTable("AuctionVideos", AuctionScheme)
                .HasKey(it => new { it.AuctionId, it.VideoId }); ;
            modelBuilder.Entity<AuctionBid>()
                .ToTable("AuctionBids", AuctionScheme)
                .HasKey(it => new { it.AuctionId, it.UserId }); ;
            modelBuilder.Entity<AuctionType>()
                .ToTable("AuctionTypes", AuctionScheme);
            //todo: it might be a good idea to do separate ctx for auctions with states
            modelBuilder.Entity<AuctionState>()
                .ToTable("States",AuctionScheme);
            modelBuilder.Entity<Conversation.Conversation>()
                .ToTable("Conversations", ConversationScheme);
            modelBuilder.Entity<Conversation.ConversationType>()
                .ToTable("ConversationTypes", ConversationScheme);
            modelBuilder.Entity<Category.Category>()
                .ToTable("Categories", CategoryScheme);

            modelBuilder.Entity<Attachment.Attachment>().ToTable("Attachments", AttachmentScheme);
            modelBuilder.Entity<AttachmentType>().ToTable("AttachmentTypes", AttachmentScheme);
            modelBuilder.Entity<Extension>().ToTable("Extensions", AttachmentScheme);
            modelBuilder.Entity<AttachmentTypeExtension>().ToTable("AttachmentTypeExtentions", AttachmentScheme)
                .HasKey(it => new { it.AttachmentTypeId, it.ExtensionId});
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}
