namespace Pokaiju.Guo.GameItem;
using System.Reflection;

 class ItemAttr : Attribute
{
    private string Name { get; }
    public bool UseBattle { get; }
    public bool UseBag { get; }
    internal ItemAttr(string name, bool useBattle, bool useBag)
    {
        Name = name;
        UseBattle = useBattle;
        UseBag = useBag;
    }
}

public static class GameItemType
{
    private static ItemAttr GetAttr(GameItemTypes p)
    {
        var itemAtt = (ItemAttr?) Attribute.GetCustomAttribute(ForValue(p), typeof(ItemAttr));
        if (itemAtt is null)
        {
            throw new ArgumentNullException();
        }

        return itemAtt;
    }
    private static MemberInfo ForValue(GameItemTypes p)
    {
        var name = Enum.GetName(typeof(GameItemTypes), p);
        if (name is null)
        {
            throw new ArgumentException();
        }
        MemberInfo? memberInfo = typeof(GameItemTypes).GetField(name);
        if (memberInfo is null)
        {
            throw new ArgumentException();
        }
        return memberInfo;
    }

    public static bool IsConsumableInBag(GameItemTypes p)
    {
        var att = GetAttr(p);
        return att.UseBag;
    }

    public static bool IsConsumableInBattle(GameItemTypes p) {
        var att = GetAttr(p);
        return att.UseBattle;
    }
}

public enum GameItemTypes
{
    [ItemAttr("Heal",true, true)] Heal,
    [ItemAttr("EvolutionTool",false, true)] EvolutionTool,
    [ItemAttr("MonsterBall",true, false)] MonsterBall,
}