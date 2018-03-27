using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ck_project.Helpers
{
    public class Tracker
    {
        //backup code for log system. do not delete!

        //public int SaveChanges(int lead_id, string op = "update")
        //{
        //    var changes = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
        //    var time_s = DateTime.Now.ToLocalTime();
        //    foreach (var change in changes)
        //    {
        //        var target_name = change.Entity.GetType().Name;


        //        foreach (var field in change.OriginalValues.PropertyNames)
        //        {
        //            if (change.OriginalValues[field].ToString() != change.CurrentValues[field].ToString())
        //            {
        //                lead_log_file log = new lead_log_file
        //                {
        //                    prvious_value = change.OriginalValues[field].ToString(),
        //                    new_value = change.CurrentValues[field].ToString(),
        //                    table_name = change.Entity.GetType().Name,
        //                    column_name = field,
        //                    update_date = time_s,
        //                    lead_number = lead_id,
        //                    emp_username = System.Web.HttpContext.Current.User.Identity.Name,
        //                    action_name = op
        //                };


        //            }

        //        }
        //    }
        //    return base.SaveChanges();
        //}
    }
}