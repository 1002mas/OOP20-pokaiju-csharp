using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Npc;

public interface INpcMerchant
{

    /// <summary>
    /// This function returns a dictionary of items and relatives prices.
    /// </summary>
    /// <returns>dictionary of game items and price</returns>
    Dictionary<IGameItem, int> GetInventory();

    /// <summary>
    /// This function returns the price of an item.
    /// </summary>
    /// <param name="item"></param>
    int GetPrice(IGameItem item);

    /// <summary>
    /// This function returns the total price amount of items to buy.
    /// </summary>
    /// <param name="itemList"></param>
    /// <returns>sum of item price</returns>
    int GetTotalPrice(IList<Tuple<IGameItem, int>> itemList);

    /// <summary>
    /// This function tries to make player buy a list of items.
    /// </summary>
    /// <param name="itemList"></param>
    /// <param name="player"></param>
    /// <returns>true if item is bought, false otherwise</returns>
    bool BuyItem(IList<Tuple<IGameItem, int>> itemList, IPlayer player);

}


