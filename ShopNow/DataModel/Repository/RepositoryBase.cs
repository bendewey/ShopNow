using Microsoft.WindowsAzure.MobileServices;

namespace ShopNow.DataModel.Repository
{
    public class RepositoryBase
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
                "https://shopnowdemo.azure-mobile.net/",
                "JeidcEqLMdRSFpjZRpQjdCuDAjpnsK89"
                );
    }
}
