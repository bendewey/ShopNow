using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.MobileServices;
using ShopNow.Data;

namespace ShopNow.DataModel
{
    [DataContract]
    public class Cart : IIdentifiable
    {
        public Cart()
        {
            Items = new List<CartItem>();
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTimeOffset OrderDate { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string HardwareId { get; set; }

        [DataMember]
        [DataMemberJsonConverter(ConverterType = typeof(CartItemArrayConverter))]
        public List<CartItem> Items { get; set; }

        public double Subtotal
        {
            get { return Items.Sum(i => i.Price * i.Quantity); }
        }

        public double Shipping
        {
            get { return 10; }
        }

        public double Tax
        {
            get { return Subtotal * 0.05; }
        }

        public double Total
        {
            get { return Subtotal + Shipping + Tax; }
        }

        public int ItemCount
        {
            get { return Items.Count; }
        }

        public void AddProduct(Product product, int quantity)
        {
            var cartItem = new CartItem()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Image,
                    ThumbnailImage = product.ThumbnailImage,
                    Price = product.Price,
                    Quantity = quantity
                };

            Items.Add(cartItem);
        }
    }
}