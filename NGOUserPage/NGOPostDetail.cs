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
    
    public partial class NGOPostDetail
    {
        public int PostDetailID { get; set; }
        public int PostID { get; set; }
        public string EmailID { get; set; }
        public int RoleID { get; set; }
        public Nullable<bool> IsLike { get; set; }
        public string PostComment { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
