//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class PostLike
    {
        public int LikeID { get; set; }
        public int LoginID { get; set; }
        public Nullable<bool> IsLike { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> PostID { get; set; }
    
        public virtual NGOPost NGOPost { get; set; }
    }
}
