using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = RoleName.CanManageHotels)]
    public class HotelsController : Controller
    {
        private ApplicationDbContext _context;
        private string _userId;
        public HotelsController()
        {
            _context = new ApplicationDbContext();
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageHotels))
                return View("List");

            return View("ReadOnlyList");
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult New()
        {
            var countries = _context.Countries.ToList();

            var viewModel = new HotelViewModel()
            {
                Hotel = new Hotel(),
                Countries = countries
            };

            return View("Form", viewModel);
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult Edit(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(h => h.UserId == _userId && h.Id == id);

            if (hotel == null)
                return HttpNotFound();

            var viewModel = new HotelViewModel()
            {
                Hotel = hotel,
                Countries = _context.Countries.ToList()
            };

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult Save(Hotel hotel)
        {
            ModelState.Remove("hotel.UserId");
            if (!ModelState.IsValid)
            {
                var viewModel = new HotelViewModel()
                {
                    Hotel = hotel,
                    Countries = _context.Countries.ToList()
                };

                return View("Form", viewModel);
            }

            if (hotel.Id == 0)
            {
                hotel.UserId = _userId;
                _context.Hotels.Add(hotel);
            }
            else
            {
                var hotelInDb = _context.Hotels.Single(c => c.UserId == _userId && c.Id == hotel.Id);
                hotelInDb.Name = hotel.Name;
                hotelInDb.City = hotel.City;
                hotelInDb.CountryId = hotel.CountryId;
                hotelInDb.Stars = hotel.Stars;
                hotelInDb.FreeCancelationDaysBeforeReservationDate = hotel.FreeCancelationDaysBeforeReservationDate;
                hotelInDb.DeductionPercentageForReservationCancelation=hotel.DeductionPercentageForReservationCancelation;
                hotelInDb.CheckinTime=hotel.CheckinTime;
                hotelInDb.IsInactive = hotel.IsInactive;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Hotels");
        }

        public ActionResult NewCountry()
        {
            var country = new Country();

            return View("NewCountryForm", country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult SaveCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View("NewCountryForm", country);
            }

            if (country.Id == 0)
                _context.Countries.Add(country);
            else
            {
                var countryInDb = _context.Countries.Single(c => c.Id == country.Id);
                countryInDb.Name = country.Name;
            }

            _context.SaveChanges();

            return RedirectToAction("New", "Hotels");
        }
    }
}