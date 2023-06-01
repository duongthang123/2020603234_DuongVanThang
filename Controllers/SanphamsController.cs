using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _2020603234.Models;
using PagedList;

namespace _2020603234.Controllers
{
    public class SanphamsController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index(String searchValue, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.currentSort = sortOrder;
            ViewBag.sapXepTheoTen = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.sapXepTheoGia = sortOrder == "Gia" ? "price_desc" : "";
            var sanphams = db.Sanphams.Include(s => s.Danhmuc);
            if(searchValue != null)
            {
                page = 1;
            } else
            {
                searchValue = currentFilter;
            }

            ViewBag.currentFillter = searchValue;
            if (!String.IsNullOrEmpty(searchValue))
            {
                if(decimal.TryParse(searchValue, out decimal price))
                {
                    sanphams = sanphams.Where(p => p.Giatien == price);
                } else
                {
                    sanphams = sanphams.Where(p => p.Tenvd.Contains(searchValue));
                }
            }


            switch(sortOrder)
            {
                case "name_desc":
                    sanphams = db.Sanphams.OrderByDescending(p => p.Tenvd); break;
                case "price_desc":
                    sanphams = db.Sanphams.OrderByDescending(p => p.Giatien);break;
                default: sanphams = sanphams.OrderByDescending(p => p.Soluong); break;
            }

            int PageSize = 4;
            int PageNumber = (page ?? 1);
            return View(sanphams.ToPagedList(PageNumber, PageSize));
        }

        [Route("shop/danhmuc/{MaDanhmuc?}")]
        public ActionResult ProductByCate(int MaDanhmuc)
        {
            return View(db.Sanphams.Where(p => p.MaDanhmuc == MaDanhmuc).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }


        // giai
        // GET: Sanphams/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc");
            return View();
        }

        // POST: Sanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Mavd,Tenvd,TenAnh,Mota,Giatien,Soluong,MaDanhmuc")] Sanpham sanpham)
        {
            try
            {
                sanpham.TenAnh = "";
                var f = Request.Files["ImageFile"];

                if(f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UpLoadFile = Server.MapPath("~/Content/Images/" + FileName);
                    f.SaveAs(UpLoadFile);
                    sanpham.TenAnh = FileName;
                }
                db.Sanphams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "Cos looix " + ex.ToString();
                ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
                return View(sanpham);

            }
            

        }

        // GET: Sanphams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }

        // POST: Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Mavd,Tenvd,TenAnh,Mota,Giatien,Soluong,MaDanhmuc")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }

        // GET: Sanphams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // POST: Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sanpham sanpham = db.Sanphams.Find(id);
            db.Sanphams.Remove(sanpham);
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
