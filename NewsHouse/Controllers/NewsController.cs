using NewsHouse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsHouse.Controllers
{
    public class NewsController: Controller
    {
        ApplicationDbContext db= new ApplicationDbContext();
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
               var data = db.News.Include(x=>x.Tags).Include(y=>y.Categories).ToList();
                return View(data);
            }
            else
            {
                var data = db.News.Include(x => x.Tags).Include(y => y.Categories).ToList();
                return View(data);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AddNewCategory(int? id)
        {
            ViewData["Categories"] = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryTitle", (id != null) ? id.ToString() : "");
            return PartialView("_addNewCategory");
        }
    }
}