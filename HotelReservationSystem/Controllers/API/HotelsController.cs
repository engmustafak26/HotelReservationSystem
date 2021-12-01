using AutoMapper;
using HotelReservationSystem.DTOs;
using HotelReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace HotelReservationSystem.Controllers.API
{
    public class HotelsController : ApiController
    {
        private ApplicationDbContext _context;
        private string _userId;
        public HotelsController()
        {
            _context = new ApplicationDbContext();
            _userId = User.Identity.GetUserId();
        }

        [AllowAnonymous]
        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels.Where(h => h.UserId == _userId)
                .Include(c => c.Country)
                .ToList()
                .Select(Mapper.Map<Hotel, HotelDto>);
        }

        [AllowAnonymous]
        public IHttpActionResult GetHotel(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.UserId == _userId && c.Id == id);

            if (hotel == null)
                return NotFound();

            return Ok(Mapper.Map<Hotel, HotelDto>(hotel));
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public IHttpActionResult CreateHotel(HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var hotel = Mapper.Map<HotelDto, Hotel>(hotelDto);
            hotel.UserId = _userId;
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            hotelDto.Id = hotel.Id;

            return Created(new Uri(Request.RequestUri + "/" + hotel.Id), hotelDto);
        }

        [HttpPut]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public void UpdateHotel(int id, HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var hotelInDb = _context.Hotels.SingleOrDefault(c => c.UserId == _userId && c.Id == id);

            if (hotelInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map<HotelDto, Hotel>(hotelDto, hotelInDb);
           hotelInDb.UserId= _userId;

            _context.SaveChanges();
        }

        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageHotels)]
        public void DeleteHotel(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == id);

            if (hotel == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            try
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, Resources.en.TableCanNotDeleted));

            }

        }
    }
}
