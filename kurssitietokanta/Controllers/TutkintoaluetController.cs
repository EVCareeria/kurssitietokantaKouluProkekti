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
    public class TutkintoalueetController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Tutkintoalueet
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

            //var Opettajat = from p in db.Opettajat
            //                select p;

            var tutkintoalueet = db.Tutkintoalueet.Include(t => t.Tutkinnot);

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                tutkintoalueet = tutkintoalueet.Where(t => t.Tutkintoaluenimi.Contains(SearchedItem));
            }


            ViewBag.ShowSortedTutkintoalue = String.IsNullOrEmpty(SortByOrder) ? "Tutkintoalue_desc" : "";


            switch (SortByOrder)
            {
                case "Tutkintoalue_desc":
                    tutkintoalueet = tutkintoalueet.OrderByDescending(t => t.Tutkintoaluenimi);
                    break;

                default:
                    tutkintoalueet = tutkintoalueet.OrderBy(t => t.Tutkintoaluenimi);
                    break;
            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);
            return View(tutkintoalueet.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Tutkintoalueet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintoalueet tutkintoalueet = db.Tutkintoalueet.Find(id);
            if (tutkintoalueet == null)
            {
                return HttpNotFound();
            }
            return View(tutkintoalueet);
        }

        // GET: Tutkintoalueet/Create
        public ActionResult Create()
        {
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi");
            return View();
        }

        // POST: Tutkintoalueet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutkintoalue_Id,Tutkinto_Id,Tutkintoaluenimi")] Tutkintoalueet tutkintoalueet)
        {
            if (ModelState.IsValid)
            {
                db.Tutkintoalueet.Add(tutkintoalueet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintoalueet.Tutkinto_Id);
            return View(tutkintoalueet);
        }

        // GET: Tutkintoalueet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintoalueet tutkintoalueet = db.Tutkintoalueet.Find(id);
            if (tutkintoalueet == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintoalueet.Tutkinto_Id);
            return View(tutkintoalueet);
        }

        // POST: Tutkintoalueet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tutkintoalue_Id,Tutkinto_Id,Tutkintoaluenimi")] Tutkintoalueet tutkintoalueet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutkintoalueet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", tutkintoalueet.Tutkinto_Id);
            return View(tutkintoalueet);
        }

        // GET: Tutkintoalueet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkintoalueet tutkintoalueet = db.Tutkintoalueet.Find(id);
            if (tutkintoalueet == null)
            {
                return HttpNotFound();
            }
            return View(tutkintoalueet);
        }

        // POST: Tutkintoalueet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutkintoalueet tutkintoalueet = db.Tutkintoalueet.Find(id);
            db.Tutkintoalueet.Remove(tutkintoalueet);
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
            stringWriter.WriteLine("\"Tutkintoalue_Id\",\"Tutkintoalue\",\"Tutkinto\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkintoalueet_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listTutkintoalueet = db.Tutkintoalueet.Include(t => t.Tutkinnot).OrderBy(t => t.Tutkintoalue_Id).ToList();

            foreach (var Tutkintoalue in listTutkintoalueet)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"", Tutkintoalue.Tutkintoalue_Id, Tutkintoalue.Tutkintoaluenimi, Tutkintoalue.Tutkinnot.Tutkintonimi));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from t in db.Tutkintoalueet.Include(t => t.Tutkinnot).OrderBy(t => t.Tutkintoalue_Id).ToList()
                                  select new
                                  {
                                      Id = t.Tutkintoalue_Id,
                                      Tutkintoalue = t.Tutkintoaluenimi,
                                      Tutkinto = t.Tutkinnot.Tutkintonimi,
                                  };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkintoalueet_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }
}
