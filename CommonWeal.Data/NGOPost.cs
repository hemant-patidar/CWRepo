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
    
    public partial class NGOPost
    {
        public int PostID { get; set; }
        public Nullable<int> LoginID { get; set; }
        public Nullable<System.DateTime> PostDateTime { get; set; }
        public string PostType { get; set; }
        public string PostContent { get; set; }
        public string PostUrl { get; set; }
        public Nullable<int> PostLikeCount { get; set; }
        public Nullable<int> PostCommentCount { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
