using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsHouse.Models
{
    public class News
    {
        public News()
        {
            Tags = new List<Tags>();
            Categories = new List<Category>();
        }
        public int NewsId { get; set; }
        public string Title { get; set; }

        [Display(Name ="News Body")]
        public string NewsBody {  get; set; }

        [Display(Name ="Header Image")]
        public string HeaderImage { get; set; }
        public bool IsArchived { get; set; }

        public DateTime Published { get; set; }
        public Author Author { get; set; }
        public ICollection<Tags> Tags { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}