namespace Pokaiju.Carafassi.GameEvents;

using Barattini;
using System.Collections.Immutable;
using Guo.Player;

/// <summary>
///  This class allows to create an event gives one or more monster to the player.
/// </summary>
public class MonsterGift : AbstractGameEvent
{
    private readonly IList<IMonster> _monsters;
    private readonly IPlayer _player;


    /// <summary>
    /// It creates an event that allows to give the player some monsters.
    /// </summary>
    /// <param name="id">It is used to identify the event.</param>
    /// <param name="isActive">If it is active when the player interacts with a trigger,
    /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
    /// calling the function <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isToActiveImmediately">If the event has to be activated right after
    /// another event is triggered.</param>
    /// <param name="monsters">The list of monsters the player will receive.</param>
    /// <param name="player">The player receiving the monsters.</param>
    public MonsterGift(int id, bool isActive, bool isReactivable, bool isToActiveImmediately,
        IList<IMonster> monsters, IPlayer player) : base(id, isActive, isReactivable, isToActiveImmediately)
    {
        _monsters = monsters;
        _player = player;
    }

    /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
    protected override void ActivateEvent()
    {
        foreach (var monster in _monsters)
        {
            _player.AddMonster(monster);
        }
    }

    /// <inheritdoc cref="AbstractGameEvent.GetMonster" />
    public override IList<IMonster> GetMonster()
    {
        return _monsters.ToImmutableList();
    }
}