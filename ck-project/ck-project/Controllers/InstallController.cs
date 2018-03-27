using ck_project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    [Authorize]
    public class InstallController : Controller
    { // GET: Install
        ckdatabase db = new ckdatabase();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lis(int lid,string msg=null)
        {

            ViewBag.msg = msg;
            installation target = db.installations.Where(a => a.lead_number == lid).FirstOrDefault();
            ViewBag.mainc = new List<SelectListItem>
            {
               new SelectListItem{ Text="Kitchen",Value="kitchen"},
               new SelectListItem{ Text="Framing",Value="Framing"},
               new SelectListItem{ Text="Doors&Windows",Value="Doors_Windows"},
               new SelectListItem{ Text="Mechanicals",Value="MECHANICALS"},
               new SelectListItem{ Text="Electrical",Value="Electrical"},
               new SelectListItem{ Text="Finish",Value="Finish"},
               new SelectListItem{ Text="Cabinetry",Value="Cabinetry"},
               new SelectListItem{ Text="Countertops",Value="Countertops"},
               new SelectListItem{ Text="Appliance/Fixture Install",Value="Appliance"},
               new SelectListItem{ Text="GENERAL",Value="GENERAL"},
               new SelectListItem{ Text="BATH",Value="BATH"},
               new SelectListItem{ Text="MISC",Value="MISC"}
            };

            

            ViewBag.kitchen = new List<SelectListItem>
            {
                new SelectListItem{ Text="Appliance & Sink Demo",Value="asd"},
                 new SelectListItem{ Text="Cabinet & Top Demo",Value="ctd"},
                  new SelectListItem{ Text="Millwork Demo",Value="md"},
                   new SelectListItem{ Text="Floor - Ceiling - Wall Covering Demo",Value="fcwcd"},
                    new SelectListItem{ Text="Plumbing Demo",Value="pd"},
                     new SelectListItem{ Text="Electrical Demo",Value="ed"},
                      new SelectListItem{ Text="HVAC Demo",Value="hd"},
                       new SelectListItem{ Text="Windows & Door Demo",Value="wdd"},
                        new SelectListItem{ Text="Wood Framing Demo",Value="wfd"},
                         new SelectListItem{ Text="Misc Demo",Value="mid"}
            };

            ViewBag.framing = new List<SelectListItem> {
                new SelectListItem{ Text="Wall Framing",Value="WF"},
                new SelectListItem{ Text="Ceiling Framing",Value="CF"},
                new SelectListItem{ Text="Floor Framing",Value="FF"},
                new SelectListItem{ Text="Insulation",Value="ins"}
            };

            ViewBag.doorswindows = new List<SelectListItem> {
                new SelectListItem{Text="Doors",Value="Doors" },
                new SelectListItem{Text="Windows",Value="Windows" }
            };

            ViewBag.mech = new List<SelectListItem> {
                new SelectListItem{Text="Plumbing (Rough-In)",Value="Plumbing" },
                new SelectListItem{Text="HVAC",Value="hvac" }
            };

            ViewBag.elect = new List<SelectListItem> {
                new SelectListItem{Text="Wiring/Devices",Value="WD" },
                new SelectListItem{Text="Lighting",Value="lighting" }
            };

            ViewBag.finish = new List<SelectListItem> {
                new SelectListItem{Text="Ceiling and Walls",Value="caw" },
                new SelectListItem{Text="Flooring",Value="flooring" },
                new SelectListItem{Text="Millwork",Value="millwork" }
            };

            ViewBag.cabin = new List<SelectListItem> {
                new SelectListItem{Text="Cabinetry & Trim",Value="ct" }
            };

            ViewBag.counter = new List<SelectListItem> {
                new SelectListItem{Text="Countertop Installation",Value="CI" }
            };

            ViewBag.appliance = new List<SelectListItem> {
                new SelectListItem{ Text="Appliances And Fixtures Installation",Value="app"}
            };
            //372
            ViewBag.tbd = new List<SelectListItem> {
                new SelectListItem{ Text="Granite & Solid Surface Tops",Value="gsst"},
                new SelectListItem{Text="General",Value="General" }
            };

            ViewBag.bath = new List<SelectListItem> {
                new SelectListItem{ Text="Demo",Value="demo"},
                new SelectListItem{ Text="Wiring/Devices",Value="wd"},
                new SelectListItem{ Text="Walls/Finishes",Value="wf"},
                new SelectListItem{ Text="Flooring",Value="flooring"},
                new SelectListItem{ Text="Plumbing",Value="plumbing"},
                new SelectListItem{Text="Cabinetry & Trim",Value="ct" },
                new SelectListItem{Text="Countertop Installation",Value="CI" },
                new SelectListItem{Text="Lighting & Fixtures",Value="lighting" },
                new SelectListItem{Text="Millwork",Value="Millwork" }
            };

            ViewBag.misc = new List<SelectListItem> {
                new SelectListItem{ Text="Disclaimers",Value="disclaimers"}
            };
            List<employee> emplist = db.employees.Where(x => x.deleted == false).ToList();
            List<SelectListItem> al = new List<SelectListItem>();
                foreach (employee e in emplist) {
                al.Add(new SelectListItem { Text = e.emp_firstname + " " + e.emp_lastname, Value = e.emp_firstname });
            }
            ViewBag.emplist = al;


            //no installation yet
            //if ( target==null) {

            //    target = init(lid);

            //}
            var lead = db.leads.Where(l => l.lead_number == lid).First();
            ViewBag.lid = lid;
            ViewBag.customerNbr = lead.customer_number;

            return View(target);


        }

        public ActionResult custT(int iid)
        {
            List<tasks_installation> result = db.tasks_installation.Where(s => s.installation_number == iid & s.task_number > 408).ToList();
            return View(result);

        }

        [HttpPost]
        public ActionResult cusT(FormCollection fo)
        {
            int lid = int.Parse(fo["lid"]);


            return RedirectToAction("lis", new { lid = lid });
        }

        public ActionResult AddJob(string maincat,string subcat,int insnum=0,int mode=0) {
            
            List<task> option = db.tasks.Where(x => x.special_task == false && x.task_main_cat == maincat && x.task_sub_cat == subcat).ToList();
            List<SelectListItem> taskname = new List<SelectListItem>();
            foreach (task a in option) {
                taskname.Add(new SelectListItem { Text = a.task_name, Value =a.task_number.ToString() });
            }
            ViewBag.taskname = taskname;
            ViewBag.insnum = insnum;
            ViewBag.mode = mode;
           


            return PartialView();

        }
        //new task handler
        [HttpPost]
        public ActionResult handler(FormCollection fo) {
            int iid = int.Parse(fo["installation_number"]);
            string  msg = "The task was created successfully at " + System.DateTime.Now; ;
            int lid = db.installations.Where(a => a.installation_number == iid).First().lead_number;
            lead idk = db.leads.Where(d => d.lead_number == lid).First();
            GeneralHelper helper = new GeneralHelper();
            tasks_installation target = new tasks_installation();
            if (int.Parse(fo["mode"]) == 0)
            {
                target.hours = double.Parse(fo["hours"]);
                target.task_number = int.Parse(fo["task_number"]);
                target.m_cost = double.Parse(fo["m_cost"]);
                target.labor_cost= helper.GetInstallationLaborRate(idk)* target.hours;
                target.material_retail_cost = helper.GetInstallationMaterialRate(idk) * target.m_cost;
                target.installation_number = iid;
                try
                {
                    db.tasks_installation.Add(target);
                    db.SaveChanges(lid);
                    //db.SaveChanges(lid, "create new");

                }
                catch (Exception e)
                {
                    msg = e.Message;
                }
            }
            else {
                
                    task ctask = new task();
                    ctask.special_task = true;
                    ctask.task_main_cat = "MISC";
                    ctask.task_sub_cat = "Disclaimers";
                    ctask.task_name = fo["task_name"];
                    db.tasks.Add(ctask);
                    

                    target.task = ctask;

                    target.hours = double.Parse(fo["hours"]);
                    target.m_cost = double.Parse(fo["m_cost"]);
                target.labor_cost = helper.GetInstallationLaborRate(idk) * target.hours;
                target.material_retail_cost = helper.GetInstallationMaterialRate(idk) * target.m_cost;
                db.tasks_installation.Add(target);
                    target.installation_number = iid;
                    try
                    {
                        db.SaveChanges(lid);
                }
                catch (Exception e)
                {
                    msg = e.Message;

                }
            }


            return RedirectToAction("lis", new { lid = lid,msg=msg });
        }

        public ActionResult readtask(int lid,string maincat,string subcat) {
            installation t = db.installations.Where(r => r.lead_number == lid).First();
            int tt = t.installation_number;
            List<int> check = db.tasks_installation.Where(v => v.installation_number == tt).Select(p => p.task_number).ToList();           
            
            if (check.Count>0)
            {   //install exist
                List<tasks_installation> ins = db.leads.Where(c => c.lead_number == lid).First().installations.First().tasks_installation.ToList();
                ins =ins.FindAll(x => x.task.task_main_cat == maincat && x.task.task_sub_cat == subcat).ToList();
                ViewBag.maincat = maincat;
                ViewBag.subcat = subcat;
                return PartialView(ins);
            }
            else {
                //install not exist
                List<tasks_installation> taskset = new List<tasks_installation>();
                return PartialView(taskset);
            }
        }
        //update task
        [HttpPost]
        public ActionResult savetask(FormCollection fo) {
            string msg = "The task was updated successfully at " + System.DateTime.Now; ;
           
            int tin =int.Parse( fo["item.tasks_installation_number"]);

            GeneralHelper helper = new GeneralHelper();
          
                int iid = db.tasks_installation.Where(c => c.tasks_installation_number == tin).First().installation_number;
                int lid = db.installations.Where(g => g.installation_number == iid).First().lead_number;
            lead idk = db.leads.Where(b => b.lead_number == lid).First();
                try
                {

                    tasks_installation target = db.tasks_installation.Where(f => f.tasks_installation_number == tin).First();
                    target.hours = double.Parse(fo["item.hours"]);
                    target.m_cost = double.Parse(fo["item.m_cost"]);
                target.labor_cost = helper.GetInstallationLaborRate(idk) * target.hours;
                target.material_retail_cost = helper.GetInstallationMaterialRate(idk) * target.m_cost;
                db.SaveChanges(lid);
                }
                catch (Exception e)
                {
                msg = "Sonething went wrong " + e.Message;
            }
                
            
            
               
                return RedirectToAction("lis", new { lid = lid, msg = msg });
            }
            
            
        

        public ActionResult Delete(int tin,int iid) {
            int lid = db.installations.Where(q => q.installation_number == iid).First().lead_number;
            string msg = "The task was deleted successfully at " + System.DateTime.Now; ;
            try
            {
                tasks_installation target = db.tasks_installation.Where(v => v.tasks_installation_number == tin).First();
                db.tasks_installation.Remove(target);
                db.SaveChanges(lid);
                //db.SaveChanges(lid, "delete");
            } catch(Exception e)
            {

                msg = "Sonething went wrong "+e.Message;
            }

            return RedirectToAction("lis", new { lid = lid,msg=msg });
        }

        [HttpPost]
        public ActionResult Lis(FormCollection fo) {
            int lid = int.Parse(fo["lead_num"]);
            
            
            string msg=null;
            if (db.installations.Any(g=>g.lead_number==lid))
            {
                //update
                try {
                    installation target = db.installations.Where(b=>b.lead_number==lid).First();
                    TryUpdateModel(target, new string[] { "estimated_by", "statrt_date", "total_tile_cost", "estimated_date", "oneway_mileages_to_destination", "ov_labor_rate", "ov_material_rate" }, fo.ToValueProvider());
                    //target.estimated_by = fo[""];
                    //target.statrt_date = DateTime.Parse(fo[""]);
                    //target.total_tile_cost =double.Parse( fo[""]);
                    //target.estimated_date = DateTime.Parse(fo[""]);

                    //recalculate labor cost and material retails cost when user add or remove override rate
                    target.lead = new InstallationCalculationHelper().CalculateLaborAndMaterialsCostForAllTasks(target.lead);

                    db.SaveChanges(lid);
                    msg = "The installation was updated successfully at " + System.DateTime.Now; ;
                }
                catch(Exception e){
                    msg = e.Message;
                }
               
            }
            else {
                //new
                try {
                    installation target = new installation();

                    TryUpdateModel(target, new string[] { "estimated_by", "statrt_date", "total_tile_cost", "estimated_date", "oneway_mileages_to_destination", "ov_labor_rate", "ov_material_rate" }, fo.ToValueProvider());

                    target.lead = db.leads.Where(h => h.lead_number == lid).First();
                    target.lead_number = lid;

                    db.installations.Add(target);
                    db.SaveChanges(lid);
                    msg = "The installation was updated successfully at " + System.DateTime.Now; ;
                } catch (Exception e) {
                    msg = e.Message;
                }
                

            }
            return RedirectToAction("lis", new { lid = lid, msg = msg });
        }
       
    }
}