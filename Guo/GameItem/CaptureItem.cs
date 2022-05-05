namespace Pokaiju.Guo.GameItem;
using Barattini;

public class CaptureItem : AbstractGameItem
{
    public CaptureItem(string nameItem, string description) : base(nameItem, description, GameItemTypes.MonsterBall)
    {
    }

    public override bool Use(IMonster? m)
    {
        return true;
    }
}