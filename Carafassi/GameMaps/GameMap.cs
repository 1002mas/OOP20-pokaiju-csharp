namespace Pokaiju.Carafassi.GameMaps;

using Optional;
using Optional.Unsafe;
using Barattini;
using GameEvents;
using Castorina.Npc;
using Pierantoni;

public class GameMap : IGameMap
{
    private const int MonsterSpawnRate = 5;
    private const int MaximumMonsterSpawnRate = 10;
    private IGameMapData _mapData;
    private Option<Tuple<int, int>> _enteringStartPosition;

    /// <summary>
    /// It manages maps and map switching.
    /// </summary>
    /// <param name="mapData">Initial map.</param>
    public GameMap(IGameMapData mapData)
    {
        _mapData = mapData;
        _enteringStartPosition = Option.None<Tuple<int, int>>();
    }

    /// <inheritdoc cref="IGameMap.GetCurrentMapId"/>>
    public int GetCurrentMapId()
    {
        return _mapData.MapId;
    }

    /// <inheritdoc cref="IGameMap.CanPassThrough"/>>
    public bool CanPassThrough(Tuple<int, int> block)
    {
        var npc = _mapData.GetNpc(block);
        return _mapData.GetBlockType(block).CanPassThrough() &&
               (!npc.HasValue || npc.HasValue && !npc.ValueOrFailure().IsEnabled());
    }

    private IList<Tuple<IMoves, int>> GetRandomListMoves(IMonsterSpecies species)
    {
        const int maxProb = 10;
        const int prob = 5;
        var movesList = new List<Tuple<IMoves, int>>();
        var learnableMoves = species.GetAllLearnableMoves();
        var rnd = new Random();
        foreach (var m in learnableMoves)
        {
            if (movesList.Count < Monster.NumMaxMoves && rnd.Next(maxProb) < prob)
            {
                movesList.Add(new Tuple<IMoves, int>(m, m.GetPp()));
            }
            else
            {
                break;
            }
        }

        if (!movesList.Any() && learnableMoves.Any())
        {
            movesList.Add(new Tuple<IMoves, int>(learnableMoves[0], learnableMoves[0].GetPp()));
        }

        return movesList;
    }

    /// <inheritdoc cref="IGameMap.GetWildMonster"/>>
    public Option<IMonster> GetWildMonster(Tuple<int, int> pos)
    {
        var monsters = _mapData.GetMonstersInArea();
        var rnd = new Random();

        if (!_mapData.GetBlockType(pos).CanMonstersSpawn() || !monsters.Any()
                                                           || rnd.Next(MaximumMonsterSpawnRate) >
                                                           MonsterSpawnRate)
        {
            return Option.None<IMonster>();
        }

        var species = monsters[rnd.Next(monsters.Count)];
        var monsterLevel = rnd.Next(_mapData.GetWildMonsterLevelRange().Item1,
            _mapData.GetWildMonsterLevelRange().Item2);
        var m = new MonsterBuilder().Species(species).Wild(true)
            .Exp(0).Level(monsterLevel)
            .MovesList(GetRandomListMoves(species)).Build();
        return m.Some();
    }

    /// <inheritdoc cref="IGameMap.ChangeMap"/>>
    public void ChangeMap(Tuple<int, int> playerPosition)
    {
        if (CanChangeMap(playerPosition) && _mapData.GetNextMap(playerPosition).HasValue)
        {
            var p = _mapData.GetNextMap(playerPosition).ValueOrFailure();
            _enteringStartPosition = p.Item2.Some();
            _mapData = p.Item1;
        }
        else
        {
            throw new ArgumentException("Cannot change map for the given position.");
        }
    }

    /// <inheritdoc cref="IGameMap.CanChangeMap"/>>
    public bool CanChangeMap(Tuple<int, int> playerPosition)
    {
        return _mapData.GetBlockType(playerPosition).Equals(MapBlockType.MapChange);
    }

    /// <inheritdoc cref="IGameMap.GetPlayerMapPosition"/>>
    public Option<Tuple<int, int>> GetPlayerMapPosition()
    {
        var temp = _enteringStartPosition;
        _enteringStartPosition = Option.None<Tuple<int, int>>();
        return temp;
    }

    /// <inheritdoc cref="IGameMap.GetNpcAt"/>>
    public Option<INpcSimple> GetNpcAt(Tuple<int, int> position)
    {
        var npc = _mapData.GetNpc(position);
        if (!npc.HasValue || !npc.ValueOrFailure().IsEnabled())
        {
            return Option.None<INpcSimple>();
        }

        return npc;
    }

    /// <inheritdoc cref="IGameMap.GetEventAt"/>>
    public Option<IGameEvent> GetEventAt(Tuple<int, int> position)
    {
        var gameEvent = _mapData.GetEvent(position);
        return gameEvent.HasValue && gameEvent.ValueOrFailure().Active ? gameEvent : Option.None<IGameEvent>();
    }

    /// <inheritdoc cref="IGameMap.GetAllNpcsInCurrentMap"/>>
    public IList<INpcSimple> GetAllNpcsInCurrentMap()
    {
        return _mapData.GetAllNpcs();
    }

    /// <inheritdoc cref="object.ToString"/>>
    public override string? ToString()
    {
        return _mapData.ToString();
    }
}