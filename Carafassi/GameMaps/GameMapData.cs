namespace Pokaiju.Carafassi.GameMaps;

using System.Collections.Immutable;
using Optional;
using Barattini;
using GameEvents;
using Castorina.Npc;

public class GameMapData : IGameMapData
{
    private readonly int _minimumMonsterLevel;
    private readonly int _maximumMonsterLevel;
    private readonly IDictionary<Tuple<int, int>, MapBlockType> _blocks;
    private readonly IList<IMonsterSpecies> _wildMonsters;
    private readonly IDictionary<Tuple<int, int>, IGameMapData> _linkedMaps;
    private readonly IDictionary<IGameMapData, Tuple<int, int>> _linkedMapsStartingPosition;

    private readonly IDictionary<Tuple<int, int>, IGameEvent> _eventLocation;
    private readonly ISet<INpcSimple> _npcs;

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
        _npcs = new HashSet<INpcSimple>();
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
    public void AddNpc(INpcSimple npc)
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
        return _blocks.ContainsKey(block) ? _blocks[block] : MapBlockType.Obstacle;
    }

    /// <inheritdoc cref="IGameMapData.GetMonstersInArea"/>>
    public IList<IMonsterSpecies> GetMonstersInArea()
    {
        return _wildMonsters.ToImmutableList();
    }

    /// <inheritdoc cref="IGameMapData.GetNpc"/>>
    public Option<INpcSimple> GetNpc(Tuple<int, int> block)
    {
        return _npcs.Any(npc => npc.GetPosition().Equals(block))
            ? _npcs.First(npc => npc.GetPosition().Equals(block)).Some()
            : Option.None<INpcSimple>();
    }

    /// <inheritdoc cref="IGameMapData.GetEvent"/>>
    public Option<IGameEvent> GetEvent(Tuple<int, int> block)
    {
        return _eventLocation.ContainsKey(block)
            ? _eventLocation[block].Some()
            : Option.None<IGameEvent>();
    }

    /// <inheritdoc cref="IGameMapData.GetNextMap"/>>
    public Option<Tuple<IGameMapData, Tuple<int, int>>> GetNextMap(Tuple<int, int> playerPosition)
    {
        return _linkedMaps.ContainsKey(playerPosition) &&
               _linkedMapsStartingPosition.ContainsKey(_linkedMaps[playerPosition])
            ? new Tuple<IGameMapData, Tuple<int, int>>(_linkedMaps[playerPosition],
                _linkedMapsStartingPosition[_linkedMaps[playerPosition]]).Some()
            : Option.None<Tuple<IGameMapData, Tuple<int, int>>>();
    }

    /// <inheritdoc cref="IGameMapData.GetAllNpcs"/>>
    public IList<INpcSimple> GetAllNpcs()
    {
        return _npcs.ToImmutableList();
    }

    /// <inheritdoc cref="IGameMapData.AddEventAt"/>>
    public void AddEventAt(IGameEvent e, Tuple<int, int> block)
    {
        if (_eventLocation.ContainsKey(block))
        {
            _eventLocation.Remove(block);
        }

        _eventLocation.Add(block, e);
    }

    /// <summary>
    /// It checks if a MapData is equal to another one field by field and property by property.
    /// </summary>
    private bool Equals(IGameMapData other)
    {
        return MapId == other.MapId;
    }

    /// <inheritdoc cref="object.Equals(object?)"/>>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((GameMapData) obj);
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