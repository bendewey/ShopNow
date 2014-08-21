using System.Runtime.Serialization;
using ShopNow.Data;

namespace ShopNow.DataModel
{
    public class Product : IIdentifiable
    {
        public long Id { get; set; }

        public string Category { get; set; }
        public string CategoryImage { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
    }
}