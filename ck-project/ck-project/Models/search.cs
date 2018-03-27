using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ck_project.Models
{
    public class Search
    {
        [DataType(DataType.Date)]
        public DateTime start;
        [DataType(DataType.Date)]
        public DateTime end;
        public List<SelectListItem> Emp;
       
        public string name;
            
    }
}