using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace BugTracker.RestServices.Models
{
    public class EditBugBindingModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}