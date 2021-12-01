using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelReservationSystem.DTOs;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;
using HotelReservationSystem.Services;
using Microsoft.AspNet.Identity;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = RoleName.CanManageHotels)]
    public class HotelOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ReservationService _reservationService;
        private string _userId;
        public HotelOrdersController()
        {
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            _reservationService = new ReservationService(_userId);
        }
        // GET: HotelOrders
        public ActionResult Index()
        {
            return View();
        }

        // GET: HotelOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_reservationService.GetOrderWithDetails(id.Value));
        }


        public ActionResult Form(int? id)
        {
            OrderDto dto = new OrderDto();
            if (id.HasValue)
            {
                dto = _reservationService.GetOrder(id.Value);
            }
            int hotelId = 0;
            var hotelSelectList = GetHotels();
            if (dto.Id == 0)
            {
                int.TryParse(hotelSelectList.FirstOrDefault()?.Value, out hotelId);
            }
            else
            {
                hotelId = dto.HotelId;
            }
            ViewBag.CustomerId = GetCustomersSelectList();
            ViewBag.HotelId = hotelSelectList;
            ViewBag.RoomId = GetRoomsSelectList(hotelId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Form([Bind(Include = "Id,CustomerId,HotelId,DateOrdered,StartDate,EndDate,RoomId")] OrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (orderDto.Id == 0)
                        _reservationService.CreateHotelOrder(orderDto);
                    else
                        _reservationService.UpdateHotelOrder(orderDto);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("",ex.Message);
                }

               
            }

            ViewBag.CustomerId = GetCustomersSelectList();
            ViewBag.HotelId = GetHotels();
            ViewBag.RoomId = GetRoomsSelectList(orderDto.HotelId);
            return View(orderDto);
        }


        [Authorize(Roles = RoleName.CanManageHotels)]
        [HttpPost]
        public JsonNetResult GetOrders(JqueryDatatableParam param)
        {
            param.Search = Request.Form.GetValues("search[value]")[0];
            int totalRecords = 0;
            var orders = _reservationService.GetHotelOrders(param, out totalRecords);
            return new JsonNetResult
            {
                Data = new
                {
                    draw = param.Draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = orders
                }
            };
        }

        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult DeleteOrder(int id)
        {
            string error = null;
            try
            {
                _reservationService.DeleteHotelOrder(id);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new { success = string.IsNullOrWhiteSpace(error), message = error });
        }

        public JsonResult GetRooms(int hotelId)
        {
            return Json(GetRoomsSelectList(hotelId), JsonRequestBehavior.AllowGet);
        }
        private SelectList GetCustomersSelectList()
        {
            return new SelectList(db.Customers.Where(h => h.UserId == _userId)
                                                 .Select(h => new { h.Id, h.Name })
                                                 .ToList(), "Id", "Name");
        }
        private SelectList GetHotels()
        {
            return new SelectList(db.Hotels.Where(h => h.UserId == _userId)
                                                 .Select(h => new { h.Id, h.Name })
                                                 .ToList(), "Id", "Name");
        }
        private SelectList GetRoomsSelectList(int hotelId)
        {
            return new SelectList(db.Rooms.Where(h => h.HotelId == hotelId)
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
