using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerWebApp.Models
{
    public class Dashboard
    {
        public List<SelectListItem> ApplicationList { get; set; }
        public List<SelectListItem> ModuleList { get; set; }

        public int ApplicationId { get; set; }
        public string ModuleValue { get; set; }
    }
}