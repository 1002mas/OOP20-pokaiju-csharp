using Pokaiju.Barattini;


namespace Pokaiju.Castorina.Npc
{
    public interface INpcTrainer : INpcSimple
    {
        /// <summary>
        /// This function returns trainer monsters owned.
        /// </summary>
        /// <returns>a list of monsters</returns>
        IList<IMonster> GetMonstersOwned();

        /// <summary>
        /// This function returns if a trainer is defeated.
        /// </summary>
        /// <returns>true if is defeated, false otherwise</returns>
        bool IsDefeated();
        
        /// <summary>
        /// This function set if trainer is defeated.
        /// </summary>
        /// <param name="isDefeated"></param>
        void SetDefeated(bool isDefeated);
    }
}
