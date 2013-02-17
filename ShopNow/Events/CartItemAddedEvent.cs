using ShopNow.DataModel;

namespace ShopNow.Events
{
    public class CartItemAddedEvent : IEvent
    {
        public Cart Cart { get; set; }

        public CartItemAddedEvent(Cart cart)
        {
            Cart = cart;
        }
    }
}
