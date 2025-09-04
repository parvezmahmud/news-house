using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsHouse.Models.ViewModel
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "News Body")]
        public string NewsBody { get; set; }

        [Display(Name = "Header Image")]
        public HttpPostedFileBase HeaderImage { get; set; }
        public bool IsArchived { get; set; }
        public DateTime Published { get; set; }
    }
}