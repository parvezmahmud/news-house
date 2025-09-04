using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsHouse.Models
{
    public class Tags
    {
        public int TagsId { get; set; }
        public string TagsName { get; set; }
        public ICollection<TagTracker> TagTracker { get; set; }
    }
}