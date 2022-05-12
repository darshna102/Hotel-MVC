using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace HotelManegment.ViewModel
{
    public class BookingViewModel
    {
        [Display(Name ="Customer Name")]
        [Required(ErrorMessage ="Customer is required.")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Address")]
        [Required(ErrorMessage = "Customer Address is required.")]
        public string CustomerAddress { get;set;}

        [Required(ErrorMessage ="Customer Phone is required")]
        [Display(Name = "Customer Phone")]
        public string CustomerPhone{get; set;}
        [Display(Name = "BookingForm")]
        [Required(ErrorMessage = "BookingForm is required")]
        [DisplayFormat(DataFormatString="{0:dd-MMM-yyyy}",ApplyFormatInEditMode =true)]

        public DateTime BookingForm { get; set; }
        [Display(Name = "BookingTo")]

        [Required(ErrorMessage = "BookingTo is required")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingTo { get; set; }
        [Display(Name = "Assign Room")]
        [Required(ErrorMessage = "Room is required")]
        public int AssignRoomId { get; set; }
       
        [Display(Name = "No Of Members")]
        [Required(ErrorMessage = "Number of member  is required")]
        public int NoOfMembers { get; set; }
        public IEnumerable<SelectListItem> ListOfRooms { get; set; }

    }
}