using Pokaiju.Barattini;

namespace Pokaiju.Guo.GameItem;

public class HealingItem : AbstractGameItem
{
    private static readonly int DEFAULT_HEALING_POINTS = 50;
    private readonly int _healedHp;

    public HealingItem(string nameItem, string description) : this(nameItem, description, DEFAULT_HEALING_POINTS)
    {
    }

    public HealingItem(string nameItem, string description, int healedHp) : base(nameItem, description,
        GameItemTypes.Heal)
    {
        if (healedHp < 0) throw new ArgumentException("Healed Hp must be greater than zero");
        _healedHp = healedHp;
    }

    public override bool Use(Monster? m)
    {
        if (m.GetStats().Health== m.GetMaxHealth()) return false;
        m.SetHealth(_healedHp + m.GetStats().Health);
        return true;
    }

}