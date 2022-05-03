using System.Collections.ObjectModel;
using Optional;
using Optional.Unsafe;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Castorina.Npc
{

    public class NpcSimple : INpcSimple
    {
        private readonly string _name;
        private TypeOfNpc _typeOfNpc;
        private IList<string> _sentences;
        private int _currentSentence;
        private ISet<IGameEvent> _events;
        private Option<IGameEvent> _triggeredEvent;
        private Tuple<int, int> _position;
        private bool _isVisible;
        private bool _isEnabled;

        protected NpcSimple(string name, TypeOfNpc typeOfNpc, IList<string> sentences,
            Tuple<int, int> position, bool isVisible, bool isEnabled)
        {
            this._name = name;
            this._typeOfNpc = typeOfNpc;
            this._sentences = sentences;
            this._position = position;
            this._currentSentence = 0;
            this._isVisible = isVisible;
            this._isEnabled = isEnabled;
            this._events = new HashSet<IGameEvent>();
            this._triggeredEvent = Option.None<IGameEvent>();
        }

        public NpcSimple(string name, IList<string> sentences, Tuple<int, int> position,
            bool isVisible, bool isEnabled) : this(name, TypeOfNpc.SIMPLE, sentences, position, isVisible, isEnabled)
        {
            throw new NotImplementedException();
        }


        public string GetName()
        {
            return this._name;
        }


        public virtual Option<string> InteractWith()
        {
            //TODO complete
            if (_isEnabled)
            {
                Option<string> result = Option.Some(_sentences.ElementAt(_currentSentence));
                Option<IGameEvent> activeEvent = Option.None<IGameEvent>();
                foreach (var firstEvent in this._events)
                {
                    if (firstEvent.Active)
                    {
                        activeEvent = Option.Some(firstEvent);
                        break;
                    }
                }
                if (activeEvent.HasValue)
                {
                    this._triggeredEvent = activeEvent;
                    activeEvent.ValueOrFailure().Activate();
                }
                else
                {
                    this._triggeredEvent = Option.None<IGameEvent>();
                }

                return result;
            }

            return Option.None<string>();


        }

        public Option<IGameEvent> GetTriggeredEvent()
        {
            return this._triggeredEvent;
        }


        public void SetDialogueText(int textId)
        {
            if (textId >= 0 && textId < _sentences.Count)
            {
                this._currentSentence = textId;
            }
        }

        public TypeOfNpc GetTypeOfNpc()
        {
            return this._typeOfNpc;
        }


        public Tuple<int, int> GetPosition()
        {
            return this._position;
        }


        public void ChangeNpcPosition(Tuple<int, int> newPosition)
        {
            this._position = newPosition;
        }

        public bool IsVisible()
        {
            return this._isVisible;
        }

        public void SetVisible(bool visible)
        {
            this._isVisible = visible;
        }

        public bool IsEnabled()
        {
            return this._isEnabled;
        }

        public void SetEnabled(bool enabled)
        {
            this._isEnabled = enabled;
        }

        public void AddGameEvent(IGameEvent gameEvent)
        {
            this._events.Add(gameEvent);
        }

        public IList<IGameEvent> GetGameEvents()
        {
             return new ReadOnlyCollection<IGameEvent>( new List<IGameEvent>(this._events));
        }

        public int GetCurrentSetence()
        {
            return this._currentSentence;
        }
    }
}   
