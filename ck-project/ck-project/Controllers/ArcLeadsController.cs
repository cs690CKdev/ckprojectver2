using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace ck_project.Controllers
{
    [Authorize]
    public class ArcLeadsController : Controller
    {
       

        //Creating the DB connecton
        ckdatabase db = new ckdatabase();


        // GET: ArcLeads
        public ActionResult ListArcLead(int? page, string search = null, string Start = null, string end = null, string msg = null)
        {
           
            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);

            return View(db.archive_leads.Where(x => (x.project_name.Contains(search) || search == null)
                && (x.lead_date >= start && x.lead_date <= end2)).ToList().ToPagedList(page ?? 1, 8));


        }


        public ActionResult Details(int id)
        {
            return View(db.archive_leads.Where(x => x.lead_number == id).ToList());
        }


    }
    }
