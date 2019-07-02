using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MatRoleClaim.Models;

namespace MatRoleClaim.Controllers
{
    [Authorize]
    public class BlogsController : BaseController
    {
        public ActionResult Index()
        {
            var blogs = DbContext.Blogs.Include(b => b.Author);
            return View(blogs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = DbContext.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(DbContext.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,PostDate,AuthorId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                DbContext.Blogs.Add(blog);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(DbContext.Users, "Id", "Email", blog.AuthorId);
            return View(blog);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = DbContext.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(DbContext.Users, "Id", "Email", blog.AuthorId);
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,PostDate,AuthorId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(blog).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(DbContext.Users, "Id", "Email", blog.AuthorId);
            return View(blog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = DbContext.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = DbContext.Blogs.Find(id);
            DbContext.Blogs.Remove(blog);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
