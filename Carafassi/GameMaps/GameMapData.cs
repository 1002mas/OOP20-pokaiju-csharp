using System.Collections.Immutable;
using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Carafassi.GameMaps
{
    public class GameMapData : IGameMapData
    {
        private readonly int _minimumMonsterLevel;
        private readonly int _maximumMonsterLevel;
        private readonly IDictionary<Tuple<int, int>, MapBlockType> _blocks;
        private readonly IList<IMonsterSpecies> _wildMonsters;
        private readonly IDictionary<Tuple<int, int>, IGameMapData> _linkedMaps;
        private readonly IDictionary<IGameMapData, Tuple<int, int>> _linkedMapsStartingPosition;

        private readonly IDictionary<Tuple<int, int>, IGameEvent> _eventLocation;

        // TODO use NpcSimple
        private readonly ISet<string> _npcs;

        public GameMapData(int mapId, string mapName, int minimumMonsterLevel, int maximumMonsterLevel,
            IDictionary<Tuple<int, int>, MapBlockType> blocks, IList<IMonsterSpecies> wildMonsters)
        {
            MapId = mapId;
            MapName = mapName;
            _minimumMonsterLevel = minimumMonsterLevel;
            _maximumMonsterLevel = maximumMonsterLevel;
            _blocks = blocks;
            _wildMonsters = wildMonsters;
            _linkedMaps = new Dictionary<Tuple<int, int>, IGameMapData>();
            _linkedMapsStartingPosition = new Dictionary<IGameMapData, Tuple<int, int>>();
            _eventLocation = new Dictionary<Tuple<int, int>, IGameEvent>();
            _npcs = new HashSet<string>();
        }

        /// <inheritdoc cref="IGameMapData.MapId"/>>
        public int MapId { get; }

        /// <inheritdoc cref="IGameMapData.MapName"/>>
        public string MapName { get; }

        /// <inheritdoc cref="IGameMapData.AddMapLink"/>>
        public void AddMapLink(GameMapData map, Tuple<int, int> mapLinkPosition, Tuple<int, int> characterSpawn)
        {
            _linkedMaps.Add(mapLinkPosition, map);
            _linkedMapsStartingPosition.Add(map, characterSpawn);
        }

        /// <inheritdoc cref="IGameMapData.AddNpc"/>>
        public void AddNpc(string npc)
        {
            if (_npcs.Contains(npc))
            {
                _npcs.Remove(npc);
            }

            _npcs.Add(npc);
        }

        /// <inheritdoc cref="IGameMapData.GetWildMonsterLevelRange"/>>
        public Tuple<int, int> GetWildMonsterLevelRange()
        {
            return new Tuple<int, int>(_minimumMonsterLevel, _maximumMonsterLevel);
        }

        /// <inheritdoc cref="IGameMapData.GetBlockType"/>>
        public MapBlockType GetBlockType(Tuple<int, int> block)
        {
            return _blocks[block];
        }

        /// <inheritdoc cref="IGameMapData.GetMonstersInArea"/>>
        public IList<IMonsterSpecies> GetMonstersInArea()
        {
            return _wildMonsters.ToImmutableList();
        }

        /// <inheritdoc cref="IGameMapData.GetNpc"/>>
        public Option<string> GetNpc(Tuple<int, int> block)
        {
            string? npcFound = null;
            foreach (string npc in _npcs)
            {
                if ( /*npc.position.equals(block)*/true)
                {
                    npcFound = npc;
                    break;
                }
            }

            return npcFound is null ? Option.None<string>() : Option.Some<>(npcFound);
        }

        /// <inheritdoc cref="IGameMapData.GetEvent"/>>
        public Option<IGameEvent> GetEvent(Tuple<int, int> block)
        {
            return _eventLocation.ContainsKey(block)
                ? Option.Some<>(_eventLocation[block])
                : Option.None<IGameEvent>();
        }

        /// <inheritdoc cref="IGameMapData.GetNextMap"/>>
        public Option<Tuple<IGameMapData, Tuple<int, int>>> GetNextMap(Tuple<int, int> playerPosition)
        {
            return _linkedMaps.ContainsKey(playerPosition) &&
                   _linkedMapsStartingPosition.ContainsKey(_linkedMaps[playerPosition])
                ? Option.Some<>(
                    new Tuple<IGameMapData, Tuple<int, int>>(_linkedMaps[playerPosition],
                        _linkedMapsStartingPosition[_linkedMaps[playerPosition]]))
                : Option.None<Tuple<IGameMapData, Tuple<int, int>>>();
        }

        /// <inheritdoc cref="IGameMapData.GetAllNpcs"/>>
        public IList<string> GetAllNpcs()
        {
            return _npcs.ToImmutableList();
        }

        /// <inheritdoc cref="IGameMapData.AddEventAt"/>>
        public void AddEventAt(IGameEvent e, Tuple<int, int> block)
        {
            _eventLocation.Add(block, e);
        }

        /// <summary>
        /// It checks if a MapData is equal to another one field by field and property by property.
        /// </summary>
        private bool Equals(GameMapData other)
        {
            return MapId == other.MapId;
        }

        /// <inheritdoc cref="object.Equals(object?)"/>>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameMapData) obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>>
        public override int GetHashCode()
        {
            return MapId;
        }

        /// <inheritdoc cref="object.ToString"/>>
        public override string ToString()
        {
            return "ID: " + MapId + " \nname:" + MapName;
        }
    }
}