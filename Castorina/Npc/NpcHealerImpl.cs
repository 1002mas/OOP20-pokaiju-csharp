using Optional;
using Pokaiju.Barattini;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Npc;

public class NpcHealerImpl : NpcSimpleImpl
{
    private readonly IPlayer _player;
    
    public NpcHealerImpl(string name, IList<string> sentences, Tuple<int, int> position, bool isVisible, bool isEnabled, IPlayer player) : base(name, sentences, position, isVisible, isEnabled)
    {
        this._player = player;
    }
    
    private void Heal() 
    {
        foreach ( IMonster monster in this._player.GetAllMonsters()) 
        {
            monster.SetHealth(monster.GetMaxHealth());
            monster.RestoreAllMovesPp();
        }

    }
    
    public override Option<string> InteractWith() 
    {
        Option<string> result = base.InteractWith();
        if (IsEnabled()) 
        {
            Heal();
        }
        return result;
    }

}