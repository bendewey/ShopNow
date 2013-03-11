using System.Linq;
using ShopNow.Common;
using ShopNow.DataModel;
using ShopNow.DataModel.Repository;
using System;
using ShopNow.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace ShopNow
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            var tileUpdaterService = ServiceLocator.Get<TileUpdaterService>();
            tileUpdaterService.Initialize();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            SuspensionManager.KnownTypes.Add(typeof(Catalog));
            SuspensionManager.KnownTypes.Add(typeof(Cart));

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                if (!SuspensionManager.SessionState.ContainsKey(SuspensionManagerConstants.Catalog))
                {
                    Window.Current.Content = new ExtendedSplash();
                    Window.Current.Activate();
        
                    var repository = ServiceLocator.Get<CatalogRepository>();
                    var catalog = await repository.GetCatalog();
                    SuspensionManager.SessionState.Add(SuspensionManagerConstants.Catalog, catalog);
                }

                if (!SuspensionManager.SessionState.ContainsKey(SuspensionManagerConstants.Cart))
                {
                    SuspensionManager.SessionState.Add(SuspensionManagerConstants.Cart, new Cart());
                }

                var pushNotificationRegistration = ServiceLocator.Get<PushNotificationRegistrationService>();
                pushNotificationRegistration.AcquirePushChannel();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllCategories"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            if (!string.IsNullOrEmpty(args.Arguments))
            {
                var decoder = new WwwFormUrlDecoder(args.Arguments);

                var category = decoder.Where(v => v.Name == "category").Select(v => v.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(category))
                {
                    // if category exists then its launched from a secondary tile.
                    rootFrame.Navigate(typeof (GroupDetailPage), category);
                }

                var cartParam = decoder.Where(v => v.Name == "cartId").Select(v => v.Value).FirstOrDefault();
                long cartId;
                if (long.TryParse(cartParam, out cartId))
                {
                    // if cartId exists then its launched from a toast.
                    rootFrame.Navigate(typeof (CartPage), cartId);
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
