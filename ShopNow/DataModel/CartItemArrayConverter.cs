using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Data.Json;

namespace ShopNow.DataModel
{
    public class CartItemArrayConverter : IDataMemberJsonConverter
    {
        public object ConvertFromJson(IJsonValue value)
        {
            // unused
            return null;
        }

        public IJsonValue ConvertToJson(object instance)
        {
            var cartItems = ((IEnumerable<CartItem>)instance).ToArray();
            var result = new JsonArray();
            foreach (var cartItem in cartItems)
            {
                result.Add(MobileServiceTableSerializer.Serialize(cartItem));
            }

            return result;
        }
    }
}