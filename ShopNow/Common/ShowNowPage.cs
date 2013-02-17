using ShopNow.DataModel;

namespace ShopNow.Common
{
    public class ShowNowPage : LayoutAwarePage
    {
        public Catalog Catalog
        {
            get { return SuspensionManager.SessionState[SuspensionManagerConstants.Catalog] as Catalog; }
        }

        public Cart Cart
        {
            get { return SuspensionManager.SessionState[SuspensionManagerConstants.Cart] as Cart; }
            set { SuspensionManager.SessionState[SuspensionManagerConstants.Cart] = value; }
        }
    }
}