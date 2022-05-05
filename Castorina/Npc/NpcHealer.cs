using Optional;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Npc;

public class NpcHealer : NpcSimple
{
    private readonly IPlayer _player;

    /// <summary>
    /// Constructor for NpcHealer 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sentences"></param>
    /// <param name="position"></param>
    /// <param name="isVisible"></param>
    /// <param name="isEnabled"></param>
    /// <param name="player"></param>
    public NpcHealer(string name, IList<string> sentences, Tuple<int, int> position, bool isVisible,
        bool isEnabled, IPlayer player) : base(name, TypeOfNpc.Healer, sentences, position, isVisible, isEnabled)
    {
        _player = player;
    }

    private void Heal()
    {
        foreach (var monster in _player.GetAllMonsters())
        {
            monster.SetHealth(monster.GetMaxHealth());
            monster.RestoreAllMovesPp();
        }

    }

    /// <summary>
    /// Override of InteractWith() in NpcSimple  
    /// </summary>
    /// <returns>Option of string </returns>
    public override Option<string> InteractWith()
    {
        var result = base.InteractWith();
        if (IsEnabled())
        {
            Heal();
        }

        return result;
    }
}
