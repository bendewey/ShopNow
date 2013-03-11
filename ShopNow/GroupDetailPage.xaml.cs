using System.Linq;
using ShopNow.Common;
using ShopNow.Data;

using System;
using System.Collections.Generic;
using ShopNow.DataModel;
using ShopNow.Services;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace ShopNow
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : ShowNowPage
    {
        public GroupDetailPage()
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
            var category = Catalog.GetProductsForCategory((String)navigationParameter).FirstOrDefault();
            if (category == null) return;

            this.DefaultViewModel["Category"] = category;
            this.DefaultViewModel["Products"] = category.Products;

            ToggleAppBarButtonStyle(!SecondaryTile.Exists(category.TileId));

        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((IIdentifiable)e.ClickedItem).Id;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void ToggleAppBarButtonStyle(bool showPinButton)
        {
            if (pinToAppBar != null)
            {
                pinToAppBar.Style = (showPinButton) ? (Application.Current.Resources["PinAppBarButtonStyle"] as Style) : (Application.Current.Resources["UnPinAppBarButtonStyle"] as Style);
            }
        }

        private async void PinCategory_OnClick(object sender, RoutedEventArgs e)
        {
            var rect = GetPositionOfElement(sender as FrameworkElement);
            var category = (Category) DefaultViewModel["Category"];

            var creator = ServiceLocator.Get<SecondaryTileCreator>();
            if (SecondaryTile.Exists(category.TileId))
            {
                var wasRemoved = await creator.RemoveSecondaryTile(category, rect);

                ToggleAppBarButtonStyle(wasRemoved);
            }
            else
            {
                var wasAdded = await creator.CreateSecondaryTile(category, rect);

                ToggleAppBarButtonStyle(!wasAdded);
            }
        }

        private static Rect GetPositionOfElement(FrameworkElement element)
        {
            if (element == null)
                return Rect.Empty;

            Windows.UI.Xaml.Media.GeneralTransform buttonTransform = element.TransformToVisual(null);
            var point = buttonTransform.TransformPoint(new Point());
            var rect = new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
            return rect;
        }
    }
}
