﻿using HotelManegment.Models;
using HotelManegment.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace HotelManegment.Controllers
{
    public class RoomController : Controller
    {
        private HotelDbEntities1 objHotelDbEntities;

        public RoomController()
        {
            objHotelDbEntities=new HotelDbEntities1();
        }
        public ActionResult Index()
        {


            RoomViewModel objRoomViewModel = new RoomViewModel();
            objRoomViewModel.ListOfBookingStatus=(from obj in objHotelDbEntities.BookingStatus
                                                  select new SelectListItem()
                                                  {
                                                      Text=obj.BookingStatus,
                                                      Value=obj.BookingStatusId.ToString()
                                                  }).ToList();

            objRoomViewModel.ListOfRoomType= (from obj in objHotelDbEntities.RoomTypes
                                                    select new SelectListItem()
                                                    {
                                                        Text = obj.RoomTypeName,
                                                        Value = obj.RoomTypeId.ToString()
                                                    }).ToList();

            return View(objRoomViewModel);
        }
        [HttpPost]
        public ActionResult Index(RoomViewModel objRoomViewModel)
        {
            string message = String.Empty;
            string ImageUniqueName=String.Empty;
            string ActualImageName = String.Empty;
            if (objRoomViewModel.RoomId==0)
            {
                ImageUniqueName = Guid.NewGuid().ToString();
                ActualImageName = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
                objRoomViewModel.Image.SaveAs(filename: Server.MapPath("~/RoomImages/" + ActualImageName));
                Room objRoom = new Room()
                {
                    RoomNumber = objRoomViewModel.RoomNumber,
                    RoomDescription = objRoomViewModel.RoomDescription,
                    RoomPrice = objRoomViewModel.RoomPrice,
                    BookingStatusId = objRoomViewModel.BookingStatusId,
                    IsActive = true,
                    RoomImage = ActualImageName,
                    RoomCapacity = objRoomViewModel.RoomCapacity,
                    RoomTypeId = objRoomViewModel.RoomTypeId,
                };
                objHotelDbEntities.Rooms.Add(objRoom);
                message = "Added";
            }
            else
            {
                 Room objRoom = objHotelDbEntities.Rooms.Single(model => model.RoomId == objRoomViewModel.RoomId);
                if(objRoomViewModel.Image!=null)
                {
                    ImageUniqueName = Guid.NewGuid().ToString();
                    ActualImageName = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
                    objRoomViewModel.Image.SaveAs(filename: Server.MapPath("~/RoomImages/" + ActualImageName));
                    objRoom.RoomImage = ActualImageName;
                }
                  objRoom.RoomNumber = objRoomViewModel.RoomNumber;
                objRoom. RoomDescription = objRoomViewModel.RoomDescription;
                objRoom. RoomPrice = objRoomViewModel.RoomPrice;
                objRoom. BookingStatusId = objRoomViewModel.BookingStatusId;
                objRoom. IsActive = true;
                //RoomImage = ActualImageName,
                objRoom. RoomCapacity = objRoomViewModel.RoomCapacity;
                objRoom. RoomTypeId = objRoomViewModel.RoomTypeId;
                message = "Updated";
            }

          
            objHotelDbEntities.SaveChanges();
            return Json(data: new {message="Room Successfully."+ message, success=true},JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAllRooms()
        {
            IEnumerable<RoomDetailsViewModel> listOfRoomDetailsViewModel =
                (from objRoom in objHotelDbEntities.Rooms
                 join objBooking in objHotelDbEntities.BookingStatus on objRoom.BookingStatusId equals objBooking.BookingStatusId
                 join objRoomType in objHotelDbEntities.RoomTypes on objRoom.RoomTypeId equals objRoomType.RoomTypeId
                 where objRoom.IsActive== true
                 select new RoomDetailsViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     RoomDescription = objRoom.RoomDescription,
                     RoomCapacity = objRoom.RoomCapacity,
                     RoomPrice = objRoom.RoomPrice,
                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     RoomId = objRoom.RoomId
                 }).ToList();
            return PartialView("_RoomDetailsPartial", listOfRoomDetailsViewModel);
        }
        [HttpGet]
        public JsonResult EditRoomDetails(int roomId)
        {
            var result = objHotelDbEntities.Rooms.Single(model => model.RoomId == roomId);
            return Json(result,JsonRequestBehavior.AllowGet);
           
        }
        [HttpGet]
        public JsonResult DeleteRoomDetails(int roomId)
        {
            Room objRoom = objHotelDbEntities.Rooms.Single(model => model.RoomId == roomId);
            objRoom.IsActive = false;
            objHotelDbEntities.SaveChanges();
            return Json(data: new {message="Record Successfully Deleted.",success=true},JsonRequestBehavior.AllowGet);

        }
    }
}