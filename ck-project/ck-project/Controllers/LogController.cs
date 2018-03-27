using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;



using System.Data.SqlClient;

using PagedList.Mvc;

namespace ck_project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LogController : Controller
    {
        // GET: Log
        ckdatabase db = new ckdatabase();
        static List<tax> lst = new List<tax>();
        static List<rate> ratelst = new List<rate>();
        //string Emp = null, 

  /*      
        public ActionResult StateTax()
        {

            try
            {
                
                var state = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                state.AddRange(db.taxes.Where(CCVV => CCVV.state != null).Select(b => new SelectListItem

                {
                    Text = b.state,
                    Selected = true,
                    Value = b.state.ToString()
                }).Distinct());
                ViewBag.state = state;
 
                var result = db.taxes.Where(l => ((l.state=="Type") && l.tax_anme == "State Tax")).ToList(); 
                //LogController.lst = result;
                return View();

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }

        */


        public ActionResult StateTax(int? page, string search = null, string Type = null, string Start = null, string end = null, string msg = null)
        {


            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>();

                ClassInfo.AddRange(db.taxes.Where(CCVV => CCVV.state != null).Select(b => new SelectListItem

                {
                    Text = b.state,
                    Selected = true,
                    Value = b.state.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;



                
                var result = db.taxes.Where(l => (l.state == Type)  && l.tax_anme == "State Tax" ).ToList();
                //LogController.lst = result;
               
               return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }






        [AcceptVerbs(HttpVerbs.Post)]






      /*  public ActionResult StateTax(FormCollection form)
        {

            try
            {
                //setting dropdown list for forgern key
                var state = new List<SelectListItem>();
                state.AddRange(db.taxes.Where(x => x.state != null).Select(a => new SelectListItem
                {
                    Text = a.state,
                    Selected = false,
                    Value = a.state.ToString()
                }).Distinct());

                //setting variable passing
                ViewBag.state = state;
              
                //create instance
                tax target = new tax();
                //get property
                TryUpdateModel(target, new string[] { "tax_value", "tax_anme", "city", "state", "county", "zipcode", "end_date", "tax_number", "start_date" }, form.ToValueProvider());

                //validate
                //  double tempPhoneNumber = 0;
                ViewBag.Error = null;

                
                {
                    target.tax_anme = "State Tax";
                    target.city = "";
                    target.county = "";
                    target.zipcode = "";
                    
                    target.start_date = System.DateTime.Now;
                    target.end_date = System.DateTime.Now;

                    db.taxes.Add(target);
                    db.SaveChanges();
                    ViewBag.m = "Tax was successfully updated " + "on " + System.DateTime.Now;
                }
                return View(target);




            }
                  

                catch (Exception e)
                {
                    ViewBag.m = "The customer was not created ... " + e.Message;
                    return View();
                }

           

        }


    */


        

        public ActionResult CountyTax(int? page, string search = null, string Type = null, string Type1 = null, string Start = null, string end = null, string msg = null)
        {


            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>();
               
                ClassInfo.AddRange(db.taxes.Where(CCVV => CCVV.state != null).Select(b => new SelectListItem

                {
                    Text = b.state,
                    Selected = true,
                    Value = b.state.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;



                var ClassInfo1 = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo1.AddRange(db.taxes.Where(CCVV => CCVV.county != null && CCVV.county != "" && CCVV.state == Type).Select(b => new SelectListItem

                {
                    Text = b.county,
                    Selected = true,
                    Value = b.county.ToString()
                }).Distinct());
                ViewBag.lead_type_county = ClassInfo1;
                var result = db.taxes.Where(l => (l.state == Type) && (l.county == Type1) && l.tax_anme == "County Tax" && l.state == Type).ToList();
                //LogController.lst = result;

                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }


        public ActionResult CityTax(int? page, string search = null, string Type = null, string Type1 = null, string Start = null, string end = null, string msg = null)
        {


            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo.AddRange(db.taxes.Where(CCVV => CCVV.state != null).Select(b => new SelectListItem

                {
                    Text = b.state,
                    Selected = true,
                    Value = b.state.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;



                var ClassInfo1 = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo1.AddRange(db.taxes.Where(CCVV => CCVV.city != null && CCVV.city != "" && CCVV.state == Type).Select(b => new SelectListItem

                {
                    Text = b.city,
                    Selected = true,
                    Value = b.city.ToString()
                }).Distinct());
                ViewBag.lead_type_city = ClassInfo1;
                var result = db.taxes.Where(l => (l.state==Type) && (l.city==Type1) && l.tax_anme == "City Tax" && l.state == Type).ToList();


                LogController.lst = result;
                
                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }









        public ActionResult BOTax(int? page, string search = null, string Type = null, string Type1 = null, string Start = null, string end = null, string msg = null)
        {


            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo.AddRange(db.taxes.Where(CCVV => CCVV.state != null).Select(b => new SelectListItem

                {
                    Text = b.state,
                    Selected = true,
                    Value = b.state.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;



                var ClassInfo1 = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo1.AddRange(db.taxes.Where(CCVV => CCVV.city != null && CCVV.city != "" && CCVV.state == Type).Select(b => new SelectListItem

                {
                    Text = b.city,
                    Selected = true,
                    Value = b.city.ToString()
                }).Distinct());
                ViewBag.lead_type_bo = ClassInfo1;
                var result = db.taxes.Where(l => (l.state==Type) && (l.city==Type1) && l.tax_anme == "B&O Tax" && l.state == Type).ToList();


                LogController.lst = result;
                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }

























        public ActionResult Details(int id)
        {
            return View(db.taxes.Where(x => x.tax_number == id).ToList());
        }
        public ActionResult InCity(int? page, string search = null, string Type = null, string Start = null, string end = null, string msg = null)
        {
            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo.AddRange(db.rates.Where(CCVV => CCVV.rate_name != null).Select(b => new SelectListItem

                {
                    Text = b.rate_name,
                    Selected = true,
                    Value = b.rate_name.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;

                var result = db.rates.Where(l => ((l.rate_name==Type) )).ToList();


                LogController.ratelst = result;
                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }






        public ActionResult OutCity(int? page, string search = null, string Type = null, string Start = null, string end = null, string msg = null)
        {
            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;


            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem>
                {
                    //  new SelectListItem() { Text = "WV", Selected = true, Value = "" }
                };
                ClassInfo.AddRange(db.rates.Where(CCVV => CCVV.rate_name != null).Select(b => new SelectListItem

                {
                    Text = b.rate_name,
                    Selected = true,
                    Value = b.rate_name.ToString()
                }).Distinct());
                ViewBag.lead_type = ClassInfo;

                var result = db.rates.Where(l => ((l.rate_name==Type))).ToList();


                LogController.ratelst = result;
                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }



        public ActionResult ListLog(int? page, string search, string Emp, string Start, string end)
        {
            DateTime startDt = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime endDt = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            endDt = endDt.Date + ts;


            try
            {

                var EmpInfo = new List<SelectListItem>();
                EmpInfo.AddRange(db.employees.Select(b => new SelectListItem
                {
                    Text = b.emp_username,
                    Selected = false,
                    Value = b.emp_number.ToString()
                }));
                ViewBag.EmpInfo = EmpInfo;

                string employeeUserName = null;
                if (!string.IsNullOrEmpty(Emp))
                {
                    int employeeNbr = Int32.Parse(Emp);
                    var employee = db.employees.Where(e => e.emp_number == employeeNbr).First();
                    employeeUserName = employee.emp_username;
                }


                var result = db.lead_log_file.Where(l => l.update_date >= startDt && l.update_date <= endDt
                                                    && (string.IsNullOrEmpty(search) || l.lead.project_name.Contains(search))
                                                    && (string.IsNullOrEmpty(employeeUserName) || l.emp_username == employeeUserName)).ToList();


                return View(result.ToPagedList(page ?? 1, 8));
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View(new List<lead_log_file>().ToPagedList(page ?? 1, 8));
            }
        }


        public ActionResult Index()
        {
            return RedirectToAction("ListLog", "Log");
        }
        [HttpPost]
        public ActionResult Vlog(FormCollection fo, string order = "")
        {
            ViewBag.order = String.IsNullOrEmpty(order) ? "name" : "";
            ViewBag.date = order == "date" ? "date_d" : "date";
            //default return top 30 changes sort by time
            var logs = from l in db.lead_log_file select l;
            switch (order)
            {
                case "name":
                    logs = from nlog in logs where (nlog.emp_username == fo["name"]) select nlog;
                    break;
                case "date":
                    logs = logs.OrderByDescending(e => e.update_date);
                    break;
                case "date_d":
                    logs = logs.OrderBy(w => w.update_date);
                    break;
                default:
                    logs = logs.OrderBy(t => t.update_date).Take(30);
                    break;
            }
            return View(logs.ToList());
        }

        public ActionResult Vlog()
        {
            return View();
        }
    }

}
