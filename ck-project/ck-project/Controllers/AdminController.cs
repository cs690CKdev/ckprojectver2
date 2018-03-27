using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        ckdatabase db = new ckdatabase();
        private static List<lead> result = new List<lead>();


        public ActionResult AdminMainPage(string search, string searchby)
        {
            try
            {
                return RedirectToAction("ListLead", "Lead");
            }

            catch (Exception e)
            {
                ViewBag.m = " Something went wrong ... " + e.Message;
                return RedirectToAction("ListLead", "Lead");
            }
        }

        public ActionResult Sort(string by)
        {
            List<lead> pro = new List<lead>();
            switch (by)
            {
                case "pn":
                    pro = AdminController.result.OrderBy(a => a.project_name).ToList();
                    break;
                case "cu":
                    pro = AdminController.result.OrderBy(a => a.customer.customer_firstname).ToList();
                    break;
                case "pt":
                    pro = AdminController.result.OrderBy(a => a.project_type_number).ToList();
                    break;
                case "cd":
                    pro = AdminController.result.OrderBy(a => a.lead_date).ToList();
                    break;
                case "lmd":
                    pro = AdminController.result.OrderBy(a => a.Last_update_date).ToList();
                    break;
                case "de":
                    pro = AdminController.result.OrderBy(a => a.employee.emp_firstname).ToList();
                    break;

            }

            return View("AdminMainPage", pro);
        }
    }
}