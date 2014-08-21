using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ShopNow.DataModel
{
    public class Catalog
    {
        public Catalog(List<Product> products)
        {
            Products = products;
        }

        public List<Product> Products { get; set; }

        public List<Category> GetProductsForCategory(string categoryName)
        {
            var categories = from p in Products
                             group p by p.Category into c
                             let firstProductInCategory = c.FirstOrDefault()
                             where firstProductInCategory != null
                             select new Category
                                 {
                                     Name = c.Key,
                                     Image = firstProductInCategory.CategoryImage,
                                     Products = c.ToList()
                                 };

            if (categoryName == "AllCategories")
            {
                return categories.ToList();
            }
            else
            {
                return categories.Where(c => c.Name == categoryName).ToList();
            }
        }

        public Product GetProduct(long productId)
        {
            return Products.FirstOrDefault(p => p.Id == productId);
        }
    }
}