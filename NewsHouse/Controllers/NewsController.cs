using NewsHouse.Models;
using NewsHouse.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NewsHouse.Controllers
{
    public class NewsController: Controller
    {
        ApplicationDbContext db= new ApplicationDbContext();
        public ActionResult Index(int? id)
        {
            var data = db.News.Include(x=>x.Categories).Include(y=>y.Tags).ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Title, NewsBody, HeaderImage, IsArchived")]NewsVM newsVM, DateTime dateTime, int[] CategoryId, int[] TagsId)
        {
            News news = new News();
            HttpPostedFileBase file = newsVM.HeaderImage;
            if (file != null)
            {
                string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                file.SaveAs(Server.MapPath(filePath));
                news.HeaderImage = filePath;
            }
            foreach(int  id in TagsId)
            {
                var tag = db.Tags.Find(id);
                if (tag != null)
                {
                    news.Tags.Add(tag);
                }
            }
            foreach(int id in CategoryId)
            {
                var category = db.Categories.Find(id);
                if(category != null)
                {
                    news.Categories.Add(category);
                }
            }
            news.Title=newsVM.Title;
            news.NewsBody =newsVM.NewsBody;
            news.IsArchived=newsVM.IsArchived;
            news.Published = dateTime;
            db.News.Add(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddNewCategory(int? id) { 
            ViewData["Categories"] = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryTitle", (id != null) ? id.ToString() : "");
            return PartialView("_addNewCategory");
        }

        public ActionResult AddNewtag(int? id)
        {
            ViewData["Tags"] = new SelectList(db.Tags.ToList(), "TagsId", "TagsName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewTag");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            News news = db.News
                          .Include(n => n.Categories)
                          .Include(n => n.Tags)
                          .FirstOrDefault(n => n.NewsId == id);

            if (news == null)
                return HttpNotFound();

            // Prepare data for checkboxes
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();

            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase HeaderImage, int[] CategoryIds, int[] TagIds)
        {
            var newsToUpdate = db.News
                                 .Include(n => n.Categories)
                                 .Include(n => n.Tags)
                                 .FirstOrDefault(n => n.NewsId == id);

            if (newsToUpdate == null)
                return HttpNotFound();

            if (TryUpdateModel(newsToUpdate, "", new string[] { "Title", "NewsBody", "IsArchived" }))
            {
                // Handle image upload
                if (HeaderImage != null && HeaderImage.ContentLength > 0)
                {
                    string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(HeaderImage.FileName));
                    HeaderImage.SaveAs(Server.MapPath(filePath));
                    newsToUpdate.HeaderImage = filePath;
                }

                // Update categories
                newsToUpdate.Categories.Clear();
                if (CategoryIds != null)
                {
                    foreach (var catId in CategoryIds)
                    {
                        var category = db.Categories.Find(catId);
                        if (category != null)
                            newsToUpdate.Categories.Add(category);
                    }
                }

                // Update tags
                newsToUpdate.Tags.Clear();
                if (TagIds != null)
                {
                    foreach (var tagId in TagIds)
                    {
                        var tag = db.Tags.Find(tagId);
                        if (tag != null)
                            newsToUpdate.Tags.Add(tag);
                    }
                }

                db.Entry(newsToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Reload dropdowns
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();

            return View(newsToUpdate);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var data = db.News.Include(x=>x.Categories).Include(y=>y.Tags).FirstOrDefault(m=>m.NewsId==id);
            return View(data);
        }
    }
}
