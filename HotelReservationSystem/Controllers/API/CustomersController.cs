﻿using HotelReservationSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelReservationSystem.Controllers.API
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;
        private string userId = "";
        public CustomersController()
        {
            _context = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.Where(c=> c.UserId == userId).ToList();
        }

        [Authorize(Roles = RoleName.CanManageHotels)]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId && c.Id == id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public IHttpActionResult CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customer);
        }

        [HttpPut]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public void UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == userId && c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            customerInDb.Name = customer.Name;
            customerInDb.Birthdate = customer.Birthdate;

            _context.SaveChanges();
        }

        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            try
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,Resources.en.TableCanNotDeleted));
            }
        }
    }
}
