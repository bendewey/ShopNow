using ShopNow.DataModel;

namespace ShopNow.Events
{
    public class OrderPlacedEvent : IEvent
    {
        public Cart Cart { get; set; }

        public OrderPlacedEvent(Cart cart)
        {
            Cart = cart;
        }
    }
}