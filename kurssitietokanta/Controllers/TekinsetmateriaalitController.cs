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
    public class TeknisetmateriaalitController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Teknisetmateriaalit
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

            return View(db.Teknisetmateriaalit.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Teknisetmateriaalit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            if (teknisetmateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(teknisetmateriaalit);
        }

        // GET: Teknisetmateriaalit/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult _ModalCreate()
        {
            return PartialView("_ModalCreate");
        }

        // POST: Teknisetmateriaalit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tekininenmateriaali_Id,Kuvaus")] Teknisetmateriaalit teknisetmateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Teknisetmateriaalit.Add(teknisetmateriaalit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teknisetmateriaalit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalCreate([Bind(Include = "Tekininenmateriaali_Id,Kuvaus")] Teknisetmateriaalit teknisetmateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Teknisetmateriaalit.Add(teknisetmateriaalit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView("_ModalCreate", teknisetmateriaalit);
        }

        // GET: Teknisetmateriaalit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            if (teknisetmateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(teknisetmateriaalit);
        }

        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            if (teknisetmateriaalit == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalEdit", teknisetmateriaalit);
        }

        // POST: Teknisetmateriaalit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tekininenmateriaali_Id,Kuvaus")] Teknisetmateriaalit teknisetmateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teknisetmateriaalit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teknisetmateriaalit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Tekininenmateriaali_Id,Kuvaus")] Teknisetmateriaalit teknisetmateriaalit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teknisetmateriaalit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_ModalEdit", teknisetmateriaalit);
        }

        // GET: Teknisetmateriaalit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            if (teknisetmateriaalit == null)
            {
                return HttpNotFound();
            }
            return View(teknisetmateriaalit);
        }

        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            if (teknisetmateriaalit == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalDelete", teknisetmateriaalit);
        }

        // POST: Teknisetmateriaalit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            db.Teknisetmateriaalit.Remove(teknisetmateriaalit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Teknisetmateriaalit teknisetmateriaalit = db.Teknisetmateriaalit.Find(id);
            db.Teknisetmateriaalit.Remove(teknisetmateriaalit);
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
