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
            var cows = db.Cows.Include(c => c.Mother).Include(c => c.Herd).Include(c => c.Consumptions).Include(c => c.Productions);

            var cowsq = from c in cows.ToList()
                        where c.State == "A"
                        select c;

            /*
            foreach(Cow c in cowsq)
            {
                /*
                var children = from cow in cows.ToList()
                               where (cow.Mother == null) ? true : (cow.Mother.Id == c.Id)  && cow.State == "A"
                               select cow;

                c.Children = children.ToList();
                 *

                var children = from cow in c.Children.ToList()
                               where cow.State == "A"
                               select cow;

                c.Children = children.ToList();
            }*/

            return View(cowsq.ToList());
        }

        //
        // GET: /Cow/Details/5

        public ViewResult Details(int id)
        {
            var cowq = from c in db.Cows.Include(c => c.Children).Include(c => c.Herd).Include(c => c.Consumptions).Include(c => c.Productions).ToList()
                       where c.Id == id && c.State == "A"
                       select c;
            
            Cow a = cowq.Single();
            var children = from cow in a.Children.ToList()
                           where cow.State == "A"
                           select cow;

            a.Children = children.ToList();
            return View(a);
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

            var cowsq = from c in db.Cows
                        where c.State == "A"
                        select c;

            var herdsq = from h in db.Cows
                        where h.State == "A"
                        select h;

            ViewBag.MotherId = new SelectList(cowsq, "Id", "Name", cow.MotherId);
            ViewBag.HerdId = new SelectList(herdsq, "Id", "Name", cow.HerdId);
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

        public ViewResult ProductionReportCowDay(string dayp)
        {
            string[] fe = dayp.Split(new Char[] {'/'});
            string dayt = fe[2];
            string month = fe[1];
            string year = fe[0];

            if(dayt.Equals(string.Empty)) return ProductionReportCowMonth(dayp);

            
            DateTime day = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(dayt));
            //ConsumptionReportCowDay(day);

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
                if(sum>0)
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult ConsumptionReportCowDay(string dayc)
        {
            string[] fe = dayc.Split(new Char[] { '/' });
            string dayt = fe[2];
            string month = fe[1];
            string year = fe[0];

            if (dayt.Equals(string.Empty)) return ConsumptionReportCowMonth(dayc);

            DateTime fecha = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(dayt));
            
            List<ConsumptionReportCow> report = new List<ConsumptionReportCow>();

            Cow aux = new Cow();

            Consumption production = new Consumption();

            var cows = from c in db.Cows.ToList() where c.State == "A" select c;

            //var Consumption = from co in a.Consumptions.ToList() select co;

            foreach (Cow c in cows)
            {
                ConsumptionReportCow item = new ConsumptionReportCow();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                //var resultPro = Consumption.Where(p => p.CowId.Equals(c.Id));

                var query = from co in db.Consumptions
                            where co.CowId == c.Id 
                                  && co.Date.Day == fecha.Day 
                                  && co.Date.Month == fecha.Month
                                  && co.Date.Year == fecha.Year
                            select co;

                Double totalHay = new Double();
                Double totalSilage= new Double();
                foreach (Consumption co in query)
                {
                    totalHay += co.HayAmount;
                    totalSilage += co.SilageAmout;
                }

                item.HayAmount = totalHay;
                item.SilageAmout = totalSilage;
                if(totalHay > 0 || totalSilage > 0)
                report.Add(item);
            }


            return View(report);
        }

        public ViewResult ProductionReportCowMonth(string fecha)
        {
            string[] fe = fecha.Split(new Char[] { '/' });
            string month = fe[1];
            string year = fe[0];

            DateTime Month = new DateTime(Int32.Parse(year), Int32.Parse(month), 1);
            List<ProductionReportCow> report = new List<ProductionReportCow>();
            Cow aux = new Cow();
            Production production = new Production();

            //var cows = from c in db.Cows.ToList() select c;
            var cowsq = from c in db.Cows.ToList()
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
                if(sum>0)
                report.Add(item);
            }
            return View(report);
        }

        public ViewResult ConsumptionReportCowMonth(string fec)
        {
            string[] fe = fec.Split(new Char[] { '/' });
            string month = fe[1];
            string year = fe[0];

            DateTime fecha = new DateTime(Int32.Parse(year), Int32.Parse(month),1);
            
            List<ConsumptionReportCow> report = new List<ConsumptionReportCow>();

            Cow aux = new Cow();

            Consumption production = new Consumption();

            var cows = from c in db.Cows.ToList() where c.State == "A" select c;

            //var Consumption = from co in a.Consumptions.ToList() select co;

            foreach (Cow c in cows)
            {
                ConsumptionReportCow item = new ConsumptionReportCow();
                item.IdCow = c.Id;
                item.NameCow = c.Name;
                //var resultPro = Consumption.Where(p => p.CowId.Equals(c.Id));

                var query = from co in db.Consumptions
                            where co.CowId == c.Id
                                  && co.Date.Month == fecha.Month
                                  && co.Date.Year == fecha.Year
                            select co;

                Double totalHay = new Double();
                Double totalSilage = new Double();
                foreach (Consumption co in query)
                {
                    totalHay += co.HayAmount;
                    totalSilage += co.SilageAmout;
                }

                item.HayAmount = totalHay;
                item.SilageAmout = totalSilage;
                if (totalHay > 0 || totalSilage > 0)
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
                where p.CowId == c.Id && c.State == "A"
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
