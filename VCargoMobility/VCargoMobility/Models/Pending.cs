using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace VCargoMobility.Models
{
  public  class Pending
    {
        public string Id { get; set; }

        public string  BookingId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderRefNo { get; set; }
        public string OrderDate { get; set; }
        public string OrdeConsignee { get; set; }
        public string OrderDestination { get; set; }
        public Color ButtonColor { get; set; }
    }
}
