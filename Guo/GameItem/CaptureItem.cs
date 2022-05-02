using Pokaiju.Barattini;

namespace Pokaiju.Guo.GameItem;

public class CaptureItem : AbstractGameItem
{
    public CaptureItem(string nameItem, string description) : base(nameItem, description, GameItemTypes.Monsterball)
    {
    }

    public override bool Use(Monster m)
    {
        return true;
    }
}