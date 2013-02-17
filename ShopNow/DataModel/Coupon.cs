using System;

namespace ShopNow.DataModel
{
    public class Coupon
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Submitted { get; set; }
        public bool Sent { get; set; }
    }
}