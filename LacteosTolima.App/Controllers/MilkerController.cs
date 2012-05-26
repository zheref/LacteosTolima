using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LacteosTolima.App.Models;

namespace LacteosTolima.App.Controllers
{ 
    public class MilkerController : Controller
    {
        private CowsDBContext db = new CowsDBContext();

        //
        // GET: /Milker/

        public ViewResult Index()
        {
            var milkersq =  from m in db.Milkers.ToList()
                            where m.State == "A"
                            select m;

            return View(milkersq.ToList());
        }

        //
        // GET: /Milker/Details/5

        public ViewResult Details(int id)
        {
            Milker milker = db.Milkers.Find(id);
            return View(milker);
        }

        //
        // GET: /Milker/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Milker/Create

        [HttpPost]
        public ActionResult Create(Milker milker)
        {
            milker.State = "A";
            if (ModelState.IsValidField("JoinDate") && DateTime.Now < milker.JoinDate)
                ModelState.AddModelError("JoinDate", "Input a current or past date");
            if (milker.Age > 60 || milker.Age < 18)
                ModelState.AddModelError("Age", "The age of the milker is not appropiate for job");
            if (ModelState.IsValid)
            {
                db.Milkers.Add(milker);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(milker);
        }
        
        //
        // GET: /Milker/Edit/5
 
        public ActionResult Edit(int id)
        {
            Milker milker = db.Milkers.Find(id);
            return View(milker);
        }

        //
        // POST: /Milker/Edit/5

        [HttpPost]
        public ActionResult Edit(Milker milker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(milker);
        }

        //
        // GET: /Milker/Delete/5
 
        public ActionResult Delete(int id)
        {
            Milker milker = db.Milkers.Find(id);
            return View(milker);
        }

        //
        // POST: /Milker/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Milker milker = db.Milkers.Find(id);
            db.Milkers.Remove(milker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(int id)
        {
            Milker milker = db.Milkers.Find(id);
            milker.State = "I";
            if (ModelState.IsValid)
            {
                db.Entry(milker).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}