namespace Pokaiju.Barattini;

public class MonsterStats : IMonsterStats
{
    private const string HealthString = "HEALTH";
    private const string AttackString = "ATTACK";
    private const string DefenseString = "DEFENSE";
    private const string SpeedString = "SPEED";
    private readonly IDictionary<string, int> _statsMap = new Dictionary<string, int>();

    /// <summary>
    /// MonsterStats constructor
    /// </summary>
    /// <param name="health">health</param>
    /// <param name="attack">attack</param>
    /// <param name="defense">defense</param>
    /// <param name="speed">speed</param>
    public MonsterStats(int health, int attack, int defense, int speed)
    {
        _statsMap.Add(HealthString, health);
        _statsMap.Add(AttackString, attack);
        _statsMap.Add(DefenseString, defense);
        _statsMap.Add(SpeedString, speed);
    }

    /// <inheritdoc cref="IMonsterStats.Health"/>
    public int Health
    {
        get => _statsMap[HealthString];
        set => _statsMap[HealthString] = value;
    }

    /// <inheritdoc cref="IMonsterStats.Attack"/>
    public int Attack
    {
        get => _statsMap[AttackString];
        set => _statsMap[AttackString] = value;
    }

    /// <inheritdoc cref="IMonsterStats.Defense"/>
    public int Defense
    {
        get => _statsMap[DefenseString];
        set => _statsMap[DefenseString] = value;
    }

    /// <inheritdoc cref="IMonsterStats.Speed"/>
    public int Speed
    {
        get => _statsMap[SpeedString];
        set => _statsMap[SpeedString] = value;
    }

    /// <inheritdoc cref="IMonsterStats.GetStatsAsMap"/>
    public IDictionary<string, int> GetStatsAsMap()
    {
        return _statsMap;
    }

    public override string ToString()
    {
        return "stats: [health=" + _statsMap[HealthString] + " attack=" + _statsMap[AttackString] + " defense=" +
               _statsMap[DefenseString] + " speed=" + _statsMap[SpeedString] + "]";
    }
}