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
    
    public partial class PostCategory
    {
        public int PostcategoryID { get; set; }
        public int PostID { get; set; }
        public int CategoryID { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual AreaOfInterest AreaOfInterest { get; set; }
        public virtual NGOPost NGOPost { get; set; }
    }
}
