using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Npc;

public class NpcMerchant : NpcSimple, INpcMerchant
{
    private readonly Dictionary<IGameItem, int> _inventory;
    public NpcMerchant(string name, IList<string> sentences, Tuple<int, int> position,
        bool isVisible, bool isEnabled, Dictionary<IGameItem, int> inventory) : base(name, sentences, position, isVisible, isEnabled)
    {
        this._inventory = inventory;
    }

    public Dictionary<IGameItem, int> GetInventory()
    {
        return this._inventory;
    }

    public int GetPrice(IGameItem item)
    {
        if (!this._inventory.ContainsKey(item)) return 0;
        var hasValue = _inventory.TryGetValue(item, out var value);
        return hasValue ? value : 0;
    }

    public int GetTotalPrice(IList<Tuple<IGameItem, int>> itemList)
    {
        int sum = 0;
        foreach ( var item in itemList)
        {
            sum = sum + (GetPrice(item.Item1) * item.Item2);
        }
        return sum;
    }
    
    private void AddItems( IList<Tuple<IGameItem, int>> list, IPlayer player) 
    {
        foreach ( var item in list) 
        {
            for (int i = 0; i < item.Item2; i++)
            {
                player.AddItem(item.Item1);
            }

        }
    }
    
    public bool BuyItem(IList<Tuple<IGameItem, int>> itemList, IPlayer player)
    {
        if ((player.GetMoney() - GetTotalPrice(itemList)) < 0) return false;
        player.SetMoney(player.GetMoney() - GetTotalPrice(itemList));
        AddItems(itemList, player);
        return true;
    }
}