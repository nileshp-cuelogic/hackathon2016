using ErrorLoggerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private ErrorLoggerEntities db = new ErrorLoggerEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            var lstApplication = new List<SelectListItem>();
            var lstModule = new List<SelectListItem>();

            var model = new Dashboard();

            var UserId = db.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Id;
            var IsSuperAdmin = HttpContext.User.IsInRole("Super Admin");
            var applications = db.Applications.Where(x => (IsSuperAdmin || x.UserId == UserId) && x.IsActive == true).Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });

            foreach (var item in applications)
            {
                lstApplication.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            model.ApplicationList = lstApplication;

            model.ModuleList = lstModule;


            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadApplicationModules(int applicationId)
        {
            var ModuleList = db.ErrorLogs.Where(x => x.ApplicationId == applicationId).ToList();

            var ModuleData = ModuleList.Select(m => new SelectListItem()
            {
                Text = m.ModuleName,
                Value = m.ModuleName,
            });

            return Json(ModuleData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadErrorSummary(int applicationId, string moduleName)
        {
            var UserId = db.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Id;

            var IsSuperAdmin = HttpContext.User.IsInRole("Super Admin");

            var ErrorCount = (from a in db.ErrorLogs
                              join c in db.Applications on a.ApplicationId equals c.Id
                              where c.IsActive == true
                              && (IsSuperAdmin || c.UserId == UserId)
                              && (applicationId == 0 || c.Id == applicationId)
                              && (String.IsNullOrEmpty(moduleName) || a.ModuleName == moduleName)
                              select a.ModuleName).Count();

            var ApplicationErrors = from a in db.ErrorLogs
                                    join c in db.Applications on a.ApplicationId equals c.Id
                                    where c.IsActive == true
                                    && (IsSuperAdmin || c.UserId == UserId)
                                    && (applicationId == 0 || c.Id == applicationId)
                                    && (String.IsNullOrEmpty(moduleName) || a.ModuleName == moduleName)
                                    group c by new { c.Id, c.Name } into g
                                    select new { ApplicationId = g.Key.Id, ApplicatioName = g.Key.Name, ErrorCount = g.Count() };

            var ApplicationModuleErrors = from a in db.ErrorLogs
                                          join c in db.Applications on a.ApplicationId equals c.Id
                                          where c.IsActive == true && (IsSuperAdmin || c.UserId == UserId)
                                          && (applicationId == 0 || c.Id == applicationId)
                                          && (String.IsNullOrEmpty(moduleName) || a.ModuleName == moduleName)
                                          group c by new { c.Id, c.Name, a.ModuleName } into g
                                          select new { ApplicationId = g.Key.Id, ApplicatioName = g.Key.Name, ModuleName = g.Key.ModuleName, ErrorCount = g.Count() };


            return Json(new { ErrorCount = ErrorCount, ApplicationErrors = ApplicationErrors, ApplicationModuleErrors = ApplicationModuleErrors }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
