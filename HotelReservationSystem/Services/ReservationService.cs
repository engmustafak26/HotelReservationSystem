using HotelReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelReservationSystem.DTOs;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.Entity.SqlServer;
using System.Globalization;

namespace HotelReservationSystem.Services
{
    public class ReservationService
    {
        ApplicationDbContext _context;
        string _userId;

        public ReservationService(string userId)
        {
            _context = new ApplicationDbContext();
            _userId = userId;
        }

        public IEnumerable<Order> GetHotelOrders(JqueryDatatableParam param, out int totalRecords)
        {
            var ordersQuery = _context.Orders.Include(c => c.HotelCustomer).Include(c => c.Hotel).Include(c => c.Room)
                                  .Where(c => c.Hotel.UserId == _userId);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                string searchWord = param.Search.Trim().ToLower();
                DateTime searchDate;
                DateTime.TryParseExact(searchWord, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate);
                decimal searchNumeric;
                decimal.TryParse(searchWord, out searchNumeric);

                ordersQuery = ordersQuery.Where(o => o.Id == searchNumeric ||
                                                     o.NumberOfDays == searchNumeric ||
                                                     o.HotelCustomer.Name.ToLower().Contains(searchWord) ||
                                                     o.Hotel.Name.ToLower().Contains(searchWord) ||
                                                     o.Room.Name.ToLower().Contains(searchWord) ||
                                                     o.StartDate == searchDate ||
                                                     o.EndDate == searchDate ||
                                                     o.DateOrdered == searchDate ||
                                                     o.FullPrice == searchNumeric);
            }

            totalRecords = ordersQuery.Count();

            return ordersQuery.OrderByDescending(c => c.Id)
                              .Skip(param.Start)
                              .Take(param.Length)
                              .ToList();
        }
        public OrderDto GetOrder(int id)
        {
            Order order = _context.Orders.Single(c => c.Id == id);
            OrderDto dto = new OrderDto()
            {
                Id = order.Id,
                CustomerId = order.HotelCustomerId,
                HotelId = order.HotelId,
                RoomId = order.RoomId,
                DateOrdered = order.DateOrdered,
                StartDate = order.StartDate,
                EndDate = order.EndDate
            };
            return dto;
        }

        public Order GetOrderWithDetails(int id)
        {
            return _context.Orders.Include(o => o.HotelCustomer)
                                  .Include(o => o.Hotel)
                                  .Include(o => o.Room)
                                  .Include(o => o.UpdatedByHotelAdminUser)
                                  .Include(o => o.CancelUser)
                                  .Single(c => c.Id == id);

        }

        public int CreateHotelOrder(OrderDto newOrder)
        {

            var room = _context.Rooms.Single(c => c.Id == newOrder.RoomId);

            var numOfDays = Convert.ToInt32((newOrder.EndDate - newOrder.StartDate).TotalDays);

            var fullPrice = (decimal)Math.Round((room.PricePerNight * numOfDays), 2);

            EnsureReservationHasNoConflict(newOrder, room);

            var order = new Order()
            {
                HotelCustomerId = newOrder.CustomerId,
                HotelId = newOrder.HotelId,
                RoomId = newOrder.RoomId,
                DateOrdered = newOrder.DateOrdered,
                StartDate = newOrder.StartDate,
                EndDate = newOrder.EndDate,
                NumberOfDays = numOfDays,
                FullPrice = fullPrice,
                UpdatedByHotelAdminUserId = _userId
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            return order.Id;
        }


        public void UpdateHotelOrder(OrderDto order)
        {


            var orderInDb = _context.Orders.SingleOrDefault(c => c.Id == order.Id);

            if (orderInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!string.IsNullOrWhiteSpace(orderInDb.RequesterUserId))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Can not edit user reserved order")
                });


            var room = _context.Rooms.Single(c => c.Id == order.RoomId);

            EnsureReservationHasNoConflict(order, room);

            var numOfDays = Convert.ToInt32((order.EndDate - order.StartDate).TotalDays);

            var fullPrice = (decimal)Math.Round((room.PricePerNight * numOfDays), 2);

            orderInDb.HotelCustomerId = order.CustomerId;
            orderInDb.HotelId = order.HotelId;
            orderInDb.RoomId = order.RoomId;
            orderInDb.DateOrdered = order.DateOrdered;
            orderInDb.StartDate = order.StartDate;
            orderInDb.EndDate = order.EndDate;
            orderInDb.FullPrice = fullPrice;
            orderInDb.NumberOfDays = numOfDays;
            orderInDb.UpdatedByHotelAdminUserId = _userId;

            _context.SaveChanges();
        }

        public void DeleteHotelOrder(int id)
        {
            var order = _context.Orders.SingleOrDefault(c => c.Id == id);




            if (!string.IsNullOrWhiteSpace(order.RequesterUserId))
                throw new Exception("You Can not delete this order, instead cancel it");
                

            try
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.en.TableCanNotDeleted);
            }
        }




        private void EnsureReservationHasNoConflict(OrderDto newOrder, Room room)
        {
            var conflictExistOrders = _context.Orders.Include(o => o.HotelCustomer)
                                              .Where(o => o.Id != newOrder.Id && o.RoomId == room.Id && o.CancelDate == null &&
                                                      ((newOrder.StartDate >= o.StartDate && newOrder.StartDate <= o.EndDate) ||
                                                       (newOrder.EndDate >= o.StartDate && newOrder.EndDate <= o.EndDate)))
                                              .ToList();
            if (conflictExistOrders.Any())
            {
                string errorMessage = string.Join("<br/>", conflictExistOrders
                                                 .Select(x => $" - Order Number: ({x.Id})  Customer: ({x.HotelCustomer.Name})  Start date ({x.StartDate.ToString("dd/MM/yyyy")})  End date ({x.EndDate.ToString("dd/MM/yyyy")})")
                                                  .ToArray());
                throw new Exception($"<span style='font-weight: bold;'> Reservation Order conflict with exisiting reservations: </span> <br/> {errorMessage} ");
                
            }
        }
    }
}