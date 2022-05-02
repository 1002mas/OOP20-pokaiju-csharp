using Microsoft.VisualBasic;
using Optional;
using Optional.Unsafe;
using Pokaiju.Carafassi.GameEvents;
using Pokaiju.Castorina.Npc;

namespace Pokaiju.Castorina.Npc
{

    public class NpcSimpleImpl : INpcSimple
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

        protected NpcSimpleImpl(string name, TypeOfNpc typeOfNpc, IList<string> sentences,
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

        public NpcSimpleImpl(string name, IList<string> sentences, Tuple<int, int> position,
            bool isVisible, bool isEnabled)
        {
            this(name, TypeOfNpc.SIMPLE, sentences, position, isVisible, isEnabled);
        }

        public string GetName()
        {
            return this._name;
        }


        public Option<string> InteractWith()
        {
            //TODO complete
            if (_isEnabled)
            {
                Option<string> result = Option.Some(_sentences.ElementAt(_currentSentence));
                Option<IGameEvent> activeEvent = _events.stream().filter(ge->ge.isActive()).findFirst();
                if (activeEvent.HasValue)
                {
                    this._triggeredEvent = activeEvent;
                    activeEvent.ValueOr(Option.None<IGameEvent>()).Active();
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
            //TODO comlpete
        }

        public int GetCurrentSetence()
        {
            return this._currentSentence;
        }
    }
}   
