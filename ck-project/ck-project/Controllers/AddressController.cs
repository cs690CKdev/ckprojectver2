using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ck_project.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        // GET: Address
        ckdatabase db = new ckdatabase();


        public ActionResult Edit(int id, string mode)
        {
            ViewBag.id = id;
            ViewBag.mode = mode;
            ViewBag.title = "Edit Address";

            int? lid = db.addresses.Where(t => t.address_number == id).First().lead_number;
            var Sstate = new List<SelectListItem> {
               new SelectListItem() {Text="Alabama", Value="AL"},
        new SelectListItem() { Text="Alaska", Value="AK"},
        new SelectListItem() { Text="Arizona", Value="AZ"},
        new SelectListItem() { Text="Arkansas", Value="AR"},
        new SelectListItem() { Text="California", Value="CA"},
        new SelectListItem() { Text="Colorado", Value="CO"},
        new SelectListItem() { Text="Connecticut", Value="CT"},
        new SelectListItem() { Text="District of Columbia", Value="DC"},
        new SelectListItem() { Text="Delaware", Value="DE"},
        new SelectListItem() { Text="Florida", Value="FL"},
        new SelectListItem() { Text="Georgia", Value="GA"},
        new SelectListItem() { Text="Hawaii", Value="HI"},
        new SelectListItem() { Text="Idaho", Value="ID"},
        new SelectListItem() { Text="Illinois", Value="IL"},
        new SelectListItem() { Text="Indiana", Value="IN"},
        new SelectListItem() { Text="Iowa", Value="IA"},
        new SelectListItem() { Text="Kansas", Value="KS"},
        new SelectListItem() { Text="Kentucky", Value="KY"},
        new SelectListItem() { Text="Louisiana", Value="LA"},
        new SelectListItem() { Text="Maine", Value="ME"},
        new SelectListItem() { Text="Maryland", Value="MD"},
        new SelectListItem() { Text="Massachusetts", Value="MA"},
        new SelectListItem() { Text="Michigan", Value="MI"},
        new SelectListItem() { Text="Minnesota", Value="MN"},
        new SelectListItem() { Text="Mississippi", Value="MS"},
        new SelectListItem() { Text="Missouri", Value="MO"},
        new SelectListItem() { Text="Montana", Value="MT"},
        new SelectListItem() { Text="Nebraska", Value="NE"},
        new SelectListItem() { Text="Nevada", Value="NV"},
        new SelectListItem() { Text="New Hampshire", Value="NH"},
        new SelectListItem() { Text="New Jersey", Value="NJ"},
        new SelectListItem() { Text="New Mexico", Value="NM"},
        new SelectListItem() { Text="New York", Value="NY"},
        new SelectListItem() { Text="North Carolina", Value="NC"},
        new SelectListItem() { Text="North Dakota", Value="ND"},
        new SelectListItem() { Text="Ohio", Value="OH"},
        new SelectListItem() { Text="Oklahoma", Value="OK"},
        new SelectListItem() { Text="Oregon", Value="OR"},
        new SelectListItem() { Text="Pennsylvania", Value="PA"},
        new SelectListItem() { Text="Rhode Island", Value="RI"},
        new SelectListItem() { Text="South Carolina", Value="SC"},
        new SelectListItem() { Text="South Dakota", Value="SD"},
        new SelectListItem() { Text="Tennessee", Value="TN"},
        new SelectListItem() { Text="Texas", Value="TX"},
        new SelectListItem() { Text="Utah", Value="UT"},
        new SelectListItem() { Text="Vermont", Value="VT"},
        new SelectListItem() { Text="Virginia", Value="VA"},
        new SelectListItem() { Text="Washington", Value="WA"},
        new SelectListItem() { Text="West Virginia", Value="WV"},
        new SelectListItem() { Text="Wisconsin", Value="WI"},
        new SelectListItem() { Text="Wyoming", Value="WY"}

            };
            ViewBag.Sstate = Sstate;
            //special for lead
            if (mode == "ll")
            {
                ViewBag.title = "Edit Alternative Address";
                ViewBag.mode = "ll";
                if (db.addresses.Any(e => e.lead_number == lid && e.address_type == "alternativeAddress"))
                {

                    //update ll
                    address ll = db.addresses.Where(q => q.lead_number == lid && q.address_type == "alternativeAddress").First();
                    Sstate.Where(v => v.Value == ll.state).First().Selected = true;
                    return View(ll);
                }
                else
                {
                    //new ll
                    return View(new address { address_number = -2 });
                }
            }
            //reguler address
            try
            {
                address target = db.addresses.Where(t => t.address_number == id).First();

                Sstate.Where(r => r.Value == target.state).First().Selected = true;
                return View(target);
            }
            catch (Exception e)
            {
                ViewBag.m = e.Message;
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string mode, FormCollection fo)
        {
            var Sstate = new List<SelectListItem> {
               new SelectListItem() {Text="Alabama", Value="AL"},
        new SelectListItem() { Text="Alaska", Value="AK"},
        new SelectListItem() { Text="Arizona", Value="AZ"},
        new SelectListItem() { Text="Arkansas", Value="AR"},
        new SelectListItem() { Text="California", Value="CA"},
        new SelectListItem() { Text="Colorado", Value="CO"},
        new SelectListItem() { Text="Connecticut", Value="CT"},
        new SelectListItem() { Text="District of Columbia", Value="DC"},
        new SelectListItem() { Text="Delaware", Value="DE"},
        new SelectListItem() { Text="Florida", Value="FL"},
        new SelectListItem() { Text="Georgia", Value="GA"},
        new SelectListItem() { Text="Hawaii", Value="HI"},
        new SelectListItem() { Text="Idaho", Value="ID"},
        new SelectListItem() { Text="Illinois", Value="IL"},
        new SelectListItem() { Text="Indiana", Value="IN"},
        new SelectListItem() { Text="Iowa", Value="IA"},
        new SelectListItem() { Text="Kansas", Value="KS"},
        new SelectListItem() { Text="Kentucky", Value="KY"},
        new SelectListItem() { Text="Louisiana", Value="LA"},
        new SelectListItem() { Text="Maine", Value="ME"},
        new SelectListItem() { Text="Maryland", Value="MD"},
        new SelectListItem() { Text="Massachusetts", Value="MA"},
        new SelectListItem() { Text="Michigan", Value="MI"},
        new SelectListItem() { Text="Minnesota", Value="MN"},
        new SelectListItem() { Text="Mississippi", Value="MS"},
        new SelectListItem() { Text="Missouri", Value="MO"},
        new SelectListItem() { Text="Montana", Value="MT"},
        new SelectListItem() { Text="Nebraska", Value="NE"},
        new SelectListItem() { Text="Nevada", Value="NV"},
        new SelectListItem() { Text="New Hampshire", Value="NH"},
        new SelectListItem() { Text="New Jersey", Value="NJ"},
        new SelectListItem() { Text="New Mexico", Value="NM"},
        new SelectListItem() { Text="New York", Value="NY"},
        new SelectListItem() { Text="North Carolina", Value="NC"},
        new SelectListItem() { Text="North Dakota", Value="ND"},
        new SelectListItem() { Text="Ohio", Value="OH"},
        new SelectListItem() { Text="Oklahoma", Value="OK"},
        new SelectListItem() { Text="Oregon", Value="OR"},
        new SelectListItem() { Text="Pennsylvania", Value="PA"},
        new SelectListItem() { Text="Rhode Island", Value="RI"},
        new SelectListItem() { Text="South Carolina", Value="SC"},
        new SelectListItem() { Text="South Dakota", Value="SD"},
        new SelectListItem() { Text="Tennessee", Value="TN"},
        new SelectListItem() { Text="Texas", Value="TX"},
        new SelectListItem() { Text="Utah", Value="UT"},
        new SelectListItem() { Text="Vermont", Value="VT"},
        new SelectListItem() { Text="Virginia", Value="VA"},
        new SelectListItem() { Text="Washington", Value="WA"},
        new SelectListItem() { Text="West Virginia", Value="WV"},
        new SelectListItem() { Text="Wisconsin", Value="WI"},
        new SelectListItem() { Text="Wyoming", Value="WY"}

            };
            ViewBag.Sstate = Sstate;

            switch (fo["mode"])
            {
                case "l"://lead add update mode
                    address lad = db.addresses.Where(t => t.address_number == id).First();
                    lad.address_type = "JobAddress";
                    lad.state = fo["state"];
                    lad.deleted = false;
                    TryUpdateModel(lad, new string[] { "address1", "city", "county", "zipcode" }, fo.ToValueProvider());
                    //lad.address1 = fo["item.address1"];
                    // lad.city = fo["item.city"];
                    //lad.county = fo["item.county"];
                    // lad.zipcode = fo["item.zipcode"];
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e) { ViewBag.m = e.Message; }

                    break;
                case "c"://customer add update mode
                    address custadd = db.addresses.Where(r => r.address_number == id).First();
                    custadd.address_type = "Billing";
                    custadd.deleted = false;
                    custadd.state = fo["state"];
                    TryUpdateModel(custadd, new string[] { "address1", "city", "county", "zipcode" }, fo.ToValueProvider());
                    //custadd.address1 = fo["item.address1"];
                    //custadd.city = fo["item.city"];
                    //custadd.county = fo["item.county"];
                    //custadd.zipcode = fo["item.zipcode"];

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e) { ViewBag.m = e.Message; }
                    break;
                case "ll"://lead second add mode
                    int? lid = db.addresses.Where(t => t.address_number == id).First().lead_number;
                    if (int.Parse(fo["address_number"]) < 0)
                    {
                        //new  second address
                        address n = new address();
                        n.deleted = false;
                        n.address_type = "alternativeAddress";
                        n.state = fo["state"];
                        TryUpdateModel(n, new string[] { "address1", "city", "county", "zipcode" }, fo.ToValueProvider());
                        //  n.address1 = fo["item.address1"];
                        //  n.city = fo["item.city"];
                        //n.county = fo["item.county"];
                        // n.zipcode = fo["item.zipcode"];
                        n.lead_number = lid;
                        try
                        {
                            db.addresses.Add(n);
                            db.SaveChanges();
                        }
                        catch (Exception e) { ViewBag.m = e.Message; }

                    }
                    else
                    {
                        //update second
                        int aid = int.Parse(fo["address_number"]);
                        address q = db.addresses.Where(c => c.address_number == aid).First();
                        TryUpdateModel(q, new string[] { "address1", "city", "county", "zipcode" }, fo.ToValueProvider());
                        // q.address1 = fo["item.address1"];
                        //q.city = fo["item.city"];
                        //q.county = fo["item.county"];
                        //q.zipcode = fo["item.zipcode"];
                        q.address_type = "alternativeAddress";
                        q.state = fo["state"];
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e) { ViewBag.m = e.Message; }

                    }


                    break;
                case "b"://branch add update mode
                    address target = db.addresses.Where(r => r.address_number == id).First();
                    target.deleted = false;
                    target.state = fo["state"];
                    target.address_type = "BranchAddress";
                    TryUpdateModel(target, new string[] { "address1", "city", "county", "zipcode" }, fo.ToValueProvider());
                    // target.address1 = fo["item.address1"];
                    //target.city = fo["item.city"];
                    // target.county = fo["item.county"];
                    // target.zipcode = fo["item.zipcode"];
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e) { ViewBag.m = e.Message; }
                    break;


            }

            ViewBag.m = " The address was successfully updated on " + System.DateTime.Now;
            return RedirectToAction("Edit", new { id = id, mode = mode });
        }



    }
}