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
    public class OpintomateriaalitController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Opintomateriaalit
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
            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);

            return View(db.Opintomateriaalit.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Opintomateriaalit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintomateriaalit opintomateriaalit = db.Opintomateriaalit.Find(id);
            if (opintomateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(opintomateriaalit);
        }

        // GET: Opintomateriaalit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Opintomateriaalit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Opintomateriaali_Id,Kuvaus")] Opintomateriaalit opintomateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Opintomateriaalit.Add(opintomateriaalit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opintomateriaalit);
        }

        // GET: Opintomateriaalit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintomateriaalit opintomateriaalit = db.Opintomateriaalit.Find(id);
            if (opintomateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(opintomateriaalit);
        }

        // POST: Opintomateriaalit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Opintomateriaali_Id,Kuvaus")] Opintomateriaalit opintomateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opintomateriaalit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opintomateriaalit);
        }

        // GET: Opintomateriaalit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintomateriaalit opintomateriaalit = db.Opintomateriaalit.Find(id);
            if (opintomateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(opintomateriaalit);
        }

        // POST: Opintomateriaalit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opintomateriaalit opintomateriaalit = db.Opintomateriaalit.Find(id);
            db.Opintomateriaalit.Remove(opintomateriaalit);
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
            stringWriter.WriteLine("\"Opintomateriaali_Id\",\"Kuvaus\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opintomateriaalit_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listOpintomateriaalit = db.Opintomateriaalit.OrderBy(o => o.Opintomateriaali_Id).ToList();

            foreach (var Opintomateriaali in listOpintomateriaalit)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\"", Opintomateriaali.Opintomateriaali_Id, Opintomateriaali.Kuvaus));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from o in db.Opintomateriaalit.OrderBy(o => o.Opintomateriaali_Id).ToList()
                                  select new
                                  {
                                      Id = o.Opintomateriaali_Id,
                                      KUVAUS = o.Kuvaus,
                                  };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opintomateriaalit_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }
}
