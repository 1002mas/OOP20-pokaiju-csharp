namespace Pokaiju.Carafassi.GameEvents
{
    /// <summary>
    /// This class allows to create an event that changes a npc dialogue text.
    /// </summary>
    public class NpcTextChanger : AbstractGameEvent
    {
        // TODO use NpcSimple
        private readonly string _npc;
        private readonly int _textId;

        /// <summary>
        /// It creates an event that allows to change a npc sentence.
        /// </summary>
        /// <param name="id">It is used to identify the event.</param>
        /// <param name="isActive">If it is active when the player interacts with a trigger,
        /// this event will use <see cref="AbstractGameEvent.Activate()"/></param>
        /// <param name="isReactivable">If the event has to be has to deactivate and never be reactivated after
        /// calling the function <see cref="AbstractGameEvent.Activate()"/></param>
        /// <param name="isToActiveImmediately">If the event has to be activated right after
        /// another event is triggered.</param>
        /// <param name="npc">The npc that changes sentence.</param>
        /// <param name="textId">Sentence ID. Be careful, there is no check about the id. Giving a wrong ID
        /// will result in an error later.</param>
        public NpcTextChanger(int id, bool isActive, bool isReactivable, bool isToActiveImmediately, string npc,
            int textId) : base(id, isActive, isReactivable, isToActiveImmediately)
        {
            _npc = npc;
            _textId = textId;
        }

        /// <inheritdoc cref="AbstractGameEvent.ActivateEvent" />
        protected override void ActivateEvent()
        {
            //TODO _npc.settextid = _textIs;
            throw new NotImplementedException();
        }
    }
}
