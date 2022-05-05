namespace Pokaiju.Carafassi.GameEvents;

using Castorina.Npc;

/// <summary>
/// This class allows to create an event that changes a npc visibility.
/// </summary>
public class NpcVisibilityChanger : AbstractGameEvent
{
    private readonly INpcSimple _npc;
    private readonly bool _isVisible;

    /// <summary>
    /// It creates an event that allows to hide or show a npc.
    /// </summary>
    /// <param name="id">It is used to identify the event.</param>
    /// <param name="isActive">If it is active when the player interacts with a trigger,
    /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
    /// calling the function <see cref="AbstractGameEvent.Activate()"/></param>
    /// <param name="isToActiveImmediately">If the event has to be activated right after
    /// another event is triggered.</param>
    /// <param name="npc">The npc you want to hide/show.</param>
    /// <param name="isVisible">False if you want to hide it.</param>
    public NpcVisibilityChanger(int id, bool isActive, bool isReactivable, bool isToActiveImmediately, INpcSimple npc,
        bool isVisible) : base(id, isActive, isReactivable, isToActiveImmediately)
    {
        _npc = npc;
        _isVisible = isVisible;
    }

    /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
    protected override void ActivateEvent()
    {
        _npc.SetVisible(_isVisible);
    }
}