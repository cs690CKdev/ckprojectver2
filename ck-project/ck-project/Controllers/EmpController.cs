using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace ck_project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmpController : Controller
    {

        //Creating the DB connecton
        ckdatabase db = new ckdatabase();

        public ActionResult ListEmp(int? page, string search, String msg = null)

        {
            ViewBag.m = msg;
            try
            {
                return View(db.employees.Where(x => x.emp_lastname.Contains(search) || search == null && x.deleted == false).ToList().ToPagedList(page ?? 1, 8));
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... "+e.Message;
                return View();
            }
        }


        // read from the DB
        public ActionResult Edit(int id)
        {
            try {
                //setting dropdown list for forgern key
                var utype = new List<SelectListItem>();
                utype.AddRange(db.users_types.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.user_type_name,
                    Selected = false,
                    Value = a.user_type_number.ToString()
                }));

                var branchtypes = new List<SelectListItem>();
                branchtypes.AddRange(db.branches.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.branch_name,
                    Selected = false,
                    Value = b.branch_number.ToString()
                }));
                //setting variable passing
                ViewBag.utype = utype;
                ViewBag.branch = branchtypes;
                ViewBag.id = id;


                List<employee> Employees_list = db.employees.Where(d => d.emp_number == id).ToList();
                ViewBag.Customerslist = Employees_list;
                employee target = Employees_list[0];
                target.emp_password = EncryptionHelper.Decrypt(target.emp_password);
                branchtypes.Where(q =>int.Parse( q.Value )==target.branch.branch_number).First().Selected = true;
                utype.Where(t => int.Parse(t.Value) == target.users_types.user_type_number).First().Selected = true;
                return View(target);
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }


        // Write to the DB that is why we use POST
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection fo)
        {
            try {

                //setting dropdown list for forgern key
                var utype = new List<SelectListItem>();
                utype.AddRange(db.users_types.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.user_type_name,
                    Selected = false,
                    Value = a.user_type_number.ToString()
                }));

                var branchtypes = new List<SelectListItem>();
                branchtypes.AddRange(db.branches.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.branch_name,
                    Selected = false,
                    Value = b.branch_number.ToString()
                }));
                //setting variable passing
                ViewBag.utype = utype;
                ViewBag.branch = branchtypes;


                List<employee> Employees_list = db.employees.Where(d => d.emp_number == id).ToList();
                ViewBag.Employeeslist = Employees_list;
                employee target = Employees_list[0];
                string oldpassword = target.emp_password;
                TryUpdateModel(target, new string[] { "emp_firstname", "emp_middlename", "emp_lastname", "emp_username", "emp_password", "user_type_number", "branch_number", "phone_number" }, fo.ToValueProvider());
                if (target.emp_password == oldpassword) { } else {

                    target.emp_password = EncryptionHelper.Encrypt(target.emp_password);
                }

                db.SaveChanges();

                ViewBag.m = " The employee was successfully updated " + " on " + System.DateTime.Now;

                return View(target);
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(FormCollection form)
        {
            try {
                //setting dropdown list for forgern key
                var utype = new List<SelectListItem>();
                utype.AddRange(db.users_types.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.user_type_name,
                    Selected = false,
                    Value = a.user_type_number.ToString()
                }));

                var branchtypes = new List<SelectListItem>();
                branchtypes.AddRange(db.branches.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.branch_name,
                    Selected = false,
                    Value = b.branch_number.ToString()
                }));
                //setting variable passing
                ViewBag.utype = utype;
                ViewBag.branch = branchtypes;
                //create instance
                employee target = new employee();
                //get property
                TryUpdateModel(target, new string[] { "emp_firstname", "emp_middlename", "emp_password", "emp_lastname", "emp_username", "user_type_number", "branch_number", "emp_number", "phone_number" }, form.ToValueProvider());

                //validate
                //  double tempPhoneNumber = 0;
                ViewBag.Error = null;

                //if (string.IsNullOrEmpty(target.emp_firstname))
                //{
                //    ViewBag.Error = "firstname is required";
                //}
                //else if (target.emp_firstname.Length > 35)
                //{
                //    ViewBag.Error = "firstname is 35";
                //}
                //else if (string.IsNullOrEmpty(target.phone_number))
                //{
                //    ViewBag.Error = "phone number is required";
                //}

                //else
                {
                    target.emp_password = EncryptionHelper.Encrypt(target.emp_password);
                    db.employees.Add(target);
                    db.SaveChanges();
                    ViewBag.m = "The employee was successfully created " + "on " + System.DateTime.Now;
                }
                return View(target);
            }
            catch (Exception e)
            {
                ViewBag.m = "The employee was not created " + e.Message;
                return View();
            }
        }

        public ActionResult Add()
        {
            try {
                var utype = new List<SelectListItem>();
                utype.AddRange(db.users_types.Where(x => x.deleted != true).Select(a => new SelectListItem
                {
                    Text = a.user_type_name,
                    Selected = false,
                    Value = a.user_type_number.ToString()
                }));

                var branchtypes = new List<SelectListItem>();
                branchtypes.AddRange(db.branches.Where(x => x.deleted != true).Select(b => new SelectListItem
                {
                    Text = b.branch_name,
                    Selected = false,
                    Value = b.branch_number.ToString()
                }));

                ViewBag.utype = utype;
                ViewBag.branch = branchtypes;
                
                return View();
            }
            catch (Exception e)
            {
                ViewBag.m = "Something went wrong ... " + e.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                List<employee> Employees_list = db.employees.Where(d => d.emp_number == id).ToList();
                ViewBag.Customerslist = Employees_list;
                employee target = Employees_list[0];
                target.deleted = true;
                db.SaveChanges();
            ViewBag.m = "The employee was successfully deleted.";
            return RedirectToAction("ListEmp", new { search = "", msg = ViewBag.m });
        }


            catch (Exception e)
            {
                ViewBag.m = "The employee was not deleted ..." + e.Message;
                return RedirectToAction("ListEmp", new { search = "", msg = ViewBag.m });
            }
        }


        }
}