using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Npc;

public class NpcTrainer : NpcSimple, INpcTrainer
{
    private const int DefeatedTextId = 1;
    private readonly  IList<IMonster> _monstersOwned;
    private bool _isDefeated;
    
    public NpcTrainer(string name, IList<string> sentences, Tuple<int, int> position, bool isVisible, 
        bool isEnabled, IList<IMonster> monsterOwned, bool isDefeated) : base(name, sentences, position, isVisible, isEnabled)
    {
        this._monstersOwned = monsterOwned;
        this._isDefeated = isDefeated;
    }

    public IList<IMonster> GetMonstersOwned()
    {
        return this._monstersOwned;
    }

    public bool IsDefeated()
    {
        return this._isDefeated;
    }

    public void SetDefeated(bool isDefeated)
    {
        this._isDefeated = isDefeated;
        if (IsDefeated()) 
        {
            SetDialogueText(DefeatedTextId);
        } else 
        {
            SetDialogueText(0);
        }

    }
}