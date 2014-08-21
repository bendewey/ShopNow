using System;
using System.Runtime.Serialization;
using ShopNow.Common;
using ShopNow.Data;

namespace ShopNow.DataModel
{
    public class PushNotificationDealsSubscriber : BindableBase, IIdentifiable
    {
        public long Id { get; set; }
        public string HardwareId { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset ExpirationTime { get; set; }
    }
}