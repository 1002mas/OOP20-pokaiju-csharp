namespace Pokaiju.Carafassi.GameEvents;

using Barattini;

/// <summary>
/// It represents a generic game event.
/// </summary>
public interface IGameEvent
{
    /// <summary>
    /// It returns the event id.
    /// </summary>
    int EventId { get; }

    /// <summary>
    /// It gets whether the event is active or it activates/deactivates the event.
    /// </summary>
    bool Active { get; set; }


    /// <summary>
    /// It returns if the event is to trigger after another event activate it.
    /// </summary>
    bool IsToActivateImmediately { get; }

    /// <summary>
    /// It returns true if it is an event that can be reactivated.
    /// </summary>
    bool IsReactivable { get; }

    /// <summary>
    /// It adds an event that has to be set inactive after this one is triggered.
    /// </summary>
    /// <param name="e">The event you want to deactivate.</param>
    void AddDependentGameEvent(IGameEvent e);

    /// <summary>
    /// It adds an event that has to be set active after this one is triggered.
    /// </summary>
    /// <param name="e">The event you want to activate.</param>
    void AddSuccessiveGameEvent(IGameEvent e);

    /// <summary>
    /// It returns true if it is a battle event.
    /// </summary>
    bool IsBattle();

    /// <summary>
    /// If it is an event that gives monsters to the player or a battle event it will return the monsters added to
    /// the player collection or the monster to battle.
    /// </summary>
    IList<IMonster> GetMonster();

    /// <summary>
    /// This function make the event happens. If the event is not active nothing will
    /// happen.
    /// </summary>
    void Activate();
}