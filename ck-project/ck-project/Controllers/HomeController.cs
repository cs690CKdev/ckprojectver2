using ck_project.Helpers;
using ck_project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static List<lead> result=new List<lead>();
        ckdatabase db = new ckdatabase();
        public ActionResult Index()
        {
            return RedirectToAction("LogOffSession", "Account");
        }

        public ActionResult MainPage(int? page, string name, string status, string start, string end)
        {
            var projStatusList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "All", Selected = true, Value = "" }
            };

            projStatusList.AddRange(db.project_status.Where(ps => ps.project_status_name != Constants.proj_Status_Closed).Select(b => new SelectListItem
            {
                Text = b.project_status_name,
                Selected = false,
                Value = b.project_status_number.ToString()
            }));
            ViewBag.projStatusList = projStatusList;

            var identity = (ClaimsIdentity)User.Identity;
            var currUserIDStr = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                int currUserID = Int32.Parse(currUserIDStr);
                DateTime startDt = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.Parse(start);
                DateTime endDt = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
                TimeSpan ts = new TimeSpan(23, 59, 59);
                endDt = endDt.Date + ts;

                int statusNbr = 0;
                if (!string.IsNullOrEmpty(status))
                {
                    statusNbr = Int32.Parse(status);
                }

                var result = db.leads.Where(l => l.emp_number == currUserID && l.deleted == false
                                                && l.project_status.project_status_name != Constants.proj_Status_Closed
                                                && l.lead_date >= startDt && l.lead_date <= endDt
                                                && (string.IsNullOrEmpty(status) || l.project_status_number == statusNbr)
                                                && (string.IsNullOrEmpty(name) || l.project_name.Contains(name))).ToList();

                HomeController.result = result;
                return View(result.ToPagedList(page ?? 1, 10));
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return View(new List<lead>().ToPagedList(page ?? 1, 10));
            }
        }

        public ActionResult ProjPrint(int id)
        {
            // only recalculate if lead is not close
            if (id != 0)
            {
                var lead = db.leads.Where(l => l.lead_number == id).First();
                if (lead != null && !lead.project_status.project_status_name.Equals(Constants.proj_Status_Closed, StringComparison.OrdinalIgnoreCase))
                {
                    new GeneralHelper().SaveProjectTotal(lead.lead_number);
                }
                ViewBag.lead = lead;
            }

            ViewBag.leadNumber = id;
            return View();
        }

        public ActionResult ProjSummary(int id)
        {
            var projSummary = new ProjectSummary();
            if (id != 0)
            {
                var lead = db.leads.Where(l => l.lead_number == id).First();

                if (lead != null)
                {
                    // only recalculate if lead is not close
                    if (!lead.project_status.project_status_name.Equals(Constants.proj_Status_Closed, StringComparison.OrdinalIgnoreCase))
                    {
                        new GeneralHelper().SaveProjectTotal(lead.lead_number);
                    }

                    lead = db.leads.Where(l => l.lead_number == id).First();
                    ProjSummaryHelper projSummaryHelper = new ProjSummaryHelper();
                    if (db.total_cost.Where(c => c.lead_number == id).Any())
                    {
                        projSummary.TotalCost = db.total_cost.Where(c => c.lead_number == id).First();
                    }
                    projSummary = projSummaryHelper.CalculateInstallCategoryCostMap(lead, projSummary);
                    projSummary = projSummaryHelper.CalculateInstallationsData(lead, projSummary);
                    projSummary = projSummaryHelper.GetProductCategoryList(lead, projSummary);
                    projSummary = projSummaryHelper.GetProductTotalMap(lead, projSummary);
                    projSummary = projSummaryHelper.SetCustomerData(lead, projSummary);
                    projSummary = projSummaryHelper.SetAddresses(lead, projSummary);
                    projSummary.Lead = lead;
                }
            }

            return View(projSummary);
        }

        public ActionResult Sort(int? page, string by) {
            List<lead> sortedList = new List<lead>();
            switch (by)
            {
                case "pn":  //project name
                    sortedList = HomeController.result.OrderBy(a => a.project_name).ToList();
                        break;
                case "cu":  //customer
                    sortedList = HomeController.result.OrderBy(a => a.customer.customer_firstname).ToList();
                    break;
                case "pt":  //project type
                    sortedList = HomeController.result.OrderBy(a => a.project_type_number).ToList();
                    break;
                case "ps":  //project status
                    sortedList = HomeController.result.OrderBy(a => a.project_status_number).ToList();
                    break;
                case "sp":  //salesperson
                    sortedList = HomeController.result.OrderBy(a => a.lead_creator).ToList();
                    break;
                case "cd":  //created date
                    sortedList = HomeController.result.OrderBy(a => a.lead_date).ToList();
                    break;
                case "lu":  //last updated date
                    sortedList = HomeController.result.OrderBy(a => a.Last_update_date).ToList();
                    break;
            }

            var projStatusList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "All", Selected = true, Value = "" }
            };

            projStatusList.AddRange(db.project_status.Where(ps => ps.project_status_name != Constants.proj_Status_Closed).Select(b => new SelectListItem
            {
                Text = b.project_status_name,
                Selected = false,
                Value = b.project_status_number.ToString()
            }));
            ViewBag.projStatusList = projStatusList;

            return View("MainPage", sortedList.ToPagedList(page ?? 1, 10));
        }
    }
}