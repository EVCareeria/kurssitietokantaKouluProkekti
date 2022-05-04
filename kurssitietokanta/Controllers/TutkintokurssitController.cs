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

namespace kurssitietokanta.Controllers
{
    public class TutkintokurssitController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Tutkintokurssit
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

            var tutkintokurssit = db.Tutkintokurssit.Include(t => t.Opintojaksot).Include(t => t.Tutkinnot);

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                tutkintokurssit = tutkintokurssit.Where(t => t.Tutkinnot.Tutkintonimi.Contains(SearchedItem));
            }


            ViewBag.ShowSortedTutkinto = String.IsNullOrEmpty(SortByOrder) ? "Tutkinto_desc" : "";


            switch (SortByOrder)
            {
                case "Tutkinto_desc":
                    tutkintokurssit = tutkintokurssit.OrderByDescending(t => t.Tutkinnot.Tutkintonimi);
                    break;

                default:
                    tutkintokurssit = tutkintokurssit.OrderBy(t => t.Tutkinnot.Tutkintonimi);
                    break;
            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);

            return View(tutkintokurssit.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Tutkintokurssit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintokurssit tutkintokurssit = db.Tutkintokurssit.Find(id);
            if (tutkintokurssit == null)
            {
                return HttpNotFound();
            }
            return View(tutkintokurssit);
        }

        // GET: Tutkintokurssit/Create
        public ActionResult Create()
        {
            ViewBag.Opintojakso_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi");
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi");
            return View();
        }

        // POST: Tutkintokurssit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutkintokurssi_Id,Tutkinto_Id,Opintojakso_Id")] Tutkintokurssit tutkintokurssit)
        {
            if (ModelState.IsValid)
            {
                db.Tutkintokurssit.Add(tutkintokurssit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Opintojakso_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", tutkintokurssit.Opintojakso_Id);
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintokurssit.Tutkinto_Id);
            return View(tutkintokurssit);
        }

        // GET: Tutkintokurssit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintokurssit tutkintokurssit = db.Tutkintokurssit.Find(id);
            if (tutkintokurssit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opintojakso_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", tutkintokurssit.Opintojakso_Id);
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintokurssit.Tutkinto_Id);
            return View(tutkintokurssit);
        }

        // POST: Tutkintokurssit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tutkintokurssi_Id,Tutkinto_Id,Opintojakso_Id")] Tutkintokurssit tutkintokurssit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutkintokurssit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opintojakso_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", tutkintokurssit.Opintojakso_Id);
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintokurssit.Tutkinto_Id);
            return View(tutkintokurssit);
        }

        // GET: Tutkintokurssit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintokurssit tutkintokurssit = db.Tutkintokurssit.Find(id);
            if (tutkintokurssit == null)
            {
                return HttpNotFound();
            }
            return View(tutkintokurssit);
        }

        // POST: Tutkintokurssit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutkintokurssit tutkintokurssit = db.Tutkintokurssit.Find(id);
            db.Tutkintokurssit.Remove(tutkintokurssit);
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
            stringWriter.WriteLine("\"Tutkintokurssi_Id\",\"Tutkinto_Id\",\"Opintojakso_Id\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkintokurssit_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listTutkintokurssit = db.Tutkintokurssit.Include(t => t.Opintojaksot).Include(t => t.Tutkinnot).OrderBy(t => t.Tutkintokurssi_Id).ToList();

            foreach (var Tutkintokurssi in listTutkintokurssit)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"", Tutkintokurssi.Tutkintokurssi_Id, Tutkintokurssi.Tutkinnot.Tutkintonimi, Tutkintokurssi.Opintojaksot.Opintojaksonnimi));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from t in db.Tutkintokurssit.Include(t => t.Opintojaksot).Include(t => t.Tutkinnot).OrderBy(t => t.Tutkintokurssi_Id).ToList()
            select new
                                  {
                                      Id = t.Tutkintokurssi_Id,
                                      Tutkinto = t.Tutkinnot.Tutkintonimi,
                                      Opintojakso = t.Opintojaksot.Opintojaksonnimi,
                                  };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkintokurssit_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }
}
