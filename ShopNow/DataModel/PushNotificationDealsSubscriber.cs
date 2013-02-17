using System;
using System.Runtime.Serialization;
using ShopNow.Common;
using ShopNow.Data;

namespace ShopNow.DataModel
{
    [DataContract]
    public class PushNotificationDealsSubscriber : BindableBase, IIdentifiable
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string HardwareId { get; set; }
        [DataMember]
        public string Uri { get; set; }
        [DataMember]
        public DateTimeOffset ExpirationTime { get; set; }
    }
}