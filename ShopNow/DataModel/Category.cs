using System.Collections.Generic;

namespace ShopNow.DataModel
{
    public class Category
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public List<Product> Products { get; set; }

        public string TileId
        {
            get { return Name.GetHashCode().ToString(); }
        }
    }
}