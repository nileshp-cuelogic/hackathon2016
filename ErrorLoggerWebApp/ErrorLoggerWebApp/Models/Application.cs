//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErrorLoggerWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public partial class Application
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Application Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email Id")]
        [EmailAddress]
        public string ErrorEmail { get; set; }


        [DisplayName("Email Alerts Required")]
        public bool AlertsRequired { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
