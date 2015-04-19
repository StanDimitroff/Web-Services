using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.RestServices.Models
{
    public class CommentBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}