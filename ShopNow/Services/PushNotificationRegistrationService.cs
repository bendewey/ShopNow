using System;
using ShopNow.Common;
using ShopNow.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using ShopNow.DataModel.Repository;
using Windows.Networking.PushNotifications;
using Windows.Storage;
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
            if (!HasSetting(SuspensionManagerConstants.AcceptPushDeals))
            {
                var dialog = new MessageDialog("Would you like to enable push notification for deals and specials?   You can change this via settings later.");
                dialog.Commands.Add(new UICommand("Yes"));
                dialog.Commands.Add(new UICommand("No"));
                var command = await dialog.ShowAsync();
                SetSetting(SuspensionManagerConstants.AcceptPushDeals, command.Label == "Yes");
            }

            if (GetSetting(SuspensionManagerConstants.AcceptPushDeals) == false)
            {
                return;
            }

            var repository = ServiceLocator.Get<PushNotificationDealsSubscribersRepository>();
            repository.UpdateOrInsertDeviceSubscription(HardwareIdentificationHelpers.GetHardwareId(), channel.Uri, channel.ExpirationTime);
        }

        public bool HasSetting(string key)
        {
            return ApplicationData.Current.RoamingSettings.Values.ContainsKey(key);
        }

        public void SetSetting(string key, bool value)
        {
            ApplicationData.Current.RoamingSettings.Values.Add(key, value);
        }

        public bool GetSetting(string key)
        {
            return (bool)ApplicationData.Current.RoamingSettings.Values[key];
        }
    }
}
