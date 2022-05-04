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
    public class TutkinnotController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Tutkinnot
        public ActionResult Index(string SearchedItem, string SortByOrder, int? Page, int? PageTotalNumber, string PageItems)
        {
            var tutkinnot = db.Tutkinnot.Include(t => t.Tutkintoalueet);

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

            var Tutkinnot = from p in db.Tutkinnot
                            select p;

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                Tutkinnot = Tutkinnot.Where(p => p.Tutkintonimi.Contains(SearchedItem));
            }

            ViewBag.ShowSortedOpettajat = String.IsNullOrEmpty(SortByOrder) ? "Tutkintonimi_desc" : "";

            switch (SortByOrder)
            {
                case "Tutkintonimi_desc":
                    Tutkinnot = Tutkinnot.OrderByDescending(p => p.Tutkintonimi);
                    break;

                default:
                    Tutkinnot = Tutkinnot.OrderBy(p => p.Tutkintonimi);
                    break;
            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);

            ViewBag.LoginError = 1;
            return View(Tutkinnot.ToPagedList(PageNumber, Maxpages));

        }

        // GET: Tutkinnot/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
        //    if (tutkinnot == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tutkinnot);
        //}

        // GET: Tutkinnot/Create
        public ActionResult Create()
        {
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkintoalueet, "Tutkinto_Id", "Tutkintoaluenimi");
            return View();
        }

        // GET: Tutkinnot/_ModalCreate
        public ActionResult _ModalCreate()
        {
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", "Aloitusvuosi");
            return PartialView();
            //return View();
        }

        // POST: Tutkinnot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutkinto_Id,Tutkintonimi,Aloitusvuosi,Päättymisvuosi,Kesto,OSP")] Tutkinnot tutkinnot)
        {
            if (ModelState.IsValid)
            {
                db.Tutkinnot.Add(tutkinnot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", "Aloitusvuosi", "Päättymisvuosi", "Kesto", "OSP");
            ViewBag.LoginError = 1;
            //tutkinnot.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana";
            return View("Index", tutkinnot);
            //return PartialView();
            //return PartialView("_ModalCreate", tutkinnot);
        }

        // GET: Tutkinnot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            if (tutkinnot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkintoalueet, "Tutkinto_Id", "Tutkintoaluenimi", tutkinnot.Tutkinto_Id);
            return View(tutkinnot);
        }

        // GET: Orders/Edit/5/_ModalEdit
        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            if (tutkinnot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opettajan_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "CompanyName", tutkinnot.Tutkinto_Id);
            return PartialView("_ModalEdit", tutkinnot);
        }

        // POST: Tutkinnot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tutkinto_Id,Tutkintonimi,Aloitusvuosi,Päättymisvuosi,Kesto,OSP")] Tutkinnot tutkinnot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutkinnot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tutkinto_Id = new SelectList(db.Tutkintoalueet, "Tutkinto_Id", "Tutkintoaluenimi", tutkinnot.Tutkinto_Id);
            return View(tutkinnot);
        }

        // POST: MODAL EDIT
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Tutkinto_Id,Tutkintonimi,Aloitusvuosi,Päättymisvuosi,Kesto,OSP")] Tutkinnot tutkinnot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutkinnot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opettajan_Id = new SelectList(db.Tutkinnot, "Tutkinto_Id", "Tutkintonimi", "Aloitusvuosi", tutkinnot.Tutkinto_Id);
            ViewBag.LoginError = 1;
            //return PartialView("_ModalEdit", tutkinnot);
            return View("Index", tutkinnot);
        }

        // GET: Tutkinnot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            if (tutkinnot == null)
            {
                return HttpNotFound();
            }
            return View(tutkinnot);
        }

        // GET: MODAL DELETE
        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            if (tutkinnot == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opettajat.Opettajan_Id);
            //return View(opettajat);
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", "Sukunimi", tutkinnot.Tutkinto_Id);
            return PartialView("_ModalDelete", tutkinnot);
        }

        // POST: Tutkinnot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            db.Tutkinnot.Remove(tutkinnot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Tutkinnot tutkinnot = db.Tutkinnot.Find(id);
            db.Tutkinnot.Remove(tutkinnot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFromJQuery(string id)
        {
            tiimi4Entities1 db = new tiimi4Entities1();
            int iid = int.Parse(id);
            // etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Tutkinnot dbItem = (from o in db.Tutkinnot
                             where o.Tutkinto_Id == iid
                             select o).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                db.Tutkinnot.Remove(dbItem);
                db.SaveChanges();
                OK = true;
            }
            db.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
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
            stringWriter.WriteLine("\"Tutkinto_Id\",\"Tutkintonimi\",\"Aloitusvuosi\",\"Päättymisvuosi\",\"Kesto\",\"OSP\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition",string.Format("attachment;filename=Tutkinnto_{0}.csv",DateTime.Now));
            Response.ContentType = "text/csv";

            var listTutkinnto = db.Tutkinnot.OrderBy(x => x.Tutkinto_Id).ToList();

            foreach(var tutinto in listTutkinnto)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", tutinto.Tutkinto_Id, tutinto.Tutkintonimi, tutinto.Aloitusvuosi, tutinto.Päättymisvuosi, tutinto.Kesto, tutinto.OSP));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = db.Tutkinnot.OrderBy(x => x.Tutkinto_Id).ToList();
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Tutkinnto_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();


        }

    }
}
