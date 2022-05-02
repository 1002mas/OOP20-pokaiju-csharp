namespace Pokaiju.Carafassi.GameEvents
{
    /// <summary>
    /// This class allows to create an event that enable or disable the interaction
    /// with a npc.
    /// </summary>
    public class NpcActivityChanger : AbstractGameEvent
    {
        // TODO use NpcSimple
        private readonly string _npc;
        private readonly bool _isEnabled;

        /// <summary>
        /// It creates an event that allows to disable or enable a npc. Disabling the npc
        /// makes the player unable to interact with the npc.
        /// </summary>
        /// <param name="id">It is used to identify the event.</param>
        /// <param name="isActive">If it is active when the player interacts with a trigger,
        /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
        /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
        /// calling the function <see cref="AbstractGameEvent.Activate()"/></param>
        /// <param name="isToActiveImmediately">If the event has to be activated right after
        /// another event is triggered.</param>
        /// <param name="npc">The npc you want to disable/enable.</param>
        /// <param name="isEnabled">False if you want to deactivate the interaction with the npc.</param>
        public NpcActivityChanger(int id, bool isActive, bool isReactivable, bool isToActiveImmediately,
            string npc, bool isEnabled) : base(id, isActive, isReactivable, isToActiveImmediately)
        {
            _npc = npc;
            _isEnabled = isEnabled;
        }

        /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
        protected override void ActivateEvent()
        {
            //TODO _npc.setEnabled = true;
            throw new NotImplementedException();
        }
    }
}
