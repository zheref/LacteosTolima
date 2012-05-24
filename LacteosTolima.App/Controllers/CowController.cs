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
    public class CowController : Controller
    {
        private CowsDBContext db = new CowsDBContext();

        //
        // GET: /Cow/

        public ViewResult Index()
        {
            var cows = db.Cows.Include(c => c.Mother).Include(c => c.Herd);

            var cowsq = from c in cows.ToList()
                        where c.State == "A"
                        select c;

            return View(cowsq.ToList());
        }

        //
        // GET: /Cow/Details/5

        public ViewResult Details(int id)
        {
            Cow cow = db.Cows.Find(id);
            return View(cow);
        }

        //
        // GET: /Cow/Create

        public ActionResult Create()
        {
            ViewBag.MotherId = new SelectList(db.Cows, "Id", "Name");
            ViewBag.HerdId = new SelectList(db.Herds, "Id", "Name");
            return View();
        } 

        //
        // POST: /Cow/Create

        [HttpPost]
        public ActionResult Create(Cow cow)
        {
            cow.State = "A";
            if (ModelState.IsValid)
            {
                db.Cows.Add(cow);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.MotherId = new SelectList(db.Cows, "Id", "Name", cow.MotherId);
            ViewBag.HerdId = new SelectList(db.Herds, "Id", "Name", cow.HerdId);
            return View(cow);
        }
        
        //
        // GET: /Cow/Edit/5
 
        public ActionResult Edit(int id)
        {
            Cow cow = db.Cows.Find(id);
            ViewBag.MotherId = new SelectList(db.Cows, "Id", "Name", cow.MotherId);
            ViewBag.HerdId = new SelectList(db.Herds, "Id", "Name", cow.HerdId);
            return View(cow);
        }

        //
        // POST: /Cow/Edit/5

        [HttpPost]
        public ActionResult Edit(Cow cow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MotherId = new SelectList(db.Cows, "Id", "Name", cow.MotherId);
            ViewBag.HerdId = new SelectList(db.Herds, "Id", "Name", cow.HerdId);
            return View(cow);
        }

        //
        // GET: /Cow/Delete/5
 
        public ActionResult Delete(int id)
        {
            Cow cow = db.Cows.Find(id);
            return View(cow);
        }

        //
        // POST: /Cow/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cow cow = db.Cows.Find(id);
            db.Cows.Remove(cow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(int id)
        {
            Cow cow = db.Cows.Find(id);
            cow.State = "I";
            if (ModelState.IsValid)
            {
                db.Entry(cow).State = EntityState.Modified;
                db.SaveChanges();    
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ViewResult ProductionReportCowDay(string dayt, string month, string year)
        {
            if(dayt.Equals(string.Empty)) return ProductionReportCowMonth(month, year);

            DateTime day = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(dayt));
            List<ProductionReportCow> report = new List<ProductionReportCow>();
            Cow aux = new Cow();
            Production production = new Production();
            
            var cows = from c in db.Cows.ToList() select c;
            var cowsq = from c in cows.ToList()
                        where c.State == "A"
                        select c;

            foreach (Cow c in cowsq)
            {
                ProductionReportCow item = new ProductionReportCow();
                Double sum = new Double();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                // var resultPro = productions.Where(p => p.CowId.Equals(c.Id));

                var query = from p in db.Productions.ToList()
                            where   p.CowId == c.Id &&
                                    p.Date.Day == day.Day &&
                                    p.Date.Month == day.Month &&
                                    p.Date.Year == day.Year
                            select p;

                foreach (Production p in query)
                    sum += p.Quant;

                item.Quant = sum;
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult ConsumptionReportCowDay(DateTime day)
        {
            List<ConsumptionReportCow> report = new List<ConsumptionReportCow>();

            Cow aux = new Cow();

            Production production = new Production();

            var cows = from c in db.Cows.ToList() select c;

            //var Consumption = from co in a.Consumptions.ToList() select co;

            foreach (Cow c in cows)
            {
                ConsumptionReportCow item = new ConsumptionReportCow();
                Double Hay = new Double();
                Double Silage = new Double();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                //var resultPro = Consumption.Where(p => p.CowId.Equals(c.Id));

                var query = from co in db.Consumptions
                            where co.CowId == c.Id && co.Date == day && c.State.Equals('A')
                            select co;

                foreach (Consumption co in query)
                {
                    Hay = co.HayAmount;
                    Silage = co.SilageAmout;
                }
                item.HayAmount = Hay;
                item.SilageAmout = Silage;
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult ProductionReportCowMonth(string month, string year)
        {
            DateTime Month = new DateTime(Int32.Parse(year), Int32.Parse(month), 1);
            List<ProductionReportCow> report = new List<ProductionReportCow>();
            Cow aux = new Cow();
            Production production = new Production();

            var cows = from c in db.Cows.ToList() select c;
            var cowsq = from c in cows.ToList()
                        where c.State == "A"
                        select c;

            foreach (Cow c in cowsq)
            {
                ProductionReportCow item = new ProductionReportCow();
                Double sum = new Double();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                // var resultPro = productions.Where(p => p.CowId.Equals(c.Id));

                var query = from p in db.Productions
                            where   p.CowId == c.Id &&
                                    p.Date.Month == Month.Month &&
                                    p.Date.Year == Month.Year
                            select p;

                foreach (Production p in query)
                    sum += p.Quant;

                item.Quant = sum;
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult ConsumptionReportCowMonth(DateTime Month)
        {
            List<ConsumptionReportCow> report = new List<ConsumptionReportCow>();

            Cow aux = new Cow();

            Production production = new Production();

            var cows = from c in db.Cows.ToList() select c;

            //var Consumption = from co in a.Consumptions.ToList() select co;

            foreach (Cow c in cows)
            {
                ConsumptionReportCow item = new ConsumptionReportCow();
                Double Hay = new Double();
                Double Silage = new Double();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                //var resultPro = Consumption.Where(p => p.CowId.Equals(c.Id));

                var query = from co in db.Consumptions.ToList()
                            where co.Id == c.Id && co.Date.Month.Equals(Month.Month) && co.Date.Month.Equals(Month.Year) && c.State.Equals('A')
                            select co;

                foreach (Consumption co in query)
                {
                    Hay = Hay + co.HayAmount;
                    Silage = Silage + co.SilageAmout;
                }
                item.HayAmount = Hay;
                item.SilageAmout = Silage;
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult CowPerHerdReport(Int32 IdHerd)
        {
            List<CowPerHerdReport> report = new List<CowPerHerdReport>();

            var query3 = from h in db.Herds.ToList()
                where h.Id == IdHerd
                select h;

            foreach(Herd h in query3){
            List<Cow> cows = h.Cows;

            foreach (Cow c in cows)
            {
                CowPerHerdReport item =new CowPerHerdReport();
                Double quant = new Double();
                Double hay = new Double();
                Double silage = new Double();

                item.IdCow = c.Id;
                item.NameCow = c.Name;
                              
                var query = from p in db.Productions.ToList()
                where p.CowId == c.Id && c.State.Equals('A')
                select p;

                foreach (Production p in query)
                    {
                        quant = quant + p.Quant;
                    }

                item.Quant = quant;
                var query2 = from co in db.Consumptions.ToList()
                where co.CowId == c.Id && c.State.Equals('A')
                select co;

                foreach(Consumption co in query2)
                {
                    hay = hay + co.HayAmount;
                    silage = silage + co.SilageAmout;
                }
                item.HayAmount=hay;
                item.SilageAmout=silage;
                report.Add(item);
            }
            }   
                return View(report);
            }

            
        }
    }
