using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult CompanyLeadSourceAnalysis()
        {
            return View();
        }
        public ActionResult CompanyProjectTypeAnalysis()
        {
            return View();
        }
        public ActionResult CompanyLeadStatusAnalysis()
        {
            return View();
        }
        public ActionResult OtherReports()
        {
            return View();
        }

    }
}