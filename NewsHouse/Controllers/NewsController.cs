using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NewsHouse.Models;
using NewsHouse.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace NewsHouse.Controllers
{
    [Authorize]
    public static class UserInfo
    {
        public static string GetUserId()
        {
            
            return HttpContext.Current.User.Identity.GetUserId();
        }
        public static string GetUserName()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }
        public static bool Admin()
        {
            var user = HttpContext.Current.User;
            if (user.IsAdmin("Admin"))
            {
                return true;
            }else
            {
                return false;
            }
        }
        public static bool IsAdmin(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
    [Authorize]
    public class NewsController: Controller
    {
        ApplicationDbContext db= new ApplicationDbContext();


        public ActionResult Index(int? id)
        {
            string userId = UserInfo.GetUserId();
            var itemsPerPage = 4;
            var numberOfData = db.News.Count();
            var reminder = numberOfData % itemsPerPage;
            var numberOfPages = (numberOfData - reminder) / itemsPerPage;
            ViewData["NumberOfPages"] = numberOfPages + 1;
            if (UserInfo.Admin()) { 
                if (id != null)
                {
                    var data = db.News.OrderByDescending(x => x.NewsId).Skip(id.Value * itemsPerPage).Take(itemsPerPage).Include(c => c.Categories).Include(t => t.Tags).Include(a => a.Author).ToList();
                    return View(data);
                }
                else
                {
                    var data = db.News.OrderByDescending(x => x.NewsId).Take(itemsPerPage).Include(c => c.Categories).Include(t => t.Tags).Include(a=>a.Author).ToList();
                    return View(data);
                }
            }else
            {
                if (id != null)
                {
                     var data = db.News.OrderByDescending(x => x.NewsId).Skip(id.Value * itemsPerPage).Take(itemsPerPage).Include(c => c.Categories).Include(t => t.Tags).Where(y => y.Author.User.Id == userId).ToList();
                    return View(data);
                }
                else
                {
                     var data = db.News.OrderByDescending(x => x.NewsId).Take(itemsPerPage).Include(c => c.Categories).Include(t => t.Tags).Where(x => x.Author.User.Id == userId).ToList();
                    return View(data);
                }
            }
        }

        [HttpGet]
        public JsonResult Search(string searchText)
        {
            var filtered = db.News.Where(n=>n.Title.ToLower().Contains(searchText.ToLower())).Select(a=>new {a.NewsId, a.Title}).ToList();
            return Json(filtered, JsonRequestBehavior.AllowGet);
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
            var ID = UserInfo.GetUserId();
            var author = db.Authors.FirstOrDefault(x=>x.User.Id==ID);
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
            news.Author=author;
            db.News.Add(news);
            db.SaveChanges();
            TempData["Success"] = "Article created successfully!";
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
               
                if (HeaderImage != null && HeaderImage.ContentLength > 0)
                {
                    string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(HeaderImage.FileName));
                    HeaderImage.SaveAs(Server.MapPath(filePath));
                    newsToUpdate.HeaderImage = filePath;
                }

                
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

            
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();

            return View(newsToUpdate);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var data = db.News.Include(x=>x.Categories).Include(y=>y.Tags).Include(a=>a.Author).FirstOrDefault(m=>m.NewsId==id);
            return View(data);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.News.Include(x=>x.Categories).Include(t=>t.Tags).Include(a=>a.Author).ToList();
            News news = data.FirstOrDefault(x=>x.NewsId==id); 
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var data = db.News.Find(id);
            db.News.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
