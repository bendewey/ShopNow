using System;
using System.Linq;

namespace ShopNow.DataModel.Repository
{
    public class PushNotificationDealsSubscribersRepository : RepositoryBase
    {
        public async void UpdateOrInsertDeviceSubscription(string hardwareId, string channelUri, DateTimeOffset expirationTime)
        {
            var table = MobileService.GetTable<PushNotificationDealsSubscriber>();
            var results = await table.Where(p => p.HardwareId == hardwareId)
                .Take(1)
                .ToListAsync();

            var recordForDevice = results.FirstOrDefault();
            if (recordForDevice == null)
            {
                await table.InsertAsync(new PushNotificationDealsSubscriber()
                                 {
                                     HardwareId = hardwareId,
                                     Uri = channelUri,
                                     ExpirationTime = expirationTime
                                 });
            }
            else if (recordForDevice.Uri != channelUri || recordForDevice.ExpirationTime != expirationTime)
            {
                recordForDevice.Uri = channelUri;
                recordForDevice.ExpirationTime = expirationTime;

                await table.UpdateAsync(recordForDevice);
            }
        }
    }
}