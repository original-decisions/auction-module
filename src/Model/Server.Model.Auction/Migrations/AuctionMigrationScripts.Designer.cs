﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace odec.CP.Server.Model.Auction.Migrations {
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    public class AuctionMigrationScripts {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        internal AuctionMigrationScripts() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("odec.CP.Server.Model.Auction.Migrations.AuctionMigrationScripts", typeof(AuctionMigrationScripts).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to  begin tran
        ///IF schema_id(&apos;auction&apos;) IS NULL
        ///    EXECUTE(&apos;CREATE SCHEMA [auction]&apos;)
        ///	IF  NOT EXISTS (SELECT * FROM sys.objects 
        ///	WHERE object_id = OBJECT_ID(N&apos;[auction].[AuctionBids]&apos;) AND type in (N&apos;U&apos;))
        ///	begin
        ///CREATE TABLE [auction].[AuctionBids] (
        ///    [AuctionId] [int] NOT NULL,
        ///    [UserId] [int] NOT NULL,
        ///    [Value] [decimal](18, 2) NOT NULL,
        ///    [Description] [nvarchar](max) NOT NULL,
        ///	[EstimatedStartDate] [datetime] null,
        ///	[EstimatedEndDate] [datetime] null,
        ///    [IsSelected] [bit] NOT NUL [rest of string was truncated]&quot;;.
        /// </summary>
        public static string AuctionInitial {
            get {
                return ResourceManager.GetString("AuctionInitial", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to  begin tran
        ///if not exists( select top 1 1 from auction.States where code like &apos;CLOSED&apos;)
        ///begin
        ///	INSERT INTO auction.States (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
        ///	values (&apos;Closed&apos;,&apos;CLOSED&apos;,1,0,GetDate(),GetDate())
        ///end
        ///if not exists( select top 1 1 from auction.States where code like &apos;OPEN&apos;)
        ///begin
        ///	INSERT INTO auction.States (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
        ///	values (&apos;Open&apos;,&apos;OPEN&apos;,1,0,GetDate(),GetDate())
        ///end
        ///if not exists( select top 1 1 from auction.States wher [rest of string was truncated]&quot;;.
        /// </summary>
        public static string AuctionInitialSeed {
            get {
                return ResourceManager.GetString("AuctionInitialSeed", resourceCulture);
            }
        }
    }
}
