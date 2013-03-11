using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ShopNow.Common;
using ShopNow.DataModel;
using ShopNow.DataModel.Repository;
using ShopNow.Services;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace ShopNow
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class CartPage : ShowNowPage
    {
        public CartPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var cart = Cart;
            if (navigationParameter is long)
            {
                var cartId = (long) navigationParameter;
                var cartRepository = ServiceLocator.Get<CartRepository>();
                cart = await cartRepository.Get(cartId);

                SubmitOrder.IsEnabled = false;
            }

            this.DefaultViewModel["Cart"] = cart;
            this.DefaultViewModel["Items"] = cart.Items;
        }

        private async void SubmitOrder_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessingRing.IsActive = true;
            SubmitOrder.IsEnabled = false;

            Exception exception = null;
            bool submitted = false;
            try
            {
                var repository = ServiceLocator.Get<CartRepository>();

                var user = await Authenticate();
                if (user != null)
                {

                    Cart.UserId = user.UserId;
                    Cart.HardwareId = HardwareIdentificationHelpers.GetHardwareId();
                    await repository.SubmitCart(Cart);

                    Cart = new Cart();
                    submitted = true;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                var dialog = new MessageDialog(exception.Message);
                await dialog.ShowAsync();
            }
            else if (submitted)
            {
                Frame.Navigate(typeof(GroupedItemsPage), "AllCategories");
            }

            ProcessingRing.IsActive = false;
            SubmitOrder.IsEnabled = true;
        }

        private async Task<MobileServiceUser> Authenticate()
        {
            while (true)
            {
                string message;
                try
                {
                    if (RepositoryBase.MobileService.CurrentUser == null)
                    {
                        var user = await RepositoryBase.MobileService
                            .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
                        return user;    
                    }
                    return RepositoryBase.MobileService.CurrentUser;
                }
                catch (InvalidOperationException)
                {
                    message = "You must log in. Login Required";
                }

                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                dialog.Commands.Add(new UICommand("Cancel"));
                var result = await dialog.ShowAsync();
                if (result.Label == "Cancel")
                {
                    return null;
                }
            }
        }
    }
}
