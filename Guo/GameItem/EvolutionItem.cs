using Pokaiju.Barattini;


namespace Pokaiju.Guo.GameItem;

public class EvolutionItem : AbstractGameItem
{
    public EvolutionItem(string nameItem, string description) : base(nameItem, description, GameItemTypes.EvolutionTool)
    {
    }

    public override bool Use(IMonster? m)
    {
        if (m.GetSpecies().GetEvolutionType() == EvolutionType.Item && m.CanEvolveByItem(this))
        {
            m.Evolve();
            return true;
        }

        return false;
    }
}