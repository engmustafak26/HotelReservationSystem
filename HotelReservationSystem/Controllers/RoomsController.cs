using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelReservationSystem.Models;
using Microsoft.AspNet.Identity;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = RoleName.CanManageHotels)]
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string _userId;

        public RoomsController()
        {
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        // GET: Rooms
        public ActionResult Index(int? hotelId)
        {
            ViewBag.HotelId = GetHotelsSelectList();
            var rooms = db.Rooms.Where(r => r.Hotel.UserId == _userId && (hotelId == null || r.HotelId == hotelId.Value)).Include(r => r.Hotel)
                                .OrderBy(r=>r.HotelId)
                                .ToList();
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Include(r => r.Hotel).SingleOrDefault(r => r.Hotel.UserId == _userId && r.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = GetHotelsSelectList();

            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,BedsCount,PricePerNight,IsInactive,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = GetHotelsSelectList();
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.SingleOrDefault(r => r.Hotel.UserId == _userId && r.Id == id);
            if (room == null)
            {
                throw new HttpException(404, "Not found");
            }
            ViewBag.HotelId = GetHotelsSelectList();
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,BedsCount,PricePerNight,IsInactive,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = GetHotelsSelectList();
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Include(r => r.Hotel).SingleOrDefault(r => r.Hotel.UserId == _userId && r.Id == id);
            if (room == null)
            {
                throw new HttpException(404, "Not found");
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Include(r => r.Hotel).SingleOrDefault(r => r.Hotel.UserId == _userId && r.Id == id);
            if (room == null)
            {
                 throw new HttpException(404, "Not found");
            }
            try
            {
                db.Rooms.Remove(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", Resources.en.TableCanNotDeleted);
                return View(room);
            }
        }

        private SelectList GetHotelsSelectList()
        {
            return new SelectList(db.Hotels.Where(h => h.UserId == _userId)
                                                 .Select(h => new { h.Id, h.Name })
                                                 .ToList(), "Id", "Name");
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
