using ShopNow.Common;
using ShopNow.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using ShopNow.Events;
using ShopNow.Services;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace ShopNow
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage : ShowNowPage
    {
        public ItemDetailPage()
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
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            var product = Catalog.GetProduct((long) navigationParameter);
            var category = Catalog.GetProductsForCategory(product.Category).First();
            this.DefaultViewModel["Group"] = category;
            this.DefaultViewModel["Products"] = category.Products;
            this.flipView.SelectedItem = product;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (Product)this.flipView.SelectedItem;
            if (selectedItem != null)
            {
                pageState["SelectedItem"] = selectedItem.Id;    
            }
        }

        private void AddToCart_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Cart.AddProduct((Product) this.flipView.SelectedItem, 1);

            ServiceLocator.Get<IEventAggregator>().Publish(new CartItemAddedEvent(Cart));

            this.Frame.Navigate(typeof(CartPage));
        }
    }
}
