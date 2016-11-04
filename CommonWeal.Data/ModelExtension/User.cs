﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeal.Data
{
    [MetadataType(typeof(UserMeta))]
    public partial class User { }


    public partial class UserMeta

    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail")]
        [Required(ErrorMessage = "This field is required")]
        public string LoginEmailID { get; set; }
        //[RegularExpression(@"^(?=(.*\d){1})(.*\S)(?=.*[a-zA-Z\S])[0-9a-zA-Z\S]{8,}", ErrorMessage = "Password should be minimum 8  characters")]
        [Required(ErrorMessage = "This field is required")]
        public string LoginPassword { get; set; }
    }

}