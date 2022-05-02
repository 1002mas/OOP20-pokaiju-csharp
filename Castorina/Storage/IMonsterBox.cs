using Optional;
using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Storage
{
    public interface IMonsterBox
    {   
        /// <summary>
        /// This function returns a list of all monster in box.
        /// </summary>
        /// <returns>list of monsters</returns>
        IList<IMonster> GetAllMonsters();

        /// <summary>
        /// This function tries to add a monster in box.
        /// </summary>
        /// <param name="monster"></param>
        /// <returns>true if monster was added, false otherwise</returns>
        bool AddMonster(IMonster monster);

        /// <summary>
        /// this function tries to exchange two monsters, one from the box to player team
        /// with one from player team to the box.
        /// </summary>
        /// <param name="toBox"></param>
        /// <param name="monsterId"></param>
        /// <returns>an optional of monster if exchange has been done, an empty optional otherwise</returns>
        Option<IMonster> Exchange(IMonster toBox, int monsterId);

        /// <summary>
        /// This function returns a monster if it is in the box.
        /// </summary>
        /// <param name="monsterId"></param>
        /// <returns>optional of a monster if it is in box, an empty optional otherwise</returns>
        Option<IMonster> GetMonster(int monsterId);

        /// <summary>
        /// This function returns box name.
        /// </summary>
        /// <returns>box name</returns>
        string GetName();

        /// <summary>
        /// This function returns if the box is full.
        /// </summary>
        /// <returns>true if box is full, false otherwise</returns>
        bool IsFull();

        /// <summary>
        /// This function removes a monster in box if it is present.
        /// </summary>
        /// <param name="monsterId"></param>
        void RemoveMonster(int monsterId);

    }
}
    

