using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Npc;

public class NpcMerchant : NpcSimple, INpcMerchant
{
    private readonly Dictionary<IGameItem, int> _inventory;
    
    /// <summary>
    /// Constructor of NpcMerchant 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sentences"></param>
    /// <param name="position"></param>
    /// <param name="isVisible"></param>
    /// <param name="isEnabled"></param>
    /// <param name="inventory"></param>
    public NpcMerchant(string name, IList<string> sentences, Tuple<int, int> position,
        bool isVisible, bool isEnabled, Dictionary<IGameItem, int> inventory) : base(name, TypeOfNpc.Merchant, sentences, position, isVisible, isEnabled)
    {
        _inventory = inventory;
    }
    
    /// <inheritdoc cref="INpcMerchant.GetInventory"/>
    public Dictionary<IGameItem, int> GetInventory()
    {
        return _inventory;
    }
    /// <inheritdoc cref="INpcMerchant.GetPrice"/>
    public int GetPrice(IGameItem item)
    {
        if (!_inventory.ContainsKey(item)) return 0;
        var hasValue = _inventory.TryGetValue(item, out var value);
        return hasValue ? value : 0;
    }

    /// <inheritdoc cref="INpcMerchant.GetTotalPrice"/>
    public int GetTotalPrice(IList<Tuple<IGameItem, int>> itemList)
    {
        return itemList.Sum(item => GetPrice(item.Item1) * item.Item2);
    }
    
    
    private void AddItems( IList<Tuple<IGameItem, int>> list, IPlayer player) 
    {
        foreach ( var item in list) 
        {
            for (var i = 0; i < item.Item2; i++)
            {
                player.AddItem(item.Item1);
            }

        }
    }
    
    /// <inheritdoc cref="INpcMerchant.BuyItem"/>
    public bool BuyItem(IList<Tuple<IGameItem, int>> itemList, IPlayer player)
    {
        if (player.GetMoney() - GetTotalPrice(itemList) < 0) return false;
        player.SetMoney(player.GetMoney() - GetTotalPrice(itemList));
        AddItems(itemList, player);
        return true;
    }
}