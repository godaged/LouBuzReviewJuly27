using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LouBuzReview.Data;
using LouBuzReview.Models;
using LouBuzReview.ViewModels;

namespace LouBuzReview.Controllers
{
    public class WebsiteReviewsController : Controller
    {
        private LouBuzReviewContext db = new LouBuzReviewContext();

        // GET: WebsiteReviews
        public ActionResult Index(string searchName)
        {
            var websiteReviews = db.WebsiteReviews.Include(r => r.WebUser).Include(r => r.Website);

            if (!String.IsNullOrEmpty(searchName))
            {
                websiteReviews = websiteReviews.Where(w =>
                    w.WebUser.FirstName.ToLower().Contains(searchName.ToLower())
                    ||
                    w.WebUser.LastName.ToLower().Contains(searchName.ToLower())
                    ||
                    w.Website.WebsiteName.ToLower().Contains(searchName.ToLower()) 
                    ||
                    w.Ratings.ToString().ToLower().Contains(searchName.ToLower()));
            }
            return View(websiteReviews.ToList());
        }

        // GET: WebsiteReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var websiteReview = db.WebsiteReviews
                .Include(r => r.WebUser)
                .Include(r => r.Website)
                .Where(r => r.ID == id)
                .FirstOrDefault();

            if (websiteReview == null)
            {
                return HttpNotFound();
            }
            return View(websiteReview);
        }

        // GET: WebsiteReviews/Create
        public ActionResult Create()
        {
            ViewBag.WebsiteID = new SelectList(db.Websites, "WebsiteID", "WebsiteUrl");
            ViewBag.UserID = new SelectList(db.WebUsers, "UserID", "FirstName");
            return View();
        }

        // POST: WebsiteReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,WebsiteID,Ratings,UserReview,CreatedDate")] WebsiteReview websiteReview)
        {
            if (ModelState.IsValid)
            {
                db.WebsiteReviews.Add(websiteReview);
                db.SaveChanges();
                TempData["Message"] = "Review was successfully added";
                return RedirectToAction("Index");
            }

            ViewBag.WebsiteID = new SelectList(db.Websites, "WebsiteID", "WebsiteUrl", websiteReview.WebsiteID);
            ViewBag.UserID = new SelectList(db.WebUsers, "UserID", "FirstName", websiteReview.UserID);
            return View(websiteReview);
        }


        // GET: WebsiteReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebsiteReview websiteReview = db.WebsiteReviews.Find(id);
            if (websiteReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebsiteID = new SelectList(db.Websites, "WebsiteID", "WebsiteID", websiteReview.WebsiteID);
            ViewBag.UserID = new SelectList(db.WebUsers, "UserID", "UserID", websiteReview.UserID);
            return View(websiteReview);
        }

        // POST: WebsiteReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebsiteReview websiteReview)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(websiteReview).State = EntityState.Modified;
                //Confirm  WebUser table as edited
                db.Entry(websiteReview.WebUser).State = EntityState.Modified;
                //Confirm  WebUser table as edited
                db.Entry(websiteReview.Website).State = EntityState.Modified;
                db.SaveChanges(); 
                //Captures tempdata from server to display operation completed successfully
                TempData["Message"] = "Edited Review was successfully Saved";
                return RedirectToAction("Index");
            }
            
            return View(websiteReview);
        }

        // GET: WebsiteReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebsiteReview websiteReview = db.WebsiteReviews.Find(id);
            if (websiteReview == null)
            {
                return HttpNotFound();
            }
            return View(websiteReview);
        }

        // POST: WebsiteReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //select the record for the id passed to delete from WebsiteReviews table
            WebsiteReview websiteReview = db.WebsiteReviews.Find(id);
            //Get the related primary key of Webuser  
            var userid = websiteReview.WebUser.UserID;
            //Get the related primary key of Website  
            var websiteid = websiteReview.Website.WebsiteID;
            //find record for the selected Primary Keys
            Website website = db.Websites.Find(websiteid);
            WebUser webuser = db.WebUsers.Find(userid);
            //Removing records from respective tables
            db.WebsiteReviews.Remove(websiteReview);
            db.Websites.Remove(website);
            db.WebUsers.Remove(webuser);
            //save the database
            db.SaveChanges();
            //Captures tempdata from server to display operation completed successfully
            TempData["Message"] = "Review was successfully deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
