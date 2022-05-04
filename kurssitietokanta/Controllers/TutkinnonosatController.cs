using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kurssitietokanta.Models;
using PagedList;
using Rotativa;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Ajax.Utilities;

namespace kurssitietokanta.Controllers
{
    public class TutkinnonosatController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Tutkinnonosat
        public ActionResult Index(string SearchedItem, string SortByOrder, int? Page, int? PageTotalNumber, string PageItems)
        {
            
            if (SearchedItem != null)
            {
                Page = 1;
            }
            else
            {
                SearchedItem = PageItems;
            }

            ViewBag.SearchedItemList = SearchedItem;
            ViewBag.SortedPage = SortByOrder;

            ViewBag.ShowSortedTutkintoosanimi = String.IsNullOrEmpty(SortByOrder) ? "Tutkintoosanimi_desc" : "";
            ViewBag.ShowSortedTutkinnot = SortByOrder == "Tutkinnot" ? "Tutkinnot" : "Tutkinnot";


            var tutkinnonosat = db.Tutkinnonosat.Include(t => t.Tutkinnot).Include(t => t.Vastuuopettajat);

            if (!String.IsNullOrEmpty(SearchedItem))
            {

                switch (SortByOrder)
                {
                    case "Tutkintoosanimi_desc":
                        tutkinnonosat = tutkinnonosat.Where(t => t.Tutkintoosanimi.Contains(SearchedItem)).OrderByDescending(t => t.Tutkintoosanimi);
                        break;
                    case "Tutkinnot":
                        tutkinnonosat = tutkinnonosat.Where(t => t.Tutkintoosanimi.Contains(SearchedItem)).OrderBy(t => t.Tutkinnot.Tutkintonimi);
                        break;

                    case "Tutkinnot_desc":
                        tutkinnonosat = tutkinnonosat.Where(t => t.Tutkintoosanimi.Contains(SearchedItem)).OrderByDescending(t => t.Tutkinnot.Tutkintonimi);
                        break;

                    default:
                        tutkinnonosat = tutkinnonosat.Where(t => t.Tutkintoosanimi.Contains(SearchedItem)).OrderBy(t => t.Tutkintoosanimi);
                        break;
                }
            }
            else
            {
                switch (SortByOrder)
                {
                    case "Tutkintoosanimi_desc":
                        tutkinnonosat = tutkinnonosat.OrderByDescending(t => t.Tutkintoosanimi);
                        break;
                    case "Tutkinnot":
                        tutkinnonosat = tutkinnonosat.OrderBy(t => t.Tutkinnot.Tutkintonimi);
                        break;

                    case "Tutkinnot_desc":
                        tutkinnonosat = tutkinnonosat.OrderByDescending(t => t.Tutkinnot.Tutkintonimi);
                        break;

                    default:
                        tutkinnonosat = tutkinnonosat.OrderBy(t => t.Tutkintoosanimi);
                        break;
                }

            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);

            return View(tutkinnonosat.ToList().ToPagedList(PageNumber, Maxpages));
        }



        // GET: Tutkinnonosat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnonosat tutkinnonosat = db.Tutkinnonosat.Find(id);
            if (tutkinnonosat == null)
            {
                return HttpNotFound();
            }
            return View(tutkinnonosat);
        }

        // GET: Tutkinnonosat/Create
        public ActionResult Create()
        {
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi");
            ViewBag.Vastuuopettajan_Id = new SelectList(db.Vastuuopettajat, "Vastuuopettajan_Id", "Vastuuopettajanimi");
            return View();
        }

        // POST: Tutkinnonosat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutkintoosa_Id, Tutkintoosanimi, Tutkinto_Id, Vastuuopettajan_Id, LaajuusOSP, Esitietovaatimus")] Tutkinnonosat tutkinnonosat)
        {
            if (ModelState.IsValid)
            {
                db.Tutkinnonosat.Add(tutkinnonosat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkinnonosat.Tutkinto_Id);
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Vastuuopettajan_Id", "Vastuuopettajanimi", tutkinnonosat.Vastuuopettajan_Id);
            return View(tutkinnonosat);
        }

        // GET: Tutkinnonosat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnonosat tutkinnonosat = db.Tutkinnonosat.Find(id);
            if (tutkinnonosat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkinnonosat.Tutkinto_Id);
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Vastuuopettajan_Id", "Vastuuopettajanimi", tutkinnonosat.Vastuuopettajan_Id);
            return View(tutkinnonosat);
        }

        // POST: Tutkinnonosat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tutkintoosa_Id,Tutkintoosanimi,Tutkinto_Id,Vastuuopettajan_Id,LaajuusOSP,Esitietovaatimus")] Tutkinnonosat tutkinnonosat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutkinnonosat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkinnonosat.Tutkinto_Id);
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Vastuuopettajan_Id", "Vastuuopettajanimi", tutkinnonosat.Vastuuopettajan_Id);
            return View(tutkinnonosat);
        }

        // GET: Tutkinnonosat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnonosat tutkinnonosat = db.Tutkinnonosat.Find(id);
            if (tutkinnonosat == null)
            {
                return HttpNotFound();
            }
            return View(tutkinnonosat);
        }

        // POST: Tutkinnonosat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutkinnonosat tutkinnonosat = db.Tutkinnonosat.Find(id);
            db.Tutkinnonosat.Remove(tutkinnonosat);
            db.SaveChanges();
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

        public ActionResult Print()
        {
            var printPdf = new ActionAsPdf("Index");
            return printPdf;
        }

        public void ExportToCSV()
        {
            var stringWriter = new StringWriter();
            stringWriter.WriteLine("\"Tutkintoosa_Id\",\"Tutkintoosanimi\",\"Tutkinto_Id\",\"Vastuuopettajan_Id\",\"LaajuusOSP\",\"Esitietovaatimus\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkinnonosat_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listTutkinnonosat = db.Tutkinnonosat.Include(t => t.Tutkinnot).Include(t => t.Vastuuopettajat).OrderBy(t => t.Tutkintoosa_Id).ToList();

            foreach (var Tutkintoosa in listTutkinnonosat)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", Tutkintoosa.Tutkintoosa_Id, Tutkintoosa.Tutkintoosanimi, Tutkintoosa.Tutkinnot.Tutkintonimi, Tutkintoosa.Vastuuopettajat.Vastuuopettajanimi, Tutkintoosa.LaajuusOSP, Tutkintoosa.Esitietovaatimus));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from t in db.Tutkinnonosat.Include(t => t.Tutkinnot).Include(t => t.Vastuuopettajat).OrderBy(t => t.Tutkintoosa_Id).ToList()
                                  select new
                                  {
                                      Id = t.Tutkintoosa_Id,
                                      Tutkintoosa = t.Tutkintoosanimi,
                                      Tutkinto = t.Tutkinnot.Tutkintonimi,
                                      Vastuuopettaja = t.Vastuuopettajat.Vastuuopettajanimi,
                                      Laajuus_OSP = t.LaajuusOSP,
                                      Esitietovaatimukset = t.Esitietovaatimus,
                                   };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opettajat_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();


        }
    }
}
