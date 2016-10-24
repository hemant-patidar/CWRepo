//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NGOUserPage
{
    using System;
    using System.Collections.Generic;
    
    public partial class NGOUser
    {
        public int NGOUserId { get; set; }
        public string UniqueuId { get; set; }
        public string NGOName { get; set; }
        public string NGOEmailID { get; set; }
        public string NGOPassword { get; set; }
        public Nullable<int> LoginID { get; set; }
        public string NGOKey { get; set; }
        public string NGOProfilePic { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string NGOAddress { get; set; }
        public string ChairmanName { get; set; }
        public string ChairmanID { get; set; }
        public string ParentOrganisation { get; set; }
        public string RegisteredWith { get; set; }
        public string RegistrationNumber { get; set; }
        public string CityOfRegistration { get; set; }
        public Nullable<System.DateTime> DateOfRegistration { get; set; }
        public string RegistrationProof { get; set; }
        public string FCRANumber { get; set; }
        public string AreaOfIntrest { get; set; }
        public string OperationalArea { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlock { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> LoginUserType { get; set; }
    
        public virtual UserLogin UserLogin { get; set; }
    }
}
