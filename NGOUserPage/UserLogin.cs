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
    
    public partial class UserLogin
    {
        public UserLogin()
        {
            this.RegisteredUsers = new HashSet<RegisteredUser>();
            this.NGOUsers = new HashSet<NGOUser>();
        }
    
        public int LoginID { get; set; }
        public string LoginPassword { get; set; }
        public Nullable<int> LoginUserType { get; set; }
        public string LoginEmailID { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlock { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual ICollection<RegisteredUser> RegisteredUsers { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual ICollection<NGOUser> NGOUsers { get; set; }
    }
}
