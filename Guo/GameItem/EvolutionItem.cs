namespace Pokaiju.Guo.GameItem;
using Barattini;

public class EvolutionItem : AbstractGameItem
{
    public EvolutionItem(string nameItem, string description) : base(nameItem, description, GameItemTypes.EvolutionTool)
    {
    }

    public override bool Use(IMonster? m)
    {
        if (m?.GetSpecies().GetEvolutionType() != EvolutionType.Item || !m.CanEvolveByItem(this)) return false;
        m.Evolve();
        return true;

    }
}