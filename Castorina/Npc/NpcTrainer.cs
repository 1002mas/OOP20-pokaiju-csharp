using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Npc;

public class NpcTrainer : NpcSimple, INpcTrainer
{
    private const int DefeatedTextId = 1;
    private readonly  IList<IMonster> _monstersOwned;
    private bool _isDefeated;
    
    /// <summary>
    /// Constructor for NpcTrainer.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sentences"></param>
    /// <param name="position"></param>
    /// <param name="isVisible"></param>
    /// <param name="isEnabled"></param>
    /// <param name="monsterOwned"></param>
    /// <param name="isDefeated"></param>
    
    public NpcTrainer(string name, IList<string> sentences, Tuple<int, int> position, bool isVisible, 
        bool isEnabled, IList<IMonster> monsterOwned, bool isDefeated) : base(name, TypeOfNpc.Trainer, sentences, position, isVisible, isEnabled)
    {
        this._monstersOwned = monsterOwned;
        this._isDefeated = isDefeated;
    }
    
    /// <inheritdoc cref="INpcTrainer.GetMonstersOwned"/>
    public IList<IMonster> GetMonstersOwned()
    {
        return this._monstersOwned;
    }
    
    /// <inheritdoc cref="INpcTrainer.IsDefeated"/>
    public bool IsDefeated()
    {
        return this._isDefeated;
    }
    
    /// <inheritdoc cref="INpcTrainer.SetDefeated"/>
    public void SetDefeated(bool isDefeated)
    {
        this._isDefeated = isDefeated;
        SetDialogueText(IsDefeated() ? DefeatedTextId : 0);
    }
}