using System.Linq;
using System.Threading.Tasks;

namespace ShopNow.DataModel.Repository
{
    public class CatalogRepository : RepositoryBase
    {
        public async Task<Catalog> GetCatalog()
        {
            var products = await MobileService.GetTable<Product>().ReadAsync();
            return new Catalog(products.ToList());
        }

        public async void UpdateDescriptions()
        {
            var lipsum =
                @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec feugiat congue urna ut pharetra. Fusce ligula lorem, consequat quis imperdiet nec, interdum non leo. Donec porttitor varius tincidunt. Nam varius fermentum mollis. Praesent est quam, bibendum a porttitor quis, accumsan bibendum quam. Curabitur eget lorem vel purus varius egestas. Praesent in scelerisque ante. Vestibulum sed velit tellus. Vivamus tristique ante et eros porta rutrum ac at augue. Fusce mauris orci, placerat eget porta fermentum, cursus ut lacus. Ut euismod sollicitudin ante, id laoreet sem porttitor imperdiet. Morbi vel leo nisi, at facilisis lorem. Ut sagittis tincidunt eros, ac convallis lacus laoreet in. Nam ac sollicitudin lacus. Duis vitae purus id diam facilisis congue. 

Aenean urna risus, consequat quis egestas vitae, consequat et est. Nulla vulputate viverra lectus et mollis. Aliquam ut mauris libero. Aliquam quis hendrerit justo. Etiam id lorem diam, vel adipiscing est. Sed ultricies libero vel tortor molestie semper. Sed viverra blandit augue, ut ultricies leo convallis sed. Maecenas accumsan felis vel lorem blandit sit amet rhoncus odio luctus. Maecenas id velit quam, quis pretium justo. Sed consequat ipsum non nunc facilisis scelerisque. Nam porta, risus dapibus scelerisque pretium, elit elit facilisis nulla, id suscipit lacus lorem a orci. Nullam vel suscipit sapien. In tincidunt elit eget dui luctus nec egestas turpis fermentum. Nunc lobortis pretium ligula, eu adipiscing quam suscipit et. 

Maecenas nec mauris sit amet orci imperdiet vehicula. Sed elementum laoreet enim, condimentum hendrerit risus iaculis ac. Phasellus non sem quis elit venenatis pretium at ut sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus nibh velit, tincidunt at aliquam nec, tincidunt a leo. Duis fringilla convallis cursus. Ut tempus consectetur hendrerit. Nam volutpat elit eleifend dui ultrices a rhoncus felis tincidunt.";

            var products = await MobileService.GetTable<Product>().ReadAsync();
            foreach (var p in products)
            {
                p.Description = lipsum;
                await MobileService.GetTable<Product>().UpdateAsync(p);
            }
        }
    }
}
