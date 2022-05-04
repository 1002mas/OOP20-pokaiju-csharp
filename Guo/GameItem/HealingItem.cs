using Pokaiju.Barattini;

namespace Pokaiju.Guo.GameItem;

public class HealingItem : AbstractGameItem
{
    private const int DefaultHealingPoints = 50;
    private readonly int _healedHp;

    public HealingItem(string nameItem, string description) : this(nameItem, description, DefaultHealingPoints)
    {
    }

    private HealingItem(string nameItem, string description, int healedHp) : base(nameItem, description,
        GameItemTypes.Heal)
    {
        if (healedHp < 0) throw new ArgumentException("Healed Hp must be greater than zero");
        _healedHp = healedHp;
    }

    public override bool Use(IMonster? m)
    {
        if (m?.GetStats().Health== m?.GetMaxHealth()) return false;
        m?.SetHealth(_healedHp + m.GetStats().Health);
        return true;
    }

}