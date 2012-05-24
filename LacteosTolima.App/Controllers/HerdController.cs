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
    public class HerdController : Controller
    {
        private CowsDBContext db = new CowsDBContext();

        //
        // GET: /Herd/

        public ViewResult Index()
        {
            var herds = db.Herds.Include(h => h.Milker);
            
            var herdsq =    from h in herds.ToList()
                            where h.State == "A"
                            select h;

            return View(herdsq.ToList());
        }

        //
        // GET: /Herd/Details/5

        public ViewResult Details(int id)
        {
            Herd herd = db.Herds.Find(id);
            return View(herd);
        }

        //
        // GET: /Herd/Create

        public ActionResult Create()
        {
            ViewBag.OpeId = new SelectList(db.Milkers, "Id", "Name");
            return View();
        } 

        //
        // POST: /Herd/Create

        [HttpPost]
        public ActionResult Create(Herd herd)
        {
            herd.State = "A";
            if (ModelState.IsValid)
            {
                db.Herds.Add(herd);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.OpeId = new SelectList(db.Milkers, "Id", "Name", herd.MilkerId);
            return View(herd);
        }
        
        //
        // GET: /Herd/Edit/5
 
        public ActionResult Edit(int id)
        {
            Herd herd = db.Herds.Find(id);
            ViewBag.OpeId = new SelectList(db.Milkers, "Id", "Name", herd.MilkerId);
            return View(herd);
        }

        //
        // POST: /Herd/Edit/5

        [HttpPost]
        public ActionResult Edit(Herd herd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(herd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OpeId = new SelectList(db.Milkers, "Id", "Name", herd.MilkerId);
            return View(herd);
        }

        //
        // GET: /Herd/Delete/5
 
        public ActionResult Delete(int id)
        {
            Herd herd = db.Herds.Find(id);
            return View(herd);
        }

        //
        // POST: /Herd/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Herd herd = db.Herds.Find(id);
            db.Herds.Remove(herd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(int id)
        {
            Herd herd = db.Herds.Find(id);
            herd.State = "I";
            if (ModelState.IsValid)
            {
                db.Entry(herd).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ViewResult ProductionReportHerdDay(DateTime day)
        {
            List<ProductionReportHerd> report = new List<ProductionReportHerd>();

            Herd aux = new Herd();

            Milker aux1 = new Milker();

            Cow aux2 = new Cow();

            List<Cow> cows = new List<Cow>();

            Production production = new Production();

            var Herd = from h in db.Herds.ToList() select h;

            var Milker = from o in db.Milkers.ToList() select o;

            //var Cow = from c in a.Cows.ToList() select c;

            foreach (Herd h in Herd)
            {
                ProductionReportHerd item = new ProductionReportHerd();
                Double quant = new Double();
                item.IdHerd = h.Id;
                item.NameHerd = h.Name;
                foreach (Milker o in Milker)
                {
                    aux1.Name = o.Name;

                }
                item.NameOpe = aux1.Name;

                cows = h.Cows;

                foreach (Cow c in cows)
                {
                    var query = from co in db.Productions.ToList()
                                where co.Id == c.Id && co.Date == day && c.State.Equals('A')
                                select co;

                    foreach (Production p in query)
                    {
                        quant = quant + p.Quant;

                    }

                    quant = quant / (query.ToList<Production>().Count);
                    item.Quant = quant;
                }

                report.Add(item);
            }

            return View(report);
        }

        public ViewResult ConsumptionReportHerdDay(DateTime day)
        {
            List<ConsumptionReportHerd> report = new List<ConsumptionReportHerd>();

            Herd aux = new Herd();

            Milker aux1 = new Milker();

            Cow aux2 = new Cow();

            List<Cow> cows = new List<Cow>();

            Production production = new Production();

            var Herd = from h in db.Herds.ToList() select h;

            var Milker = from o in db.Milkers.ToList() select o;

            //var Cow = from c in a.Cows.ToList() select c;

            foreach (Herd h in Herd)
            {
                ConsumptionReportHerd item = new ConsumptionReportHerd();
                Double hay = new Double();
                Double Silage = new Double();

                item.IdHerd = h.Id;
                item.NameHerd = h.Name;
                item.HowCows = h.Cows.Count();
                foreach (Milker o in Milker)
                {
                    aux1.Name = o.Name;

                }
                item.NameOpe = aux1.Name;

                cows = h.Cows;

                foreach (Cow c in cows)
                {
                    var query = from co in db.Consumptions.ToList()
                                where co.Id == c.Id && co.Date == day && c.State.Equals('A')
                                select co;

                    foreach (Consumption co in query)
                    {
                        hay = hay + co.HayAmount;
                        Silage = Silage + co.SilageAmout;

                    }

                    item.HayAmount = hay;
                    item.SilageAmout = Silage;
                }

                report.Add(item);
            }

            return View(report);
        }

        public ViewResult ProductionReportHerdMonth(DateTime Month)
        {
            List<ProductionReportHerd> report = new List<ProductionReportHerd>();

            Herd aux = new Herd();

            Milker aux1 = new Milker();

            Cow aux2 = new Cow();

            List<Cow> cows = new List<Cow>();

            Production production = new Production();

            var Herd = from h in db.Herds.ToList() select h;

            var Milker = from o in db.Milkers.ToList() select o;

            //var Cow = from c in a.Cows.ToList() select c;

            foreach (Herd h in Herd)
            {
                ProductionReportHerd item = new ProductionReportHerd();
                Double quant = new Double();
                item.IdHerd = h.Id;
                item.NameHerd = h.Name;
                foreach (Milker o in Milker)
                {
                    aux1.Name = o.Name;

                }
                item.NameOpe = aux1.Name;

                cows = h.Cows;

                foreach (Cow c in cows)
                {
                    var query = from co in db.Productions.ToList()
                                where co.Id == c.Id && c.State.Equals('A') && co.Date.Month.Equals(Month.Month) && co.Date.Year.Equals(Month.Year) && c.State.Equals('A')
                                select co;

                    foreach (Production p in query)
                    {
                        quant = quant + p.Quant;
                    }

                    quant = quant / (query.ToList<Production>().Count);
                }

                report.Add(item);
            }

            return View(report);
        }

        public ViewResult ConsumptionReportHerdMonth(DateTime Month)
        {
            List<ConsumptionReportHerd> report = new List<ConsumptionReportHerd>();

            Herd aux = new Herd();

            Milker aux1 = new Milker();

            Cow aux2 = new Cow();

            List<Cow> cows = new List<Cow>();

            Production production = new Production();

            var Herd = from h in db.Herds.ToList() select h;

            var Milker = from o in db.Milkers.ToList() select o;

            //var Cow = from c in a.Cows.ToList() select c;

            foreach (Herd h in Herd)
            {
                ConsumptionReportHerd item = new ConsumptionReportHerd();
                Double hay = new Double();
                Double Silage = new Double();

                item.IdHerd = h.Id;
                item.NameHerd = h.Name;
                item.HowCows = h.Cows.Count();
                foreach (Milker o in Milker)
                {
                    aux1.Name = o.Name;

                }
                item.NameOpe = aux1.Name;

                cows = h.Cows;

                foreach (Cow c in cows)
                {
                    var query = from co in db.Consumptions.ToList()
                                where co.Id == c.Id && c.State.Equals('A') && co.Date.Month.Equals(Month.Month) && co.Date.Month.Equals(Month.Year)
                                select co;

                    foreach (Consumption co in query)
                    {
                        hay = hay + co.HayAmount;
                        Silage = Silage + co.SilageAmout;

                    }

                    item.HayAmount = hay;
                    item.SilageAmout = Silage;
                }

                report.Add(item);
            }

            return View(report);
        }
       

        
    }
}