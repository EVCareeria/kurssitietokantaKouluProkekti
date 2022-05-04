using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kurssitietokanta.Models;

namespace kurssitietokanta.Controllers
{
    public class OpettajatController : Controller
    {
        private tiimi4Entities db = new tiimi4Entities();

        // GET: Opettajat
        public ActionResult Index()
        {
            var opettajat = db.Opettajat.Include(o => o.Vastuuopettajat);
            return View(opettajat.ToList());
        }

        // GET: Opettajat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opettajat opettajat = db.Opettajat.Find(id);
            if (opettajat == null)
            {
                return HttpNotFound();
            }
            return View(opettajat);
        }

        // GET: Opettajat/Create
        public ActionResult Create()
        {
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Opettajan_Id", "Vastuuopettajanimi");
            return View();
        }

        // POST: Opettajat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Opettajan_Id,Etunimi,Sukunimi,Email,Puh")] Opettajat opettajat)
        {
            if (ModelState.IsValid)
            {
                db.Opettajat.Add(opettajat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Opettajan_Id", "Vastuuopettajanimi", opettajat.Opettajan_Id);
            return View(opettajat);
        }

        // GET: Opettajat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opettajat opettajat = db.Opettajat.Find(id);
            if (opettajat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Opettajan_Id", "Vastuuopettajanimi", opettajat.Opettajan_Id);
            return View(opettajat);
        }

        // POST: Opettajat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Opettajan_Id,Etunimi,Sukunimi,Email,Puh")] Opettajat opettajat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opettajat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opettajan_Id = new SelectList(db.Vastuuopettajat, "Opettajan_Id", "Vastuuopettajanimi", opettajat.Opettajan_Id);
            return View(opettajat);
        }

        // GET: Opettajat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opettajat opettajat = db.Opettajat.Find(id);
            if (opettajat == null)
            {
                return HttpNotFound();
            }
            return View(opettajat);
        }

        // POST: Opettajat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opettajat opettajat = db.Opettajat.Find(id);
            db.Opettajat.Remove(opettajat);
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
