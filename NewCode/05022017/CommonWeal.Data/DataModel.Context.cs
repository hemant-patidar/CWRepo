﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonWeal.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class CommonWealEntities : DbContext
    {
        public CommonWealEntities()
            : base("name=CommonWealEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AreaOfInterest> AreaOfInterests { get; set; }
        public DbSet<CityMaster> CityMasters { get; set; }
        public DbSet<CountryMaster> CountryMasters { get; set; }
        public DbSet<DonarDetail> DonarDetails { get; set; }
        public DbSet<DonationDetail> DonationDetails { get; set; }
        public DbSet<DonationRequest> DonationRequests { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<ImageHandler> ImageHandlers { get; set; }
        public DbSet<NGOPost> NGOPosts { get; set; }
        public DbSet<NGOUser> NGOUsers { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<RegistrationYear> RegistrationYears { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<Search> Searches { get; set; }
        public DbSet<SpamUser> SpamUsers { get; set; }
        public DbSet<StateMaster> StateMasters { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<WorkingArea> WorkingAreas { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
    
        //public virtual ObjectResult<PieChart_Result> PieChart(ObjectParameter nGOName, ObjectParameter totalDonationAmount)
        //{
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PieChart_Result>("PieChart", nGOName, totalDonationAmount);
        //}
    
        //public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    var versionParameter = version.HasValue ?
        //        new ObjectParameter("version", version) :
        //        new ObjectParameter("version", typeof(int));
    
        //    var definitionParameter = definition != null ?
        //        new ObjectParameter("definition", definition) :
        //        new ObjectParameter("definition", typeof(byte[]));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        //}
    
        //public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    var versionParameter = version.HasValue ?
        //        new ObjectParameter("version", version) :
        //        new ObjectParameter("version", typeof(int));
    
        //    var definitionParameter = definition != null ?
        //        new ObjectParameter("definition", definition) :
        //        new ObjectParameter("definition", typeof(byte[]));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        //}
    
        //public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        //}
    
        //public virtual ObjectResult<sp_helpdiagramdefinition_Result1> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result1>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        //}
    
        //public virtual ObjectResult<sp_helpdiagrams_Result1> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result1>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        //}
    
        //public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        //{
        //    var diagramnameParameter = diagramname != null ?
        //        new ObjectParameter("diagramname", diagramname) :
        //        new ObjectParameter("diagramname", typeof(string));
    
        //    var owner_idParameter = owner_id.HasValue ?
        //        new ObjectParameter("owner_id", owner_id) :
        //        new ObjectParameter("owner_id", typeof(int));
    
        //    var new_diagramnameParameter = new_diagramname != null ?
        //        new ObjectParameter("new_diagramname", new_diagramname) :
        //        new ObjectParameter("new_diagramname", typeof(string));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        //}
    
        //public virtual int sp_upgraddiagrams()
        //{
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        //}
    
        //public virtual ObjectResult<string> usp_GetAllData()
        //{
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("usp_GetAllData");
        //}
    
        //public virtual ObjectResult<usp_GetNGORequest_Result1> usp_GetNGORequest()
        //{
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetNGORequest_Result1>("usp_GetNGORequest");
        //}
    
        //public virtual ObjectResult<string> usp_GetPostDetails(Nullable<int> postID)
        //{
        //    var postIDParameter = postID.HasValue ?
        //        new ObjectParameter("PostID", postID) :
        //        new ObjectParameter("PostID", typeof(int));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("usp_GetPostDetails", postIDParameter);
        //}
    }
}
