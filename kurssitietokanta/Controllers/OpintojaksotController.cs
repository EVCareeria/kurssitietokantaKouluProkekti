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
    public class OpintojaksotController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Opintojaksot
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

            ViewBag.ShowSortedOpintojakso = String.IsNullOrEmpty(SortByOrder) ? "Opintojakso_desc" : "";
            ViewBag.ShowSortedOpttaja = SortByOrder == "Opettaja" ? "Opettaja" : "Opettaja";

           
            var opintojaksot = db.Opintojaksot.Include(o => o.Opettajat).Include(o => o.Opintomateriaalit).Include(o => o.Teknisetmateriaalit).Include(o => o.Tutkinnonosat);


            if (!String.IsNullOrEmpty(SearchedItem))
            {

                switch (SortByOrder)
                {
                    case "Opintojakso_desc":
                        opintojaksot = opintojaksot.Where(o => o.Opintojaksonnimi.Contains(SearchedItem)).OrderByDescending(o => o.Opintojaksonnimi);
                        break;
                    case "Opettaja":
                        opintojaksot = opintojaksot.Where(p => p.Opintojaksonnimi.Contains(SearchedItem)).OrderBy(o => o.Opettajat.Taydetnimet);
                        break;

                    case "Opettaja_desc":
                        opintojaksot = opintojaksot.Where(o => o.Opintojaksonnimi.Contains(SearchedItem)).OrderByDescending(o => o.Opettajat.Taydetnimet);
                        break;

                    default:
                        opintojaksot = opintojaksot.Where(o => o.Opintojaksonnimi.Contains(SearchedItem)).OrderBy(o => o.Opintojaksonnimi);
                        break;
                }
            }
            else
            {
                switch (SortByOrder)
                {
                    case "Opintojakso_desc":
                        opintojaksot = opintojaksot.OrderByDescending(o => o.Opintojaksonnimi);
                        break;
                    case "Opettajan Etunimi":
                        opintojaksot = opintojaksot.OrderBy(o => o.Opettajat.Etunimi);
                        break;

                    case "Opettajan Etunimi_desc":
                        opintojaksot = opintojaksot.OrderByDescending(o => o.Opettajat.Etunimi);
                        break;

                    default:
                        opintojaksot = opintojaksot.OrderBy(o => o.Opintojaksonnimi);
                        break;
                }

            }
            
            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);
            return View(opintojaksot.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Opintojaksot/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            if (opintojaksot == null)
            {
                return HttpNotFound();
            }
            return View(opintojaksot);
        }

        // GET: Opintojaksot/Create
        public ActionResult Create()
        {
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi");
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus");
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus");
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi");
            return PartialView();
            //return View();
        }

        // GET: _ModalCreate
        public ActionResult _ModalCreate()
        {
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi");
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus");
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus");
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi");

            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id","Opintojaksonnimi","Tavoiteet","Opintokeskinen","Tekininenmateriaali_Id","Opintomateriaali_Id");
            return PartialView();
            //return View();
        }

        // POST: Opintojaksot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Opintojakson_Id,Opintojaksonnimi,Tutkintoosa_Id,Opettajan_Id,Tavoiteet,Opintokeskinen,Tekininenmateriaali_Id,Opintomateriaali_Id")] Opintojaksot opintojaksot)
        {
            if (ModelState.IsValid)
            {
                db.Opintojaksot.Add(opintojaksot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opintojaksot.Opettajan_Id);
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", opintojaksot.Opintomateriaali_Id);
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", opintojaksot.Tekininenmateriaali_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", opintojaksot.Tutkintoosa_Id);
            return View(opintojaksot);
        }

        // GET: Opintojaksot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            if (opintojaksot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opintojaksot.Opettajan_Id);
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", opintojaksot.Opintomateriaali_Id);
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", opintojaksot.Tekininenmateriaali_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", opintojaksot.Tutkintoosa_Id);
            return View(opintojaksot);
        }

        // GET: _ModalEdit
        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            if (opintojaksot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opintojaksot.Opettajan_Id);
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", opintojaksot.Opintomateriaali_Id);
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", opintojaksot.Tekininenmateriaali_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", opintojaksot.Tutkintoosa_Id);
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", opintojaksot.Opintojakson_Id);
            return PartialView("_ModalEdit", opintojaksot);
        }

        // POST: Opintojaksot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Opintojakson_Id,Opintojaksonnimi,Tutkintoosa_Id,Opettajan_Id,Tavoiteet,Opintokeskinen,Tekininenmateriaali_Id,Opintomateriaali_Id")] Opintojaksot opintojaksot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opintojaksot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opintojaksot.Opettajan_Id);
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", opintojaksot.Opintomateriaali_Id);
            ViewBag.Tekininenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", opintojaksot.Tekininenmateriaali_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", opintojaksot.Tutkintoosa_Id);
            return View(opintojaksot);
        }

        // POST: _ModalEdit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Opintojakson_Id,Opintojaksonnimi,Tutkintoosa_Id,Opettajan_Id,Tavoiteet,Opintokeskinen,Tekininenmateriaali_Id,Opintomateriaali_Id")] Opintojaksot opintojaksot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opintojaksot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", opintojaksot.Opintojakson_Id);
            return PartialView("_ModalEdit", opintojaksot);
        }

        // GET: DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            if (opintojaksot == null)
            {
                return HttpNotFound();
            }
            return View(opintojaksot);
        }

        // GET: MODAL DELETE
        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            if (opintojaksot == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opettajat.Opettajan_Id);
            //return View(opettajat);
            return PartialView("_ModalDelete", opintojaksot);
        }

        // POST: DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            db.Opintojaksot.Remove(opintojaksot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: MODAL DELETE
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Opintojaksot opintojaksot = db.Opintojaksot.Find(id);
            db.Opintojaksot.Remove(opintojaksot);
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
            stringWriter.WriteLine("\"Id\",\"Opintojaksonnimi\",\"Tutkintoosa\",\"Opettajan \",\"Tavoiteet\",\"Opintokeskinen\",\"Tekininenmateriaali\",\"Opintomateriaali\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opintojaksot_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listOpintojaksot = db.Opintojaksot.Include(o => o.Opettajat).Include(o => o.Opintomateriaalit).Include(o => o.Teknisetmateriaalit).Include(o => o.Tutkinnonosat).OrderBy(x => x.Opintojakson_Id).ToList();

            foreach (var Opintojakson in listOpintojaksot)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", 
                Opintojakson.Opintojakson_Id, Opintojakson.Opintojaksonnimi, Opintojakson.Tutkinnonosat.Tutkintoosanimi, 
                Opintojakson.Opettajat.Taydetnimet, Opintojakson.Tavoiteet, Opintojakson.Opintokeskinen, 
                Opintojakson.Teknisetmateriaalit.Kuvaus, Opintojakson.Opintomateriaalit.Kuvaus));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from o in db.Opintojaksot.Include(o => o.Opettajat).Include(o => o.Opintomateriaalit).Include(o => o.Teknisetmateriaalit).Include(o => o.Tutkinnonosat).OrderBy(x => x.Opintojakson_Id).ToList()
                                  select new
                                  {
                                      Id = o.Opintojakson_Id,
                                      Opintojakso = o.Opintojaksonnimi,
                                      Tutkinnonosa = o.Tutkinnonosat.Tutkintoosanimi,
                                      Opettaja = o.Opettajat.Taydetnimet,
                                      Tavoite = o.Tavoiteet,
                                      OpintoKeskinen = o.Opintokeskinen,
                                      TeknisetMateriaalit = o.Teknisetmateriaalit.Kuvaus,
                                      OpintoMateriaalit = o.Opintomateriaalit.Kuvaus
                                  };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opintojaksot_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();


        }
    }
}
