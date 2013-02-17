using System.Runtime.Serialization;

namespace ShopNow.DataModel
{
    [DataContract]
    public class CartItem
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long CartId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string ThumbnailImage { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Quantity { get; set; }
    }
}