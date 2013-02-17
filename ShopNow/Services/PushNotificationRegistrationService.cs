using System;
using ShopNow.Common;
using ShopNow.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using ShopNow.DataModel.Repository;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;

namespace ShopNow.Services
{
    public class PushNotificationRegistrationService
    {
        public PushNotificationChannel CurrentChannel { get; private set; }

        public async void AcquirePushChannel()
        {
            var retryCount = 0;
            while (CurrentChannel == null && retryCount < 3)
            {
                try
                {
                    CurrentChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                }
                catch
                {
                }

                retryCount++;
            }

            if (CurrentChannel != null)
            {
                RequestPermissionToPushDeals(CurrentChannel);
            }
        }

        private async void RequestPermissionToPushDeals(PushNotificationChannel channel)
        {
            if (!SuspensionManager.SessionState.ContainsKey(SuspensionManagerConstants.AcceptPushDeals))
            {
                var dialog = new MessageDialog("Would you like to enable push notification for deals and specials?");
                dialog.Commands.Add(new UICommand("Yes"));
                dialog.Commands.Add(new UICommand("No"));
                var command = await dialog.ShowAsync();
                SuspensionManager.SessionState.Add(SuspensionManagerConstants.AcceptPushDeals, command.Label == "Yes");
            }

            if ((bool)SuspensionManager.SessionState[SuspensionManagerConstants.AcceptPushDeals] == false)
            {
                return;
            }

            var repository = ServiceLocator.Get<PushNotificationDealsSubscribersRepository>();
            repository.UpdateOrInsertDeviceSubscription(HardwareIdentificationHelpers.GetHardwareId(), channel.Uri, channel.ExpirationTime);
        }
    }
}
