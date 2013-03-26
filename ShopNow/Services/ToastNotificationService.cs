using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsExtensions.ToastContent;
using ShopNow.Events;
using Windows.UI.Notifications;

namespace ShopNow.Services
{
    public class ToastNotificationService
    {
        private readonly IEventAggregator _eventAggregator;
        // store these delegates as fields so they don't get garbage collected
        private Action<OrderPlacedEvent> _orderPlaced;

        public ToastNotificationService()
            : this(ServiceLocator.Get<IEventAggregator>())
        {
        }

        public ToastNotificationService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            _orderPlaced = OrderPlaced;
            _eventAggregator.Subscribe(_orderPlaced);
        }

        public void OrderPlaced(OrderPlacedEvent args)
        {
            var toast = ToastContentFactory.CreateToastText01();
            toast.TextBodyWrap.Text = "Thank you for placing your order";
            toast.Launch = "cartId=" + args.Cart.Id;

            var notification = toast.CreateNotification();

            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }
    }
}
