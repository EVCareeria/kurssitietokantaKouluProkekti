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
    public class LukukaudetController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();

        // GET: Lukukaudet
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

            var lukukaudet = db.Lukukaudet.Include(l => l.Kausiennumerot).Include(l => l.Kielet).Include(l => l.Opintojaksot).Include(l => l.Toteutuskaudet).Include(l => l.Toteutustapoja).Include(l => l.Tutkinnonosat);

            if (!String.IsNullOrEmpty(SearchedItem))
            {
                switch (SortByOrder)
                {
                    case "Opintojakso_desc":
                        lukukaudet = lukukaudet.Where(l => l.Opintojaksot.Opintojaksonnimi.Contains(SearchedItem)).OrderByDescending(l => l.Opintojaksot.Opintojaksonnimi);
                        break;
                    case "Opettaja":
                        lukukaudet = lukukaudet.Where(l => l.Opintojaksot.Opintojaksonnimi.Contains(SearchedItem)).OrderBy(l => l.Opintojaksot.Opettajat.Taydetnimet);
                        break;

                    case "Opettaja_desc":
                        lukukaudet = lukukaudet.Where(l => l.Opintojaksot.Opintojaksonnimi.Contains(SearchedItem)).OrderByDescending(l => l.Opintojaksot.Opettajat.Taydetnimet);
                        break;

                    default:
                        lukukaudet = lukukaudet.Where(l => l.Opintojaksot.Opintojaksonnimi.Contains(SearchedItem)).OrderBy(l => l.Opintojaksot.Opintojaksonnimi);
                        break;
                }
            }
            else
            {
                switch (SortByOrder)
                {
                    case "Opintojakso_desc":
                        lukukaudet = lukukaudet.OrderByDescending(l => l.Opintojaksot.Opintojaksonnimi);
                        break;
                    case "Opettajan Etunimi":
                        lukukaudet = lukukaudet.OrderBy(l => l.Opintojaksot.Opettajat.Taydetnimet);
                        break;

                    case "Opettajan Etunimi_desc":
                        lukukaudet = lukukaudet.OrderByDescending(l => l.Opintojaksot.Opettajat.Taydetnimet);
                        break;

                    default:
                        lukukaudet = lukukaudet.OrderBy(l => l.Opintojaksot.Opintojaksonnimi);
                        break;
                }

            }

            int Maxpages = (PageTotalNumber ?? 5);
            int PageNumber = (Page ?? 1);
            return View(lukukaudet.ToList().ToPagedList(PageNumber, Maxpages));
        }

        // GET: Lukukaudet/Create
        public ActionResult Create()
        {
            ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinumero_Id");
            ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi");
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi");
            ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi");
            ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi");
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi");
            return View();
        }

        // GET: Tutkinnot/_ModalCreate
        public ActionResult _ModalCreate()
        {
            //ViewBag.Lukukausi_Id = new SelectList(db.Lukukaudet, "Lukukausi_Id", "Tutkintonimi", "Aloitusvuosi");

            ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinimero");
            ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi");
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi");
            ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi");
            ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi");
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi");
            return PartialView();
            //return View();
        }

        // POST: Lukukaudet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lukukausi_Id,Kausinumero_Id,Tutkintoosa_Id,Opintojakson_Id,Päivienlukumäärä,Tuntienmäärä,Toteutustapa_Id,Toteutuskausi_Id,Pvm,Klo,Vikkonpäivä,Kieli_Id,AiheOPtilausjärjestely")] Lukukaudet lukukaudet)
        {
            db.Lukukaudet.Add(lukukaudet);
            db.SaveChanges();
            return RedirectToAction("Index");

            //if (ModelState.IsValid)
            //{
            //    db.Lukukaudet.Add(lukukaudet);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinumero_Id", lukukaudet.Kausinumero_Id);
            //ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi", lukukaudet.Kieli_Id);
            //ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", lukukaudet.Opintojakson_Id);
            //ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi", lukukaudet.Toteutuskausi_Id);
            //ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi", lukukaudet.Toteutustapa_Id);
            //ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", lukukaudet.Tutkintoosa_Id);
            //return View(lukukaudet);
        }

        // GET: Lukukaudet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lukukaudet lukukaudet = db.Lukukaudet.Find(id);
            if (lukukaudet == null)
            {
                return HttpNotFound();
            }
            ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinumero_Id", lukukaudet.Kausinumero_Id);
            ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi", lukukaudet.Kieli_Id);
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", lukukaudet.Opintojakson_Id);
            ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi", lukukaudet.Toteutuskausi_Id);
            ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi", lukukaudet.Toteutustapa_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", lukukaudet.Tutkintoosa_Id);
            return View(lukukaudet);
        }

        // GET: _ModalEdit
        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lukukaudet lukukaudet = db.Lukukaudet.Find(id);
            if (lukukaudet == null)
            {
                return HttpNotFound();
            }
            ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinumero_Id", lukukaudet.Kausinumero_Id);
            ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi", lukukaudet.Kieli_Id);
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", lukukaudet.Opintojakson_Id);
            ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi", lukukaudet.Toteutuskausi_Id);
            ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi", lukukaudet.Toteutustapa_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", lukukaudet.Tutkintoosa_Id);
            ViewBag.Lukukausi_Id = new SelectList(db.Lukukaudet, "Lukukausi_Id", "CompanyName", lukukaudet.Lukukausi_Id);
            return PartialView("_ModalEdit", lukukaudet);
        }
        // POST: Lukukaudet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lukukausi_Id,Kausinumero_Id,Tutkintoosa_Id,Opintojakson_Id,Päivienlukumäärä,Tuntienmäärä,Toteutustapa_Id,Toteutuskausi_Id,Pvm,Klo,Vikkonpäivä,Kieli_Id,AiheOPtilausjärjestely")] Lukukaudet lukukaudet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lukukaudet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Kausinumero_Id = new SelectList(db.Kausiennumerot, "Kausinumero_Id", "Kausinumero_Id", lukukaudet.Kausinumero_Id);
            ViewBag.Kieli_Id = new SelectList(db.Kielet, "Kieli_id", "Kielinimi", lukukaudet.Kieli_Id);
            ViewBag.Opintojakson_Id = new SelectList(db.Opintojaksot, "Opintojakson_Id", "Opintojaksonnimi", lukukaudet.Opintojakson_Id);
            ViewBag.Toteutuskausi_Id = new SelectList(db.Toteutuskaudet, "Toteutuskausi_Id", "Kausinimi", lukukaudet.Toteutuskausi_Id);
            ViewBag.Toteutustapa_Id = new SelectList(db.Toteutustapoja, "Toteutustapa_Id", "Toteutustapanimi", lukukaudet.Toteutustapa_Id);
            ViewBag.Tutkintoosa_Id = new SelectList(db.Tutkinnonosat, "Tutkintoosa_Id", "Tutkintoosanimi", lukukaudet.Tutkintoosa_Id);
            return View(lukukaudet);
        }

        // POST: MODAL EDIT
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "Lukukausi_Id,Kausinumero_Id,Tutkintoosa_Id,Opintojakson_Id,Päivienlukumäärä,Tuntienmäärä,Toteutustapa_Id,Toteutuskausi_Id,Pvm,Klo,Vikkonpäivä,Kieli_Id,AiheOPtilausjärjestely")] Lukukaudet lukukaudet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lukukaudet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Lukukausi_Id = new SelectList(db.Lukukaudet, "Lukukausi_Id", "CompanyName", lukukaudet.Lukukausi_Id);
            return PartialView("_ModalEdit", lukukaudet);
        }

        // GET: DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lukukaudet lukukaudet = db.Lukukaudet.Find(id);
            if (lukukaudet == null)
            {
                return HttpNotFound();
            }
            return View(lukukaudet);
        }

        // GET: MODAL DELETE
        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lukukaudet lukukaudet = db.Lukukaudet.Find(id);
            if (lukukaudet == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Opettajan_Id = new SelectList(db.Opettajat, "Opettajan_Id", "Etunimi", opettajat.Opettajan_Id);
            //return View(opettajat);
            return PartialView("_ModalDelete", lukukaudet);
        }

        // POST: Lukukaudet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lukukaudet lukukaudet = db.Lukukaudet.Find(id);
            db.Lukukaudet.Remove(lukukaudet);
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
            stringWriter.WriteLine("\"Lukukausi\",\"Kausinumero\",\"Tutkintoosa\",\"Opintojakson\",\"Päivienlukumäärä\",\"Tuntienmäärä\"," +
            "\"Toteutustapa\",\"Toteutuskausi_Id\",\"Pvm\",\"Klo\",\"Vikkonpäivä\",\"AiheOPtilausjärjestely\",\"Opettaja\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Lukukaudet_{0}.csv", DateTime.Now));
            Response.ContentType = "text/csv";

            var listLukukaudet = db.Lukukaudet.Include(l => l.Kausiennumerot).Include(l => l.Kielet).Include(l => l.Opintojaksot).Include(l => l.Toteutuskaudet).Include(l => l.Toteutustapoja).Include(l => l.Tutkinnonosat).OrderBy(x => x.Lukukausi_Id).ToList();

            foreach (var Lukukausi in listLukukaudet)
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\"",
                Lukukausi.Lukukausi_Id, Lukukausi.Kausiennumerot.Kausinimero, Lukukausi.Tutkinnonosat.Tutkintoosanimi, Lukukausi.Opintojaksot.Opintojaksonnimi, Lukukausi.Päivienlukumäärä, 
                Lukukausi.Tuntienmäärä, Lukukausi.Toteutustapoja.Toteutustapanimi, Lukukausi.Toteutuskaudet.Kausinimi, Lukukausi.Pvm, Lukukausi.Klo, Lukukausi.Vikkonpäivä, Lukukausi.AiheOPtilausjärjestely,Lukukausi.Opintojaksot.Opettajat.Taydetnimet));
            }
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        public void ExportToExcel()
        {
            var gridview = new GridView();
            gridview.DataSource = from l in db.Lukukaudet.Include(l => l.Kausiennumerot).Include(l => l.Kielet).Include(l => l.Opintojaksot).Include(l => l.Toteutuskaudet).Include(l => l.Toteutustapoja).Include(l => l.Tutkinnonosat).OrderBy(x => x.Lukukausi_Id).ToList()
                                  select new
                                  {
                                      Id = l.Lukukausi_Id,
                                      Kausi_Nro = l.Kausiennumerot.Kausinimero,
                                      Tutkintoosa = l.Tutkinnonosat.Tutkintoosanimi,
                                      Opintojakso = l.Opintojaksot.Opintojaksonnimi,
                                      Pv_Lukumäärä = l.Päivienlukumäärä,
                                      TuntiMaärä = l.Tuntienmäärä,
                                      Toteutustapa = l.Toteutustapoja.Toteutustapanimi,
                                      Toteutuskausi = l.Toteutuskaudet.Kausinimi,
                                      Päivää = l.Pvm,
                                      Aika = l.Klo,
                                      Vko_päivää = l.Vikkonpäivä,
                                      LukukausiAihe = l.AiheOPtilausjärjestely,
                                      Opettaja = l.Opintojaksot.Opettajat.Taydetnimet
                                  };
            gridview.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Lukukaudet_{0}.xls", DateTime.Now));
            Response.ContentType = "application/excel";

            var stringWriter = new StringWriter();
            var htmltextWriter = new HtmlTextWriter(stringWriter);

            gridview.RenderControl(htmltextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();


        }
    }
}
