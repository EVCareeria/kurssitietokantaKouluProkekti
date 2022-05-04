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
    public class OpettajatController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Opettajat
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

            var Opettajat = from p in db.Opettajat
                           select p;

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                Opettajat = Opettajat.Where(p => p.Taydetnimet.Contains(SearchedItem));
            }
            
                
           ViewBag.ShowSortedOpettajat = String.IsNullOrEmpty(SortByOrder) ? "Opettaja_desc" : "";
            

            switch (SortByOrder)
            {
                case "Opettaja_desc":
                    Opettajat = Opettajat.OrderByDescending(p => p.Taydetnimet);
                    break;

                default:
                    Opettajat = Opettajat.OrderBy(p => p.Taydetnimet);
                    break;
            }
            
            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);

            return View(Opettajat.ToPagedList(PageNumber, Maxpages));
            
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
            return View();
        }

        // GET: Opettajat/Create
        public ActionResult _ModalCreate()
        {
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", "Sukunimi", "Email", "Puh");
            //ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            //ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");
            return PartialView();
            //return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Opettajan_Id, Etunimi, Sukunimi, Email, Puh")] Opettajat opettajat)
        {
            if (ModelState.IsValid)
            {
                db.Opettajat.Add(opettajat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opettajat.Opettajan_Id);
            //ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            //ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
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
            return View(opettajat);
        }

        // GET: Orders/Edit/5
        public ActionResult _ModalEdit(int? id)
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
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "CustomerID", "CompanyName", opettajat.Opettajan_Id);
            //ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            //ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return PartialView("_ModalEdit", opettajat);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Opettajan_Id,Etunimi,Sukunimi,Email,Puh")] Opettajat opettajat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opettajat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", "Sukunimi", opettajat.Opettajan_Id);
            //ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            //ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return PartialView("_ModalEdit", opettajat);
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

      
        // GET: Opettajat/Delete/5 -- MODAL DELETE
        public ActionResult _ModalDelete(int? id)
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
            //ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opettajat.Opettajan_Id);
            //return View(opettajat);
            return PartialView("_ModalDelete", opettajat);
        }

        //POST: Opettajat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opettajat opettajat = db.Opettajat.Find(id);
            db.Opettajat.Remove(opettajat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Opettajat opettajat = db.Opettajat.Find(id);
            db.Opettajat.Remove(opettajat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFromJQuery(string id)
        {
            tiimi4Entities1 db = new tiimi4Entities1();
            int iid = int.Parse(id);
            // etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Opettajat dbItem = (from o in db.Opettajat
                             where o.Opettajan_Id == iid
                             select o).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                db.Opettajat.Remove(dbItem);
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
            stringWriter.WriteLine("\"Opettajan_Id\",\"Etunimi\",\"Sukunimi\",\"Email\",\"Puh\",\"Taydetnimet\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Opettajat_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listOpettajat = db.Opettajat.OrderBy(x => x.Opettajan_Id).ToList();

            foreach (var opettaja in listOpettajat)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", opettaja.Opettajan_Id, opettaja.Etunimi, opettaja.Sukunimi, opettaja.Email, opettaja.Puh, opettaja.Taydetnimet));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = db.Opettajat.OrderBy(x => x.Opettajan_Id).ToList();
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
