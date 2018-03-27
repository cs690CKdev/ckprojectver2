using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ck_project.Helpers
{
    public static class installconvert
    {
        public static string convert(int tasknum)
        {
            ckdatabase db = new ckdatabase();
            string result = db.tasks.Where(x => x.task_number==tasknum).Select(w => w.task_name).First();
            db = null;
            return result;
        }
    }
}