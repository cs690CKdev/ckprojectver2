using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ck_project.Helpers
{
    public static class TaskHelper
    {
        public static int Mainsun(int iid,string maincat) {
            ckdatabase db = new ckdatabase();
            int result;
            result = db.tasks_installation.Where(t => t.installation_number == iid && t.task.task_main_cat == maincat).Count();

            return result;
        }

        public static int Subsun(int iid, string maincat, string subcat) {
            ckdatabase db = new ckdatabase();
            int result = db.tasks_installation.Where(t => t.installation_number == iid && t.task.task_main_cat == maincat && t.task.task_sub_cat == subcat).Count();

            return result;

        }
    }
}