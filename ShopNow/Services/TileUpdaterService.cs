using System;
using System.Linq;
using NotificationsExtensions.BadgeContent;
using NotificationsExtensions.TileContent;
using ShopNow.Events;
using Windows.UI.Notifications;

namespace ShopNow.Services
{
    public class TileUpdaterService
    {
        private readonly IEventAggregator _eventAggregator;
        // store these delegates as fields so they don't get garbage collected
        private Action<CartItemAddedEvent> _itemAddedToCart;

        public TileUpdaterService()
            : this(ServiceLocator.Get<IEventAggregator>())
        {
        }

        public TileUpdaterService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            _itemAddedToCart = ItemAddedToCart;
            _eventAggregator.Subscribe(_itemAddedToCart);

            SetupPeriodicTileForCoupon();
        }

        private void SetupPeriodicTileForCoupon()
        {
            const string xmlUrl = "http://shopnow-services.azurewebsites.net/Xml";
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(new Uri(xmlUrl), DateTimeOffset.Now.AddSeconds(30), PeriodicUpdateRecurrence.HalfHour);
        }

        public void ItemAddedToCart(CartItemAddedEvent args)
        {
            var xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText01);

            var x = xml.GetXml();

            var text = xml.GetElementsByTagName("text").First();
            text.InnerText = string.Format("{0} Item(s)", args.Cart.ItemCount);

            x = xml.GetXml();

            var notification = new TileNotification(xml);

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(notification);
        }
    }
}
