using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsHouse.Models
{
    public class CategoryTracker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<News> News { get; set; }
    }
}