namespace Pokaiju.Barattini;

using System.Collections.Immutable;
using Guo.GameItem;
using Pierantoni;
using Optional.Unsafe;

public class Monster : IMonster
{
    /// <summary>
    /// Max Experience
    /// </summary>
    public const int ExpCap = 1000;

    /// <summary>
    /// Max level
    /// </summary>
    public const int MaxLvl = 100;

    /// <summary>
    /// Number of max moves
    /// </summary>
    public const int NumMaxMoves = 4;

    private const int MaxHpStep = 40;
    private const int MinHpStep = 10;
    private const int MaxStatsStep = 10;
    private const int MinStatsStep = 1;
    private int _exp;
    private int _level;
    private readonly bool _isWild;
    private IMonsterSpecies _species;
    private readonly IList<Tuple<IMoves, int>> _movesList;
    private readonly IMonsterStats _stats;
    private readonly IMonsterStats _maxStats;

    /// <summary>
    /// Constructor for MonsterImpl.
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="stats">stats</param>
    /// <param name="exp">exp</param>
    /// <param name="level">level</param>
    /// <param name="isWild">isWild</param>
    /// <param name="species">species</param>
    /// <param name="movesList">movesList</param>
    public Monster(int id, IMonsterStats stats, int exp, int level, bool isWild,
        IMonsterSpecies species, IEnumerable<Tuple<IMoves, int>> movesList)
    {
        Id = id;
        _maxStats =
            new MonsterStats(stats.Health, stats.Attack, stats.Defense, stats.Speed);
        _stats =
            new MonsterStats(_maxStats.Health, _maxStats.Attack, _maxStats.Defense, _maxStats.Speed);
        _exp = exp;
        _level = level;
        _isWild = isWild;
        _species = species;
        _movesList = new List<Tuple<IMoves, int>>(movesList);
    }


    /// <inheritdoc cref="IMonster.Id"/>
    public int Id { get; }

    /// <inheritdoc cref="IMonster.GetName"/>
    public string GetName()
    {
        return _species.GetName();
    }

    /// <inheritdoc cref="IMonster.SetHealth"/>
    public void SetHealth(int hlt)
    {
        _stats.Health = hlt <= GetMaxHealth() ? hlt : GetMaxHealth();
        _stats.Health = hlt <= 0 ? 0 : _stats.Health;
    }

    /// <inheritdoc cref="IMonster.RestoreStats"/>
    public void RestoreStats()
    {
        var maxStatsMap = _maxStats.GetStatsAsMap();
        var statsMap = _stats.GetStatsAsMap();
        foreach (var s in maxStatsMap)
        {
            statsMap[s.Key] = s.Value;
        }
    }

    /// <inheritdoc cref="IMonster.GetMaxHealth"/>
    public int GetMaxHealth()
    {
        return _maxStats.Health;
    }

    /// <inheritdoc cref="IMonster.GetInfo"/>
    public string GetInfo()
    {
        return _species.GetInfo();
    }

    /// <inheritdoc cref="IMonster.GetLevel"/>
    public int GetLevel()
    {
        return _level;
    }

    private void SetLevel(int level)
    {
        _level = level <= MaxLvl ? level : MaxLvl;
    }

    /// <inheritdoc cref="IMonster.LevelUp"/>
    public void LevelUp()
    {
        _level++;
        _exp = 0;
        OnLevelUp(1);
    }

    /// <inheritdoc cref="IMonster.IncExp"/>
    public void IncExp(int exp)
    {
        var incLevel = (_exp + exp) / ExpCap;
        var oldLevel = _level;
        SetLevel(incLevel + _level);
        incLevel = _level - oldLevel;
        _exp = (_exp + exp) % ExpCap;
        if (_level == MaxLvl)
        {
            _exp = 0;
        }

        if (incLevel > 0)
        {
            OnLevelUp(incLevel);
        }
    }

    private void OnLevelUp(int level)
    {
        var rand = new Random();
        for (var i = 0; i < level; i++)
        {
            _maxStats.Health += rand.Next(MinHpStep, MaxHpStep);
            _maxStats.Attack += rand.Next(MinStatsStep, MaxStatsStep);
            _maxStats.Defense += rand.Next(MinStatsStep, MaxStatsStep);
            _maxStats.Speed += rand.Next(MinStatsStep, MaxStatsStep);
        }

        RestoreStats();
    }

    /// <inheritdoc cref="IMonster.GetExp"/>
    public int GetExp()
    {
        return _exp;
    }

    /// <inheritdoc cref="IMonster.GetExpCap"/>
    public int GetExpCap()
    {
        return ExpCap;
    }

    /// <inheritdoc cref="IMonster.IsWild"/>
    public bool IsWild()
    {
        return _isWild;
    }

    /// <inheritdoc cref="IMonster.IsAlive"/>
    public bool IsAlive()
    {
        return _stats.Health > 0;
    }


    /// <inheritdoc cref="IMonster.GetMoves"/>
    public IMoves GetMoves(int index)
    {
        if (index < 0 || index > GetNumberOfMoves())
        {
            throw new ArgumentException("Argument Exception");
        }

        return _movesList[index].Item1;
    }

    /// <inheritdoc cref="IMonster.GetAllMoves"/>
    public IList<IMoves> GetAllMoves()
    {
        return _movesList.Select(i => i.Item1).ToImmutableList();
    }

    /// <inheritdoc cref="IMonster.GetCurrentPpByMove"/>
    public int GetCurrentPpByMove(IMoves move)
    {
        //return movesList.stream().filter(i->i.getFirst().equals(move)).findAny().get().getSecond();
        return _movesList.Where(i => i.Item1.Equals(move)).Select(i => i.Item2).First();
    }

    /// <inheritdoc cref="IMonster.IsOutOfPp"/>
    public bool IsOutOfPp(IMoves move)
    {
        //return movesList.stream().filter(i->i.getFirst().equals(move)).findAny().get().getSecond() <= 0;
        return _movesList.Where(i => i.Item1.Equals(move)).Select(i => i.Item2).First() <= 0;
    }

    /// <inheritdoc cref="IMonster.RestoreMovePp"/>
    public void RestoreMovePp(IMoves move)
    {
        var index = GetIndexOfMove(move);
        var p = _movesList[index];
        _movesList.RemoveAt(index);
        _movesList.Insert(index, new Tuple<IMoves, int>(p.Item1, p.Item1.GetPp()));
    }

    /// <inheritdoc cref="IMonster.RestoreAllMovesPp"/>
    public void RestoreAllMovesPp()
    {
        //List<IMoves> moves = _movesList.stream().map(i->i.getFirst()).collect(Collectors.toList());
        /*for (var p : moves) {
            RestoreMovePP(p);
        }*/
        var moves = _movesList.Select(i => i.Item1).ToList();
        foreach (var p in moves)
        {
            RestoreMovePp(p);
        }
    }

    /// <inheritdoc cref="IMonster.DecMovePp"/>
    public void DecMovePp(IMoves move)
    {
        var index = GetIndexOfMove(move);
        var p = _movesList[index];
        _movesList.RemoveAt(index);
        _movesList.Insert(index, new Tuple<IMoves, int>(p.Item1, p.Item2 - 1));
    }

    private int GetIndexOfMove(IMoves move)
    {
        for (var i = 0; i < _movesList.Count; i++)
        {
            if (_movesList[i].Item1.Equals(move))
            {
                return i;
            }
        }

        return -1;
    }

    /// <inheritdoc cref="IMonster.IsMoveSetFull"/>
    public bool IsMoveSetFull()
    {
        return _movesList.Count == NumMaxMoves;
    }

    /// <inheritdoc cref="IMonster.GetNumberOfMoves"/>
    public int GetNumberOfMoves()
    {
        return _movesList.Count;
    }

    /// <inheritdoc cref="IMonster.GetMonsterType"/>
    public MonsterType GetMonsterType()
    {
        return _species.GetMonsterType();
    }

    /// <inheritdoc cref="IMonster.CanEvolveByLevel"/>
    public bool CanEvolveByLevel()
    {
        return _species.GetEvolution().HasValue && _species.GetEvolutionType() == EvolutionType.Level &&
               _level >= ((MonsterSpeciesByLevel)_species).GetEvolutionLevel();
    }

    /// <inheritdoc cref="IMonster.CanEvolveByItem"/>
    public bool CanEvolveByItem(IGameItem item)
    {
        return _species.GetEvolution().HasValue && _species.GetEvolutionType() == EvolutionType.Item
                                                && item.Equals(((MonsterSpeciesByItem)_species).GetItem())
                                                && item.GetGameType() == GameItemTypes.EvolutionTool;
    }

    /// <inheritdoc cref="IMonster.Evolve"/>
    public void Evolve()
    {
        if (_species.GetEvolution().HasValue)
        {
            _species = _species.GetEvolution().ValueOrFailure();
        }
    }

    /// <inheritdoc cref="IMonster.GetSpecies"/>
    public IMonsterSpecies GetSpecies()
    {
        return _species;
    }

    /// <inheritdoc cref="IMonster.GetStats"/>
    public IMonsterStats GetStats()
    {
        return _stats;
    }

    /// <inheritdoc cref="IMonster.GetMaxStats"/>
    public IMonsterStats GetMaxStats()
    {
        return _maxStats;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool Equals(IMonster other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((IMonster)obj);
    }

    public override string ToString()
    {
        return _species + "\nHealth: " + _stats.Health + "\nAttack: " + _stats.Attack
               + "\nDefense: " + _stats.Defense + "\nSpeed: " + _stats.Speed + "\nLevel: "
               + _level + "\nExp: " + _exp + "\nMoves:" + _movesList + "\nIsWild: "
               + _isWild + "\n";
    }
}