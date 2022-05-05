using Pokaiju.Barattini;

namespace Pokaiju.Carafassi.GameEvents
{
    /// <summary>
    /// It implements some generic game event functions.
    /// </summary>
    public abstract class AbstractGameEvent : IGameEvent
    {
        private bool _hasBeenActivated;
        private bool _active;
        private readonly IList<IGameEvent> _eventsToActivate = new List<IGameEvent>();
        private readonly IList<IGameEvent> _eventsToDeactivate = new List<IGameEvent>();

        /// <summary>
        /// It creates a game event.
        /// </summary>
        /// <param name="id">It is used to identify the event.</param>
        /// <param name="isActive">If it is active when the player interacts with a trigger,
        /// this event will use <see cref="Activate()"/></param>
        /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
        /// calling the function <see cref="Activate()"/></param>
        /// <param name="isToActiveImmediately">If the event has to be activated right after
        /// another event is triggered.</param>
        protected AbstractGameEvent(int id, bool isActive, bool isReactivable, bool isToActiveImmediately)
        {
            EventId = id;
            IsToActivateImmediately = isToActiveImmediately;
            IsReactivable = isReactivable;
            Active = isActive;
            _hasBeenActivated = isActive;
        }

        /// <inheritdoc cref="IGameEvent.EventId" />
        public int EventId { get; }

        /// <inheritdoc cref="IGameEvent.Active" />
        public bool Active
        {
            get => _active;
            set
            {
                if (_hasBeenActivated && !IsReactivable)
                {
                    _active = false;
                }
                else
                {
                    _active = value;
                }

                if (_active)
                {
                    _hasBeenActivated = true;
                }
            }
        }

        /// <inheritdoc cref="IGameEvent.IsToActivateImmediately" />
        public bool IsToActivateImmediately { get; }

        /// <inheritdoc cref="IGameEvent.IsReactivable" />
        public bool IsReactivable { get; }


        /// <inheritdoc cref="IGameEvent.AddDependentGameEvent" />
        public void AddDependentGameEvent(IGameEvent e)
        {
            _eventsToDeactivate.Add(e);
        }

        /// <inheritdoc cref="IGameEvent.AddSuccessiveGameEvent" />
        public void AddSuccessiveGameEvent(IGameEvent e)
        {
            _eventsToActivate.Add(e);
        }

        /// <inheritdoc cref="IGameEvent.IsBattle" />
        public virtual bool IsBattle()
        {
            return false;
        }

        /// <inheritdoc cref="IGameEvent.GetMonster" />
        public virtual IList<IMonster> GetMonster()
        {
            return new List<IMonster>();
        }

        /// <summary>
        /// In this function you need to implement the event action.
        /// </summary>
        protected abstract void ActivateEvent();

        /// <inheritdoc cref="IGameEvent.Activate" />
        public void Activate()
        {
            if (Active)
            {
                ActivateEvent();

                // disable events
                foreach (IGameEvent e in _eventsToDeactivate)
                {
                    e.Active = false;
                }

                // enable events
                foreach (IGameEvent e in _eventsToActivate)
                {
                    e.Active = true;
                    if (e.IsToActivateImmediately)
                    {
                        e.Activate();
                    }
                }

                //disable this event
                Active = false;
            }
        }

        private bool Equals(AbstractGameEvent other)
        {
            return EventId == other.EventId;
        }

        /// <inheritdoc cref="object.Equals(object?)"/>>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AbstractGameEvent) obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>>
        public override int GetHashCode()
        {
            return EventId;
        }

        /// <inheritdoc cref="object.ToString"/>>
        public override string ToString()
        {
            return "[Event ID: " + EventId + ", active: " + Active + "]";
        }
    }
}