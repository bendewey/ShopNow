using System;
using System.Linq;
using System.Threading.Tasks;
using ShopNow.DataModel;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

namespace ShopNow.Services
{
    public class SecondaryTileCreator
    {
        public async Task<bool> CreateSecondaryTile(Category category, Rect rect)
        {
            var firstProduct = category.Products.First();
            var tile = new SecondaryTile(category.TileId, "ShopNow", 
                category.Name, "category=" + category.Name, 
                TileOptions.ShowNameOnLogo, new Uri(firstProduct.ThumbnailImage));

            tile.ForegroundText = ForegroundText.Dark;

            return await tile.RequestCreateForSelectionAsync(rect, Placement.Above);
        }

        public async Task<bool> RemoveSecondaryTile(Category category, Rect rect)
        {
            SecondaryTile secondaryTile = new SecondaryTile(category.TileId);
            return await secondaryTile.RequestDeleteForSelectionAsync(rect, Placement.Above);
        }
    }
}
