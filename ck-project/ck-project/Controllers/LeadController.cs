using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using ck_project.Helpers;



namespace ck_project.Controllers
{
    [Authorize]

    public class LeadController : Controller
    {
        //Creating the DB connecton
        ckdatabase db = new ckdatabase();
        static List<lead> lst = new List<lead>();

        // GET: Lead




        public ActionResult ListLead(int? page, string search = null, string Type = null, string Start = null, string end = null, string msg = null)
        {

            int type = string.IsNullOrEmpty(Type) ? 3 : int.Parse(Type);
            DateTime start = string.IsNullOrEmpty(Start) ? DateTime.MinValue : DateTime.Parse(Start);
            DateTime end2 = string.IsNullOrEmpty(end) ? DateTime.MaxValue : DateTime.Parse(end);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            end2 = end2.Date + ts;

            int statusNbr = 0;
            if (!string.IsNullOrEmpty(Type))
            {
                statusNbr = Int32.Parse(Type);
            }

            try
            {
                ViewBag.m = msg;
                var ClassInfo = new List<SelectListItem> {
                    new SelectListItem() { Text = "All", Selected = true, Value = "" }
                };
                ClassInfo.AddRange(db.project_status.Where(CCVV => CCVV.project_status_name != "closed").Select(b => new SelectListItem

                {
                    Text = b.project_status_name,
                    Selected = false,
                    Value = b.project_status_number.ToString()
                }));
                ViewBag.lead_type = ClassInfo;
                var result = db.leads.Where(l => (l.project_status_number != 6 && l.deleted == false)
                                      && l.lead_date >= start && l.lead_date <= end2
                                      && (string.IsNullOrEmpty(Type) || l.project_status_number == statusNbr)
                                      && (string.IsNullOrEmpty(search) || l.project_name.Contains(search))).ToList();


                LeadController.lst = result;
                return View(result.ToPagedList(page ?? 1, 8));

                //return View(db.leads.Where(x => (x.project_name.Contains(search) || search == null) && x.project_status_number == type && (x.project_status_number != 6 && x.deleted == false)).ToList());
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }


        public ActionResult Add(int id, String msg = null, string add = null)
        {
            ViewBag.add = add;
            ViewBag.m = msg;
            try
            {
                List<SelectListItem> CustomerInfo = new List<SelectListItem>();
                CustomerInfo.AddRange(db.customers.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.customer_lastname,
                    Selected = false,
                    Value = a.customer_number.ToString()
                }));

                var ClassInfo = new List<SelectListItem>();
                ClassInfo.AddRange(db.project_class.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.class_name,
                    Selected = false,
                    Value = b.class_number.ToString()
                }));

                var StatusInfo = new List<SelectListItem>();
                StatusInfo.AddRange(db.project_status.Where(x => x.deleted != true).Select(c => new SelectListItem
                {
                    Text = c.project_status_name,
                    Selected = false,
                    Value = c.project_status_number.ToString()
                }));

                var ProjectTypeInfo = new List<SelectListItem>();
                ProjectTypeInfo.AddRange(db.project_type.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.project_type_name,
                    Selected = false,
                    Value = b.project_type_number.ToString()
                }));

                var SourceInfo = new List<SelectListItem>();
                SourceInfo.AddRange(db.lead_source.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.source_name,
                    Selected = false,
                    Value = b.source_number.ToString()
                }));

                var AddressInfo = new List<SelectListItem>();
                AddressInfo.AddRange(db.addresses.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.address_type,
                    Selected = false,
                    Value = a.address_number.ToString()
                }));

                //    var AddressCityInfo = new List<SelectListItem>();

                var EmpInfo = new List<SelectListItem>();
                EmpInfo.AddRange(db.employees.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.emp_firstname + " " + a.emp_lastname,
                    Selected = false,
                    Value = a.emp_number.ToString()
                }));

                var BranchInfo = new List<SelectListItem>();
                BranchInfo.AddRange(db.branches.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.branch_name,
                    Selected = false,
                    Value = a.branch_number.ToString()
                }));

                var DeliveryStatusInfo = new List<SelectListItem>();
                DeliveryStatusInfo.AddRange(db.delivery_status.Select(a => new SelectListItem
                {
                    Text = a.delivery_status_name,
                    Selected = false,
                    Value = a.delivery_status_number.ToString()
                }));

                var TaxExemptInfo = new List<SelectListItem> {


                  new SelectListItem() { Text = "Taxable", Value = "0" },
                new SelectListItem { Text = "Tax Exempt", Value = "1" }
            };

                ViewBag.TaxExemptInfo = TaxExemptInfo;
                //setting variable passing
                ViewBag.Customer_Info = CustomerInfo;
                ViewBag.Class_Info = ClassInfo;
                ViewBag.Status_Info = StatusInfo;
                ViewBag.ProjectType_Info = ProjectTypeInfo;
                ViewBag.Source_Info = SourceInfo;
                ViewBag.Address_Info = AddressInfo;
                ViewBag.Emp_Info = EmpInfo;
                ViewBag.Branch_Info = BranchInfo;
                ViewBag.DeliveryStatus_Info = DeliveryStatusInfo;

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
                return View();
            }
            catch
            {
                ViewBag.m = " Something went wrong ... please try again";
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(FormCollection form, int id, String msg = null)
        {
            ViewBag.m = msg;
            List<SelectListItem> CustomerInfo = new List<SelectListItem>();
            CustomerInfo.AddRange(db.customers.Where(x => x.deleted != true).Select(a => new SelectListItem
            {
                Text = a.customer_lastname,
                Selected = false,
                Value = a.customer_number.ToString()
            }));

            var ClassInfo = new List<SelectListItem>();
            ClassInfo.AddRange(db.project_class.Where(x => x.deleted != true).Select(LLL => new SelectListItem
            {
                Text = LLL.class_name,
                Selected = false,
                Value = LLL.class_number.ToString()
            }));

            var StatusInfo = new List<SelectListItem>();
            StatusInfo.AddRange(db.project_status.Where(x => x.deleted != true).Select(c => new SelectListItem
            {
                Text = c.project_status_name,
                Selected = false,
                Value = c.project_status_number.ToString()
            }));

            var ProjectTypeInfo = new List<SelectListItem>();
            ProjectTypeInfo.AddRange(db.project_type.Where(x => x.deleted != true).Select(VVVb => new SelectListItem
            {
                Text = VVVb.project_type_name,
                Selected = false,
                Value = VVVb.project_type_number.ToString()
            }));

            var SourceInfo = new List<SelectListItem>();
            SourceInfo.AddRange(db.lead_source.Where(x => x.deleted != true).Select(ffb => new SelectListItem
            {
                Text = ffb.source_name,
                Selected = false,
                Value = ffb.source_number.ToString()
            }));

            var AddressInfo = new List<SelectListItem>();
            AddressInfo.AddRange(db.addresses.Where(x => x.deleted != true).Select(a => new SelectListItem
            {
                Text = a.address_type,
                Selected = false,
                Value = a.address_number.ToString()
            }));

            var AddressCityInfo = new List<SelectListItem>();

            var EmpInfo = new List<SelectListItem>();
            EmpInfo.AddRange(db.employees.Where(x => x.deleted != true).Select(a => new SelectListItem
            {
                Text = a.emp_firstname + " " + a.emp_lastname,
                Selected = false,
                Value = a.emp_number.ToString()
            }));

            var BranchInfo = new List<SelectListItem>();
            BranchInfo.AddRange(db.branches.Where(x => x.deleted != true).Select(a => new SelectListItem
            {
                Text = a.branch_name,
                Selected = false,
                Value = a.branch_number.ToString()
            }));

            var DeliveryStatusInfo = new List<SelectListItem>();
            DeliveryStatusInfo.AddRange(db.delivery_status.Select(a => new SelectListItem
            {
                Text = a.delivery_status_name,
                Selected = false,
                Value = a.delivery_status_number.ToString()
            }));


            var TaxExemptInfo = new List<SelectListItem> {


                new SelectListItem() { Text = "Taxable", Value = "0" },
                new SelectListItem { Text = "Tax Exempt", Value = "1" }
            };

            ViewBag.TaxExemptInfo = TaxExemptInfo;

            //setting variable passing
            ViewBag.Customer_Info = CustomerInfo;
            ViewBag.Class_Info = ClassInfo;
            ViewBag.Status_Info = StatusInfo;
            ViewBag.ProjectType_Info = ProjectTypeInfo;
            ViewBag.Source_Info = SourceInfo;
            ViewBag.Address_Info = AddressInfo;
            ViewBag.Emp_Info = EmpInfo;
            ViewBag.Branch_Info = BranchInfo;
            ViewBag.DeliveryStatus_Info = DeliveryStatusInfo;



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


            try
            {



                lead target = new lead();
                {
                    target.customer = db.customers.Where(aa => aa.customer_number == id).FirstOrDefault();
                    int a = Int32.Parse(form["class_number"]);
                    target.project_class = db.project_class.Where(KL => KL.class_number == a).FirstOrDefault();
                    int c = Int32.Parse(form["project_type_number"]);
                    target.project_type = db.project_type.Where(GH => GH.project_type_number == c).FirstOrDefault();
                    int dd = Int32.Parse(form["source_number"]);
                    target.lead_source = db.lead_source.Where(TY => TY.source_number == dd).FirstOrDefault();
                    int ee = Int32.Parse(form["source_number"]);
                    target.lead_source = db.lead_source.Where(qq => qq.source_number == ee).FirstOrDefault();
                    int ff = Int32.Parse(form["emp_number"]);
                    target.employee = db.employees.Where(ww => ww.emp_number == ff).FirstOrDefault();
                    int gg = Int32.Parse(form["branch_number"]);
                    target.branch = db.branches.Where(ss => ss.branch_number == gg).FirstOrDefault();
                    int hh = Int32.Parse(form["delivery_status_number"]);
                    target.delivery_status = db.delivery_status.Where(xx => xx.delivery_status_number == hh).FirstOrDefault();


                    target.in_city = Convert.ToBoolean(form["Item1.in_city"].Split(',')[0]);
                    string aaa = form["Item1.in_city"];
                    // target.tax_exempt = Convert.ToBoolean(form["Item1.tax_exempt"].Split(',')[0]);
                    target.tax_exempt = string.Equals(form["tax_exempt"], "1") ? true : false;
                    target.project_name = form["Item1.project_name"];
                    target.phone_number = form["Item1.phone_number"];
                    target.second_phone_number = form["Item1.second_phone_number"];
                    target.email = form["Item1.email"];
                    target.lead_creator = User.Identity.GetUserName();
                    target.note = form["Item1.note"];

                    target.deleted = false;
                    //   target.address_number = b.address_number;
                    target.project_status_number = 3;
                    target.lead_date = System.DateTime.Now;
                    target.Last_update_date = System.DateTime.Now;


                };
                db.leads.Add(target);
                db.SaveChanges();

                address b = new address
                {
                    address1 = form["Item2.address1"],
                    address_type = "JobAddress",
                    city = form["Item2.city"],
                    //  state = form["state"],
                    state = form["state1"],
                    county = form["Item2.county"],
                    zipcode = form["Item2.zipcode"],
                    deleted = false,
                    lead_number = target.lead_number
                };
                db.addresses.Add(b);
                db.SaveChanges();
                if (string.IsNullOrEmpty(form["Item3.address1"]))
                { }
                else
                {
                    address BC = new address
                    {
                        address1 = form["Item3.address1"],
                        //     address_type = form["Item2.address_type"],
                        address_type = "alternativeAddress",
                        city = form["Item3.city"],
                        state = form["state2"],
                        // state = form["state"],

                        //   state = "Not fixed yet",
                        county = form["Item3.county"],
                        zipcode = form["Item3.zipcode"],
                        deleted = false,
                        lead_number = target.lead_number
                    };
                    db.addresses.Add(BC);
                    db.SaveChanges();
                }


                ViewBag.m = " The lead was successfully created to " + target.customer.customer_firstname + " " + target.customer.customer_lastname + " on " + System.DateTime.Now;


                return RedirectToAction("Edit/" + target.lead_number, "lead", new { msg = ViewBag.m });
            }
            catch (Exception e)
            {
                ViewBag.m = "The lead was not created ..." + e.Message;
                return View();
            }
        }



        // read from the DB
        public ActionResult Edit(int id)
        {
            ViewBag.addressnumber = db.addresses.Where(x => x.lead_number == id).Select(v => v.address_number).First();
            try
            {
                List<SelectListItem> CustomerInfo = new List<SelectListItem>();
                CustomerInfo.AddRange(db.customers.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.customer_lastname,
                    Selected = false,
                    Value = a.customer_number.ToString()
                }));

                var ClassInfo = new List<SelectListItem>();
                ClassInfo.AddRange(db.project_class.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.class_name,
                    Selected = false,
                    Value = b.class_number.ToString()
                }));

                var StatusInfo = new List<SelectListItem>();
                StatusInfo.AddRange(db.project_status.Where(x => x.deleted != true).Select(c => new SelectListItem
                {
                    Text = c.project_status_name,
                    Selected = false,
                    Value = c.project_status_number.ToString()
                }));

                var ProjectTypeInfo = new List<SelectListItem>();
                ProjectTypeInfo.AddRange(db.project_type.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.project_type_name,
                    Selected = false,
                    Value = b.project_type_number.ToString()
                }));

                var SourceInfo = new List<SelectListItem>();
                SourceInfo.AddRange(db.lead_source.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.source_name,
                    Selected = false,
                    Value = b.source_number.ToString()
                }));

                var AddressInfo = new List<SelectListItem>();
                AddressInfo.AddRange(db.addresses.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.address_type,
                    Selected = false,
                    Value = a.address_number.ToString()
                }));

                var AddressCityInfo = new List<SelectListItem>();

                var EmpInfo = new List<SelectListItem>();
                EmpInfo.AddRange(db.employees.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.emp_firstname + " " + a.emp_lastname,
                    Selected = false,
                    Value = a.emp_number.ToString()
                }));

                var BranchInfo = new List<SelectListItem>();
                BranchInfo.AddRange(db.branches.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.branch_name,
                    Selected = false,
                    Value = a.branch_number.ToString()
                }));

                var DeliveryStatusInfo = new List<SelectListItem>();
                DeliveryStatusInfo.AddRange(db.delivery_status.Select(a => new SelectListItem
                {
                    Text = a.delivery_status_name,
                    Selected = false,
                    Value = a.delivery_status_number.ToString()
                }));
                var TaxExemptInfo = new List<SelectListItem> {


                  new SelectListItem() { Text = "Taxable",Selected = false, Value = "false" },
                new SelectListItem { Text = "Tax Exempt",Selected = false, Value = "true" }
            };

                List<lead> Leads_list = db.leads.Where(d => d.lead_number == id).ToList();
                ViewBag.Customerslist = Leads_list;
                lead target = Leads_list[0];

                //setting variable passing
                ViewBag.Customer_Info = CustomerInfo;
                ViewBag.Class_Info = ClassInfo;
                ViewBag.Status_Info = StatusInfo;
                ViewBag.ProjectType_Info = ProjectTypeInfo;
                ViewBag.Source_Info = SourceInfo;
                ViewBag.Address_Info = AddressInfo;
                ViewBag.Emp_Info = EmpInfo;
                ViewBag.Branch_Info = BranchInfo;
                ViewBag.DeliveryStatus_Info = DeliveryStatusInfo;


                TaxExemptInfo.Where(a => Convert.ToBoolean(a.Value) == target.tax_exempt).First().Selected = true;
                ViewBag.TaxExemptInfo = TaxExemptInfo;
                return View(target);
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ..." + e.Message;
                return View();
            }
        }

        // Write to the DB that is why we use POST
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection form)
        {
            ViewBag.addressnumber = db.addresses.Where(x => x.lead_number == id).Select(v => v.address_number).First();

            try
            {
                List<SelectListItem> CustomerInfo = new List<SelectListItem>();
                CustomerInfo.AddRange(db.customers.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.customer_lastname,
                    Selected = false,
                    Value = a.customer_number.ToString()
                }));

                var ClassInfo = new List<SelectListItem>();
                ClassInfo.AddRange(db.project_class.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.class_name,
                    Selected = false,
                    Value = b.class_number.ToString()
                }));

                var StatusInfo = new List<SelectListItem>();
                StatusInfo.AddRange(db.project_status.Where(x => x.deleted != true).Select(c => new SelectListItem
                {
                    Text = c.project_status_name,
                    Selected = false,
                    Value = c.project_status_number.ToString()
                }));

                var ProjectTypeInfo = new List<SelectListItem>();
                ProjectTypeInfo.AddRange(db.project_type.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.project_type_name,
                    Selected = false,
                    Value = b.project_type_number.ToString()
                }));

                var SourceInfo = new List<SelectListItem>();
                SourceInfo.AddRange(db.lead_source.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.source_name,
                    Selected = false,
                    Value = b.source_number.ToString()
                }));



                var EmpInfo = new List<SelectListItem>();
                EmpInfo.AddRange(db.employees.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.emp_firstname + " " + a.emp_lastname,
                    Selected = false,
                    Value = a.emp_number.ToString()
                }));

                var BranchInfo = new List<SelectListItem>();
                BranchInfo.AddRange(db.branches.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.branch_name,
                    Selected = false,
                    Value = a.branch_number.ToString()
                }));

                var DeliveryStatusInfo = new List<SelectListItem>();
                DeliveryStatusInfo.AddRange(db.delivery_status.Select(a => new SelectListItem
                {
                    Text = a.delivery_status_name,
                    Selected = false,
                    Value = a.delivery_status_number.ToString()
                }));

                var TaxExemptInfo = new List<SelectListItem> {


                  new SelectListItem() { Text = "Taxable",Selected = false, Value = "false" },
                new SelectListItem { Text = "Tax Exempt",Selected = false, Value = "true" }
            };

                ViewBag.TaxExemptInfo = TaxExemptInfo;





                //setting variable passing
                ViewBag.Customer_Info = CustomerInfo;
                ViewBag.Class_Info = ClassInfo;
                ViewBag.Status_Info = StatusInfo;
                ViewBag.ProjectType_Info = ProjectTypeInfo;
                ViewBag.Source_Info = SourceInfo;
                //      ViewBag.Address_Info = AddressInfo;
                ViewBag.Emp_Info = EmpInfo;
                ViewBag.Branch_Info = BranchInfo;
                ViewBag.DeliveryStatus_Info = DeliveryStatusInfo;



                List<lead> Leads_list = db.leads.Where(d => d.lead_number == id).ToList();
                ViewBag.Customerslist = Leads_list;
                lead target = Leads_list[0];

                TryUpdateModel(target, new string[] { "class_number", "project_status_number", "project_type_number", "emp_number", "branch_number", "delivery_status_number", "in_city", "source_number", "project_name", "phone_number", "second_phone_number", "email", "note" }, form.ToValueProvider());
                target.Last_update_date = System.DateTime.Now;

                target.tax_exempt = string.Equals(form["tax_exempt"], "true") ? true : false;




                {
                    new GeneralHelper().SaveProjectTotal(target.lead_number);
                }

                db.SaveChanges(target.lead_number);


                ViewBag.m = " The lead was successfully updated " + " on " + System.DateTime.Now;


                return View(target);
            }

            catch (Exception e)
            {
                ViewBag.m = "The lead was not updated ..." + e.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                List<lead> Lead_list = db.leads.Where(d => d.lead_number == id).ToList();
                ViewBag.Customerslist = Lead_list;
                lead target = Lead_list[0];
                target.deleted = true;
                db.SaveChanges(id, "delete");
                ViewBag.m = "The lead was successfully deleted.";
                return RedirectToAction("ListLead", new { search = "", msg = ViewBag.m });
            }
            catch (Exception e)
            {
                ViewBag.m = "The lead was not deleted ... " + e.Message;
                return RedirectToAction("ListLead", new { search = "", msg = ViewBag.m });
            }
        }
        public ActionResult Sort(int? page, string by)
        {
            List<lead> nresult = new List<lead>();
            switch (by)
            {
                case "pn": //project name
                    nresult = LeadController.lst.OrderBy(a => a.project_name).ToList();
                    break;
                case "cn"://customer name
                    nresult = LeadController.lst.OrderBy(a => a.customer.customer_firstname).ToList();
                    break;
                case "ds"://Designer
                    nresult = LeadController.lst.OrderBy(a => a.employee.emp_firstname).ToList();
                    break;
                case "sp"://sales person
                    nresult = LeadController.lst.OrderBy(a => a.lead_creator).ToList();
                    break;
                case "pt"://project type
                    nresult = LeadController.lst.OrderBy(a => a.project_type.project_type_name).ToList();
                    break;
                case "ps"://project status
                    nresult = LeadController.lst.OrderBy(a => a.project_status.project_status_name).ToList();
                    break;
                case "cd"://create date
                    nresult = LeadController.lst.OrderByDescending(a => a.lead_date).ToList();
                    break;
                case "lud"://last update date
                    nresult = LeadController.lst.OrderByDescending(a => a.Last_update_date).ToList();
                    break;
                case "br"://branch
                    nresult = LeadController.lst.OrderBy(a => a.branch.branch_name).ToList();
                    break;

            }

            var ClassInfo = new List<SelectListItem>();
            ClassInfo.AddRange(db.project_status.Where(CCVV => CCVV.project_status_name != "closed").Select(b => new SelectListItem

            {
                Text = b.project_status_name,
                Selected = false,
                Value = b.project_status_number.ToString()
            }));
            ViewBag.lead_type = ClassInfo;

            return View("ListLead", nresult.ToPagedList(page ?? 1, 8));
        }

    }
}