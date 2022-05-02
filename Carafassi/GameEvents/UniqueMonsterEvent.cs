using Pokaiju.Barattini;

namespace Pokaiju.Carafassi.GameEvents
{
    /// <summary>
    /// This class allows to create an event that creates a battle with a monster.
    /// Once the monster has ended, it cannot be repeated.
    /// </summary>
    public class UniqueMonsterEvent : AbstractGameEvent
    {
        private readonly IMonster _monster;

        /// <summary>
        /// It creates an event that allows to create a battle with a monster. This event
        /// is set to deactivate automatically after it happens.
        /// </summary>
        /// <param name="id">It is used to identify the event.</param>
        /// <param name="isActive">If it is active when the player interacts with a trigger,
        /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
        /// <param name="isToActiveImmediately">If the event has to be activated right after
        /// another event is triggered.</param>
        /// <param name="monster">The monster you want to battle with the player.</param>
        public UniqueMonsterEvent(int id, bool isActive, bool isToActiveImmediately, IMonster monster)
            : base(id, isActive, false, isToActiveImmediately)
        {
            _monster = monster;
        }

        /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
        protected override void ActivateEvent()
        {
        }

        /// <inheritdoc cref="AbstractGameEvent.GetMonster" />
        new IList<IMonster> GetMonster()
        {
            return new List<IMonster> {_monster};
        }

        /// <inheritdoc cref="AbstractGameEvent.IsBattle" />
        new bool IsBattle()
        {
            return true;
        }
    }
}