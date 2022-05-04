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
    public class VastuuopettajatController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Vastuuopettajat
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

           
            var vastuuopettajat = db.Vastuuopettajat.Include(v => v.Opettajat);

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                vastuuopettajat = vastuuopettajat.Where(v => v.Vastuuopettajanimi.Contains(SearchedItem));
            }


            ViewBag.ShowSortedVastuuopettaja = String.IsNullOrEmpty(SortByOrder) ? "Vastuuopettaja_desc" : "";


            switch (SortByOrder)
            {
                case "Vastuuopettaja_desc":
                    vastuuopettajat = vastuuopettajat.OrderByDescending(p => p.Vastuuopettajanimi);
                    break;

                default:
                    vastuuopettajat = vastuuopettajat.OrderBy(p => p.Vastuuopettajanimi);
                    break;
            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);
            return View(vastuuopettajat.ToList().ToPagedList(PageNumber, Maxpages));
        }


        // GET: Vastuuopettajat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vastuuopettajat vastuuopettajat = db.Vastuuopettajat.Find(id);
            if (vastuuopettajat == null)
            {
                return HttpNotFound();
            }
            return View(vastuuopettajat);
        }

        // GET: Vastuuopettajat/Create
        public ActionResult Create()
        {
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi");
            return View();
        }

        // POST: Vastuuopettajat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vastuuopettajan_Id,Vastuuopettajanimi,Opettajan_Id")] Vastuuopettajat vastuuopettajat)
        {
            if (ModelState.IsValid)
            {
                db.Vastuuopettajat.Add(vastuuopettajat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", vastuuopettajat.Opettajan_Id);
            return View(vastuuopettajat);
        }

        // GET: Vastuuopettajat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vastuuopettajat vastuuopettajat = db.Vastuuopettajat.Find(id);
            if (vastuuopettajat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", vastuuopettajat.Opettajan_Id);
            return View(vastuuopettajat);
        }

        // POST: Vastuuopettajat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vastuuopettajan_Id,Vastuuopettajanimi,Opettajan_Id")] Vastuuopettajat vastuuopettajat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vastuuopettajat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", vastuuopettajat.Opettajan_Id);
            return View(vastuuopettajat);
        }

        // GET: Vastuuopettajat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vastuuopettajat vastuuopettajat = db.Vastuuopettajat.Find(id);
            if (vastuuopettajat == null)
            {
                return HttpNotFound();
            }
            return View(vastuuopettajat);
        }

        // POST: Vastuuopettajat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vastuuopettajat vastuuopettajat = db.Vastuuopettajat.Find(id);
            db.Vastuuopettajat.Remove(vastuuopettajat);
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
            stringWriter.WriteLine("\"Vastuuopettajan_Id\",\"Vastuuopettajanimi\",\"Opettajan_Id\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Vastuuopettajat_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listVastuuopettajat = db.Vastuuopettajat.OrderBy(x => x.Vastuuopettajan_Id).ToList();

            foreach (var Vastuuopettaja in listVastuuopettajat)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"", Vastuuopettaja.Vastuuopettajan_Id, Vastuuopettaja.Vastuuopettajanimi, Vastuuopettaja.Opettajan_Id ));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = db.Vastuuopettajat.OrderBy(x => x.Vastuuopettajan_Id).ToList();
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Vastuuopettajat_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();


        }
    }
}
