using System;
using System.Runtime.Serialization;

namespace ShopNow.DataModel
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public long CartId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}