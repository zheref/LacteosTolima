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
    public class ConsumptionController : Controller
    {
        private CowsDBContext db = new CowsDBContext();

        //
        // GET: /Consumption/

        public ViewResult Index()
        {
            var consumptions = db.Consumptions.Include(c => c.Cow);
            return View(consumptions.ToList());
        }

        //
        // GET: /Consumption/Details/5

        public ViewResult Details(int id)
        {
            Consumption consumption = db.Consumptions.Find(id);
            return View(consumption);
        }

        //
        // GET: /Consumption/Create

        public ActionResult Create()
        {
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name");
            return View();
        } 

        //
        // POST: /Consumption/Create

        [HttpPost]
        public ActionResult Create(Consumption consumption)
        {
            if (ModelState.IsValid)
            {
                db.Consumptions.Add(consumption);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", consumption.CowId);
            return View(consumption);
        }
        
        //
        // GET: /Consumption/Edit/5
 
        public ActionResult Edit(int id)
        {
            Consumption consumption = db.Consumptions.Find(id);
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", consumption.CowId);
            return View(consumption);
        }

        //
        // POST: /Consumption/Edit/5

        [HttpPost]
        public ActionResult Edit(Consumption consumption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CowId = new SelectList(db.Cows, "Id", "Name", consumption.CowId);
            return View(consumption);
        }

        //
        // GET: /Consumption/Delete/5
 
        public ActionResult Delete(int id)
        {
            Consumption consumption = db.Consumptions.Find(id);
            return View(consumption);
        }

        //
        // POST: /Consumption/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Consumption consumption = db.Consumptions.Find(id);
            db.Consumptions.Remove(consumption);
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