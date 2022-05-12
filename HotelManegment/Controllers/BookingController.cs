using HotelManegment.Models;
using HotelManegment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManegment.Controllers
{
    public class BookingController : Controller
    {
        private HotelDbEntities1 objHotelDbEntities;
        public BookingController ()
        {
            objHotelDbEntities = new HotelDbEntities1 ();
        }
        public ActionResult Index()
        {
            BookingViewModel objBookingViewModel = new BookingViewModel();
            objBookingViewModel.ListOfRooms = (from objRoom in objHotelDbEntities.Rooms
                                               where objRoom.BookingStatusId == 2
                                               select new SelectListItem()
                                               {
                                                   Text = objRoom.RoomNumber,
                                                   Value = objRoom.RoomId.ToString()
                                               }
                                              ).ToList();
            objBookingViewModel.BookingForm = DateTime.Now;
            objBookingViewModel.BookingTo = DateTime.Now.AddDays(1);
            return View(objBookingViewModel);
        }
        [HttpPost]
        public ActionResult Index(BookingViewModel objBookingViewModel)
        {
            int numberOfDays = Convert.ToInt32((objBookingViewModel.BookingTo - objBookingViewModel.BookingForm).TotalDays);
            Room objRoom = objHotelDbEntities.Rooms.Single(model => model.RoomId == objBookingViewModel.AssignRoomId);
            decimal RoomPrice=objRoom.RoomPrice;
            decimal TotalAmount = RoomPrice * numberOfDays;
            RoomBooking roomBooking = new RoomBooking()
            {
                BookingForm = objBookingViewModel.BookingForm,
                BookingTo = objBookingViewModel.BookingTo,
                AssignRoomId = objBookingViewModel.AssignRoomId,
                CustomreAddress = objBookingViewModel.CustomerAddress,
                CustomerName = objBookingViewModel.CustomerName,
                CustomerPhone = objBookingViewModel.CustomerPhone,
                NoOfMembers = objBookingViewModel.NoOfMembers,
                TotalAmount = TotalAmount,

            };
            objHotelDbEntities.RoomBookings.Add(roomBooking);
            objHotelDbEntities.SaveChanges();

            objRoom.BookingStatusId = 3;
            objHotelDbEntities.SaveChanges();

            return Json(data: new {message="Hotel Booking Is Succesfully Created.",success=true}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetAllBookingHistory()
        {
        List<RoomBookingViewModel>listOfBookingViewModels=new List<RoomBookingViewModel>();
            listOfBookingViewModels=(from objHotelBooking in objHotelDbEntities.RoomBookings
                                    join objRoom in objHotelDbEntities.Rooms on objHotelBooking.AssignRoomId equals objRoom.RoomId
                                    select new RoomBookingViewModel()
                                    {
                                        BookingForm=objHotelBooking.BookingForm,
                                        BookingTo=objHotelBooking.BookingTo,
                                        CustomerPhone=objHotelBooking.CustomerPhone,
                                        CustomerName=objHotelBooking.CustomerName,
                                        TotalAmount=objHotelBooking.TotalAmount,
                                        CustomerAddress=objHotelBooking.CustomreAddress,
                                        NoOfMembers=objHotelBooking.NoOfMembers,
                                        BookingId=objHotelBooking.BookingId,
                                        RoomNumber=objRoom.RoomNumber,
                                        RoomPrice=objRoom.RoomPrice,
                                        NumberOfDays=System.Data.Entity.DbFunctions.DiffDays(objHotelBooking.BookingForm,objHotelBooking.BookingTo).Value
                                    }).ToList();
                                 return PartialView("_BookingHistoryPartial",listOfBookingViewModels);
        }
    }
}