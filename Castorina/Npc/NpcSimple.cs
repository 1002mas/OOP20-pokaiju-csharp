using System.Collections.ObjectModel;
using Optional;
using Optional.Unsafe;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Castorina.Npc;


public class NpcSimple : INpcSimple
{

    private readonly string _name;
    private readonly TypeOfNpc _typeOfNpc;
    private readonly IList<string> _sentences;
    private int _currentSentence;
    private readonly ISet<IGameEvent> _events;
    private Option<IGameEvent> _triggeredEvent;
    private Tuple<int, int> _position;
    private bool _isVisible;
    private bool _isEnabled;

    /// <summary>
    /// Protected constructor for NpcSimple
    /// </summary>
    /// <param name="name"></param>
    /// <param name="typeOfNpc"></param>
    /// <param name="sentences"></param>
    /// <param name="position"></param>
    /// <param name="isVisible"></param>
    /// <param name="isEnabled"></param>
    protected NpcSimple(string name, TypeOfNpc typeOfNpc, IList<string> sentences,
        Tuple<int, int> position, bool isVisible, bool isEnabled)
    {
        _name = name;
        _typeOfNpc = typeOfNpc;
        _sentences = sentences;
        _position = position;
        _currentSentence = 0;
        _isVisible = isVisible;
        _isEnabled = isEnabled;
        _events = new HashSet<IGameEvent>();
        _triggeredEvent = Option.None<IGameEvent>();
    }

    /// <summary>
    /// Constructor for NpcSimple
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sentences"></param>
    /// <param name="position"></param>
    /// <param name="isVisible"></param>
    /// <param name="isEnabled"></param>
    /// <exception cref="NotImplementedException"></exception>
    public NpcSimple(string name, IList<string> sentences, Tuple<int, int> position,
        bool isVisible, bool isEnabled) : this(name, TypeOfNpc.Simple, sentences, position, isVisible, isEnabled)
    {

    }

    /// <inheritdoc cref="INpcSimple.GetName"/>
    public string GetName()
    {
        return _name;
    }

    /// <inheritdoc cref="INpcSimple.InteractWith"/>
    public virtual Option<string> InteractWith()
    {
        if (!_isEnabled) return Option.None<string>();
        var result = Option.Some(_sentences.ElementAt(_currentSentence));
        var activeEvent = Option.None<IGameEvent>();
        foreach (var firstEvent in _events)
        {
            if (!firstEvent.Active) continue;
            activeEvent = Option.Some(firstEvent);
            break;
        }

        if (activeEvent.HasValue)
        {
            _triggeredEvent = activeEvent;
            activeEvent.ValueOrFailure().Activate();
        }
        else
        {
            _triggeredEvent = Option.None<IGameEvent>();
        }

        return result;


    }

    /// <inheritdoc cref="INpcSimple.GetTriggeredEvent"/>
    public Option<IGameEvent> GetTriggeredEvent()
    {
        return _triggeredEvent;
    }

    /// <inheritdoc cref="INpcSimple.SetDialogueText"/>
    public void SetDialogueText(int textId)
    {
        if (textId >= 0 && textId < _sentences.Count)
        {
            _currentSentence = textId;
        }
    }

    /// <inheritdoc cref="INpcSimple.GetTypeOfNpc"/>
    public TypeOfNpc GetTypeOfNpc()
    {
        return _typeOfNpc;
    }

    /// <inheritdoc cref="INpcSimple.GetPosition"/>
    public Tuple<int, int> GetPosition()
    {
        return _position;
    }

    /// <inheritdoc cref="INpcSimple.ChangeNpcPosition"/>
    public void ChangeNpcPosition(Tuple<int, int> newPosition)
    {
        _position = newPosition;
    }

    /// <inheritdoc cref="INpcSimple.IsVisible"/>
    public bool IsVisible()
    {
        return _isVisible;
    }

    /// <inheritdoc cref="INpcSimple.SetVisible"/>
    public void SetVisible(bool visible)
    {
        _isVisible = visible;
    }

    /// <inheritdoc cref="INpcSimple.IsEnabled"/>
    public bool IsEnabled()
    {
        return _isEnabled;
    }

    /// <inheritdoc cref="INpcSimple.SetEnabled"/>
    public void SetEnabled(bool enabled)
    {
        _isEnabled = enabled;
    }

    /// <inheritdoc cref="INpcSimple.AddGameEvent"/>
    public void AddGameEvent(IGameEvent gameEvent)
    {
        _events.Add(gameEvent);
    }

    /// <inheritdoc cref="INpcSimple.GetGameEvents"/>
    public IList<IGameEvent> GetGameEvents()
    {
        return new ReadOnlyCollection<IGameEvent>(new List<IGameEvent>(_events));
    }

    /// <inheritdoc cref="INpcSimple.GetCurrentSetence"/>
    public int GetCurrentSetence()
    {
        return _currentSentence;
    }
}   
