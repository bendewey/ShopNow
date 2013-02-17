using System.Runtime.Serialization;
using ShopNow.Data;

namespace ShopNow.DataModel
{
    [DataContract]
    public class Product : IIdentifiable
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string CategoryImage { get; set; }

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
        public double Rating { get; set; }
    }
}