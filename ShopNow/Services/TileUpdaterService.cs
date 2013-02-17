using System;
using System.Linq;
using ShopNow.Events;
using Windows.UI.Notifications;

namespace ShopNow.Services
{
    public class TileUpdaterService
    {
        private readonly IEventAggregator _eventAggregator;
        // store these delegates as fields so they don't get garbage collected
        private Action<CartItemAddedEvent> _itemAddedToCart;

        public TileUpdaterService() : this(ServiceLocator.Get<IEventAggregator>())
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
        }

        public void ItemAddedToCart(CartItemAddedEvent args)
        {
            //var xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText01);
            //var x = xml.GetXml();

            //var text = xml.GetElementsByTagName("text").First();
            //text.InnerText = string.Format("{0} Item(s)", args.Cart.ItemCount);

            //x = xml.GetXml();

            //var notification = new TileNotification(xml);

            //var tile = NotificationsExtensions.TileContent.TileContentFactory.CreateTileSquareText01();
            //tile.TextHeading.Text = string.Format("{0} Item(s)", args.Cart.ItemCount);
            //var notification = tile.CreateNotification();

            //var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            //updater.Update(notification);

            var badge = new NotificationsExtensions.BadgeContent.BadgeNumericNotificationContent();
            badge.Number = (uint)args.Cart.ItemCount;
            var notification = badge.CreateNotification();

            var updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            updater.Update(notification);

        }
    }
}
