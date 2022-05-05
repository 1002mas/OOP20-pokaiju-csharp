namespace Pokaiju.Carafassi.GameEvents;

using Castorina.Npc;

/// <summary>
/// This class allows to create an event that changes a npc position in the same
/// map it is located.
/// </summary>
public class NpcPositionChanger : AbstractGameEvent
{
    private readonly INpcSimple _npc;
    private readonly Tuple<int, int> _position;

    /// <summary>
    /// It creates an event that allows to change npc position in the same map.
    /// </summary>
    /// <param name="id">It is used to identify the event.</param>
    /// <param name="isActive">If it is active when the player interacts with a trigger,
    /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
    /// calling the function <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isToActiveImmediately">If the event has to be activated right after
    /// another event is triggered.</param>
    /// <param name="npc">The npc you want to move.</param>
    /// <param name="position">The position you want to move it to.</param>
    public NpcPositionChanger(int id, bool isActive, bool isReactivable, bool isToActiveImmediately, INpcSimple npc,
        Tuple<int, int> position) : base(id, isActive, isReactivable, isToActiveImmediately)
    {
        _npc = npc;
        _position = position;
    }

    /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
    protected override void ActivateEvent()
    {
        _npc.ChangeNpcPosition(_position);
    }
}