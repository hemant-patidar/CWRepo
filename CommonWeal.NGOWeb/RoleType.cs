//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonWeal.NGOWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoleType
    {
        public RoleType()
        {
            this.UserLogins = new HashSet<UserLogin>();
        }
    
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}