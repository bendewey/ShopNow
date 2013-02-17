using System.Linq;
using System.Threading.Tasks;

namespace ShopNow.DataModel.Repository
{
    public class CartRepository : RepositoryBase
    {
        public async Task SubmitCart(Cart cart)
        {
            await MobileService.GetTable<Cart>().InsertAsync(cart);
        }

        public async Task<Cart> Get(long cartId)
        {
            var carts = await MobileService.GetTable<Cart>().Where(c => c.Id == cartId).ToListAsync();
            var cart = carts.FirstOrDefault();
            if (cart == null)
            {
                return null;
            }
                
            cart.Items = await MobileService.GetTable<CartItem>().Where(c => c.CartId == cartId).ToListAsync();
            return cart;
        }
    }
}
