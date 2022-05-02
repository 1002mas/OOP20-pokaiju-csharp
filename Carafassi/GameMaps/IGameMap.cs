using Optional;
using Pokaiju.Barattini;

namespace Pokaiju.Carafassi.GameMaps
{
    public interface IGameMap
    {
        /// <summary>
        /// It returns the current map id.
        /// </summary>
        int GetCurrentMapId();

        /// <summary>
        /// If a player can access to the given position in the current map this function will return true.
        /// </summary>
        /// <param name="block">The position where you want to know if you can pass.</param>
        bool CanPassThrough(Tuple<int, int> block);

        /// <summary>
        /// If it is a position where a wild monster may appear this function may return a wild monster.
        /// </summary>
        /// <param name="pos">The position where you want to know if a monster can appear.</param>
        Option<IMonster> GetWildMonster(Tuple<int, int> pos);

        /// <summary>
        /// It changes map if any map is linked at the given position.
        /// </summary>
        /// <param name="playerPosition">The position where you want to change map.</param>
        void ChangeMap(Tuple<int, int> playerPosition);

        /// <summary>
        /// It returns true if any map is linked to the current map at the given position.
        /// </summary>
        /// <param name="playerPosition">The position where you want to change map.</param>
        bool CanChangeMap(Tuple<int, int> playerPosition);

        /// <summary>
        /// It returns the player position in the new map if a map change has happened.
        /// If a non empty values is returned, on the next call will be returned an empty values if no new map change
        /// occurs.
        /// </summary>
        Option<Tuple<int, int>> GetPlayerMapPosition();

        /// <summary>
        /// It returns the npc at the given position in the current map if any is present.
        /// </summary>
        /// <param name="position">The position where the npc should be located.</param>
        // TODO use NpcSimple
        Option<string> GetNpcAt(Tuple<int, int> position);

        /// <summary>
        /// It returns the game event at the given position in the current map if any is present.
        /// </summary>
        /// <param name="position">The position where event should trigger.</param>
        // TODO use GameEvent
        Option<int> GetEventAt(Tuple<int, int> position);

        /// <summary>
        /// It returns a list of all enabled npcs in the current map.
        /// </summary>
        // TODO use NpcSimple
        IList<string> GetAllNpcsInCurrentMap();
    }
}