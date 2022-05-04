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

namespace kurssitietokanta.Controllers
{
    public class KurssimateriaalitController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Kurssimateriaalit
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

            var kurssimateriaalit = db.Kurssimateriaalit.Include(k => k.Opintomateriaalit).Include(k => k.Teknisetmateriaalit);

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                kurssimateriaalit = kurssimateriaalit.Where(k => k.Opintomateriaalit.Kuvaus.Contains(SearchedItem));
            }


            ViewBag.ShowSortedOpintomateriaalitKuvaus = String.IsNullOrEmpty(SortByOrder) ? "OpintomateriaalitKuvaus_desc" : "";


            switch (SortByOrder)
            {
                case "OpintomateriaalitKuvaus_desc":
                    kurssimateriaalit = kurssimateriaalit.OrderByDescending(k => k.Opintomateriaalit.Kuvaus);
                    break;

                default:
                    kurssimateriaalit = kurssimateriaalit.OrderBy(k => k.Opintomateriaalit.Kuvaus);
                    break;
            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);
            return View(kurssimateriaalit.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Kurssimateriaalit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            if (kurssimateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(kurssimateriaalit);
        }

        // GET: Kurssimateriaalit/Create
        public ActionResult Create()
        {
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus");
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus");
            return View();
        }

        public ActionResult _ModalCreate()
        {
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus");
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus");
            return PartialView();
        }

        // POST: Kurssimateriaalit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Kurssimateriaali_Id,Opintomateriaali_Id,Tekninenmateriaali_Id")] Kurssimateriaalit kurssimateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Kurssimateriaalit.Add(kurssimateriaalit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Opintomateriaali_Id = new SelectList(db.Kurssimateriaalit, "Opintomateriaali_Id", "Kuvaus", kurssimateriaalit.Opintomateriaali_Id);
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Kurssimateriaalit, "Tekininenmateriaali_Id", "Kuvaus", kurssimateriaalit.Tekninenmateriaali_Id);
            return View(kurssimateriaalit);
        }

        // GET: Kurssimateriaalit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            if (kurssimateriaalit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", kurssimateriaalit.Opintomateriaali_Id);
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", kurssimateriaalit.Tekninenmateriaali_Id);
            return View(kurssimateriaalit);
        }

        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            if (kurssimateriaalit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", kurssimateriaalit.Opintomateriaali_Id);
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", kurssimateriaalit.Tekninenmateriaali_Id);
            return PartialView("_ModalEdit", kurssimateriaalit);
        }

        // POST: Kurssimateriaalit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Kurssimateriaali_Id,Opintomateriaali_Id,Tekninenmateriaali_Id")] Kurssimateriaalit kurssimateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kurssimateriaalit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", kurssimateriaalit.Opintomateriaali_Id);
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", kurssimateriaalit.Tekninenmateriaali_Id);
            return View(kurssimateriaalit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Kurssimateriaali_Id,Opintomateriaali_Id,Tekninenmateriaali_Id")] Kurssimateriaalit kurssimateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kurssimateriaalit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opintomateriaali_Id = new SelectList(db.Opintomateriaalit, "Opintomateriaali_Id", "Kuvaus", kurssimateriaalit.Opintomateriaali_Id);
            ViewBag.Tekninenmateriaali_Id = new SelectList(db.Teknisetmateriaalit, "Tekininenmateriaali_Id", "Kuvaus", kurssimateriaalit.Tekninenmateriaali_Id);
            return PartialView("_ModalEdit", kurssimateriaalit);
        }

        // GET: Kurssimateriaalit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            if (kurssimateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(kurssimateriaalit);
        }

        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            if (kurssimateriaalit == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalDelete", kurssimateriaalit);
        }

        // POST: Kurssimateriaalit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            db.Kurssimateriaalit.Remove(kurssimateriaalit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModelDeleteConfirmed(int id)
        {
            Kurssimateriaalit kurssimateriaalit = db.Kurssimateriaalit.Find(id);
            db.Kurssimateriaalit.Remove(kurssimateriaalit);
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
    }
}
