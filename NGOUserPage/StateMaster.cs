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
    
    public partial class StateMaster
    {
        public StateMaster()
        {
            this.CityMasters = new HashSet<CityMaster>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> CountryID { get; set; }
    
        public virtual ICollection<CityMaster> CityMasters { get; set; }
        public virtual CountryMaster CountryMaster { get; set; }
    }
}
