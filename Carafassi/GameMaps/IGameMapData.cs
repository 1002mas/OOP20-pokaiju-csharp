using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Carafassi.GameMaps
{
    /// <summary>
    /// It represents raw data about a map.
    /// </summary>
    public interface IGameMapData
    {
        /// <summary>
        /// It returns the map id.
        /// </summary>
        int MapId { get; }

        /// <summary>
        /// It returns the current map name.
        /// </summary>
        string MapName { get; }

        /// <summary>
        /// It links a map to this map. If you want a bidirectional link, you need to link
        /// this to the other one as well.
        /// </summary>
        /// <param name="map">The linked map.</param>
        /// <param name="mapLinkPosition">The position in the current map to reach the given map.</param>
        /// <param name="characterSpawn">The player position when the map is changed to map.</param>
        void AddMapLink(GameMapData map, Tuple<int, int> mapLinkPosition, Tuple<int, int> characterSpawn);

        /// <summary>
        /// It adds the npc in the map.
        /// </summary>
        /// <param name="npc">The npc you want to add to the map</param>
        //TODO use NpcSimple
        void AddNpc(String npc);

        /// <summary>
        /// A tuple containing the minimum and maximum level for monsters in the area.
        /// </summary>
        Tuple<int, int> GetWildMonsterLevelRange();

        /// <summary>
        /// It returns what kind of block is present in a given position.
        /// </summary>
        /// <param name="block">The position in the map.</param>
        MapBlockType GetBlockType(Tuple<int, int> block);

        /// <summary>
        /// It returns a list of all wild monsters that may appears in the area.
        /// </summary>
        IList<IMonsterSpecies> GetMonstersInArea();

        /// <summary>
        /// It returns a npc if it is present in the given position.
        /// </summary>
        /// <param name="block">The position in the map.</param>
        // TODO use NpcSimple
        Option<string> GetNpc(Tuple<int, int> block);

        /// <summary>
        /// It returns an event if it is present in the given position.
        /// </summary>
        /// <param name="block">The position in the map.</param>
        Option<IGameEvent> GetEvent(Tuple<int, int> block);

        /// <summary>
        /// It returns the map linked to the position PlayerPosition and the place
        /// where the player appears after map change.
        /// </summary>
        /// <param name="playerPosition">The position in the map.</param>
        Option<Tuple<IGameMapData, Tuple<int, int>>> GetNextMap(Tuple<int, int> playerPosition);

        /// <summary>
        /// A list containing all npcs in the map
        /// </summary>
        // TODO use NpcSimple
        IList<string> GetAllNpcs();

        /// <summary>
        /// It adds an event to the given position.
        /// </summary>
        /// <param name="e">The event you want to add</param>
        /// <param name="block">The position in the map.</param>
        void AddEventAt(IGameEvent e, Tuple<int, int> block);
    }
}