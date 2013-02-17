using System;
using System.Threading.Tasks;

namespace ShopNow.DataModel.Repository
{
    public class CouponRepository : RepositoryBase
    {
        public async Task SendCoupon(string text)
        {
            await MobileService.GetTable<Coupon>().InsertAsync(new Coupon
                {
                    Text = text,
                    Submitted = DateTime.Now
                });
        }
    }
}