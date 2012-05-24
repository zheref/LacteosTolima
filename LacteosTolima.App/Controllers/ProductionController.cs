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
    public class ProductionController : Controller
    {
        private CowsDBContext db = new CowsDBContext();

        //
        // GET: /Production/

        public ViewResult Index()
        {
            var productions = db.Productions.Include(p => p.Cow);
            return View(productions.ToList());
        }

        //
        // GET: /Production/Details/5

        public ViewResult Details(int id)
        {
            Production production = db.Productions.Find(id);
            return View(production);
        }

        //
        // GET: /Production/Create

        public ActionResult Create()
        {
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name");
            return View();
        } 

        //
        // POST: /Production/Create

        [HttpPost]
        public ActionResult Create(Production production)
        {
            if (ModelState.IsValid)
            {
                db.Productions.Add(production);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", production.CowId);
            return View(production);
        }
        
        //
        // GET: /Production/Edit/5
 
        public ActionResult Edit(int id)
        {
            Production production = db.Productions.Find(id);
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", production.CowId);
            return View(production);
        }

        //
        // POST: /Production/Edit/5

        [HttpPost]
        public ActionResult Edit(Production production)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", production.CowId);
            return View(production);
        }

        //
        // GET: /Production/Delete/5
 
        public ActionResult Delete(int id)
        {
            Production production = db.Productions.Find(id);
            return View(production);
        }

        //
        // POST: /Production/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Production production = db.Productions.Find(id);
            db.Productions.Remove(production);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}