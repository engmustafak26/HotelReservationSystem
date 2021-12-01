using HotelReservationSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = RoleName.CanManageHotels)]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult New()
        {
            var customer = new Customer();

            return View("Form", customer);
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public ActionResult Edit(int id)
        {
            string userId = HttpContext.User.Identity.GetUserId();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId && c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View("Form", customer);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageHotels)]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            ModelState.Remove(nameof(Customer.UserId));
            if (!ModelState.IsValid)
            {
                return View("Form", customer);
            }

            if (customer.Id == 0)
            {
                customer.UserId = HttpContext.User.Identity.GetUserId();
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.Email=customer.Email;
               customerInDb.IsInactive = customer.IsInactive;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}