using ck_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        ckdatabase db = new ckdatabase();
        
        //main view
        public ActionResult View(int id,string msg=null) {
            List<string> pset = db.products.Where(a => a.lead_number == id && a.deleted==false).Select(e=> e.cat_anme).Distinct().ToList();
            var lead = db.leads.Where(l => l.lead_number == id).First();
            ViewBag.lid = id;
            ViewBag.customerNbr = lead.customer_number;

            List<Pg> agent = new List<Pg>();
            foreach (var a in pset) {
                agent.Add(new Pg { cat = a, lid = id ,uid=a.Split(' ')[0].Replace('/','_').Replace(',','C')});
            }

            return View(agent);
        }

        //create new
        public ActionResult Add(int lid) {
            ViewBag.lid = lid;
            List<SelectListItem> cat = new List<SelectListItem>();
            foreach (var a in Helpers.Constants.catDefList)
            {
                cat.Add(new SelectListItem { Text = a, Value = a });
            }
            ViewBag.catlist = cat;
            return PartialView();
        }

        [HttpPost]
        public ActionResult Add(FormCollection fo,int lid) {
            lead mian = db.leads.Where(a => a.lead_number == lid).FirstOrDefault();
            product target = new product();
            
            TryUpdateModel<product>(target, new string[] {"cat_anme","quantity","Description","model","color","product_source","manufacture","location","price" }, fo.ToValueProvider());
            //target.cat_anme = fo["Product Category"];
            if (ModelState.IsValid) {
                mian.products.Add(target);
                db.products.Add(target);
                db.SaveChanges(lid);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);

        }

        ///**
        // * <param name="id"> id here means product id not lead id
        // * as lead id already in product</param>
        // * */
        //public ActionResult Edit(int id) {
        //    //todo: another listing
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult Edit(FormCollection fo, int id) {

        //    return View(); 
        //}

        //helper
        public ActionResult gp(string cat,int lid) {
            List<product> ll = db.products.Where(a => a.lead_number == lid && a.cat_anme.Equals(cat) && a.deleted==false).ToList();
            List<SelectListItem> catt = new List<SelectListItem>();
            foreach (var a in Helpers.Constants.catDefList)
            {
                catt.Add(new SelectListItem { Text = a, Value = a });
            }
            ViewBag.catlist = catt;
            return PartialView(ll);
        }

        //[HttpPost]
        //public ActionResult gp(FormCollection fo) {
        //    int id = int.Parse(fo["item.product_number"]);
        //    product target = db.products.Where(a => a.product_number == id).FirstOrDefault();
        //    TryUpdateModel(target, new string[] { "quantity","Description","model","color", "product_source", "manufacture", "location", "price" },fo.ToValueProvider());
        //    if (ModelState.IsValid) {
        //        db.SaveChanges(id);
        //    }
        //    //ViewBag.lid = id;
        //    string cat = fo["item.cat_anme"];
        //    List<product> ll = db.products.Where(d => d.lead_number == target.lead_number && d.cat_anme.Equals(cat)).ToList();
        //        return PartialView(ll); 

        //    }

        //public ActionResult Notify(int id,string status) {
        //    ViewBag.lid = id;
        //    switch (status) {
        //        case "ok":
        //            ViewBag.outt= "change sussfuelly";
        //            break;
        //        case "bad":
        //            ViewBag.outt = "chage not sucessful";
        //            break;
        //        default:
        //            ViewBag.outt = "system error";
        //            break;

        //    }
        //    return View();
        //}

        //@model=product, not gp
        [HttpPost]
        public ActionResult handle(int pid,FormCollection fo) {
            product target = db.products.Where(a => a.product_number == pid).FirstOrDefault();
            int lid = target.lead_number;
            //not work?
            //TryUpdateModel(target, fo.ToValueProvider());
            target.color = fo["item.color"];
            target.Description = fo["item.Description"];
            target.location = fo["item.location"];
            target.manufacture = fo["item.manufacture"];
            target.model = fo["item.model"];
            target.price = Double.Parse(fo["item.price"]);
            target.product_source = fo["item.product_source"];
            target.quantity = int.Parse(fo["item.quantity"]);
            //target.cat_anme = fo["item.cat_anme"];
            if (ModelState.IsValid) {
                db.SaveChanges(lid);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            //return RedirectToAction("view",new {id=target.lead_number });
            return null;
             
        }

        //delete
        public ActionResult DelP(int pid) {
            product target = db.products.Where(p => p.product_number == pid).First();
            int lid = target.lead_number;
            target.deleted = true;
            //db.products.Remove(target);
            db.SaveChanges(lid);

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }

}