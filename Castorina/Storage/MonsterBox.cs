using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Storage;

public class MonsterBox : IMonsterBox
{

    private readonly string _name;
    private readonly int _boxSize;
    private readonly IList<IMonster> _monsterList;

    /// <summary>
    /// Constructor for MonsterBox.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="boxSize"></param>
    public MonsterBox(string name, int boxSize)
    {
        _name = name;
        _boxSize = boxSize;
        _monsterList = new List<IMonster>();
    }

    /// <summary>
    /// Constructor for MonsterBox.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="monsters"></param>
    /// <param name="boxSize"></param>
    public MonsterBox(string name, IList<IMonster> monsters, int boxSize) : this(name, boxSize)
    {
        foreach (var monster in monsters)
        {
            AddMonster(monster);
        }
    }

    /// <inheritdoc cref="IMonsterBox.GetAllMonsters"/>
    public IList<IMonster> GetAllMonsters()
    {
        return _monsterList.ToList();
    }

    /// <inheritdoc cref="IMonsterBox.AddMonster"/>
    public bool AddMonster(IMonster monster)
    {
        if (IsFull()) return false;
        _monsterList.Add(monster);
        return true;

    }

    /// <inheritdoc cref="IMonsterBox.GetMonster"/>
    public Option<IMonster> GetMonster(int monsterId)
    {
        foreach (var monster in _monsterList)
        {
            if (monster.Id == monsterId)
            {
                return Option.Some(monster);
            }
        }

        return Option.None<IMonster>();
    }

    /// <inheritdoc cref="IMonsterBox.IsFull"/>
    public bool IsFull()
    {
        return _monsterList.Count >= _boxSize;
    }

    /// <inheritdoc cref="IMonsterBox.Exchange"/>
    public Option<IMonster> Exchange(IMonster toBox, int monsterId)
    {
        var monsterInBox = GetMonster(monsterId);
        if (!monsterInBox.HasValue) return monsterInBox;
        _monsterList.Remove(monsterInBox.ValueOrFailure());
        AddMonster(toBox);

        return monsterInBox;
    }

    /// <inheritdoc cref="IMonsterBox.GetName"/>
    public string GetName()
    {
        return _name;
    }

    /// <inheritdoc cref="IMonsterBox.RemoveMonster"/>
    public void RemoveMonster(int monsterId)
    {
        var monsterInBox = GetMonster(monsterId);
        if (monsterInBox.HasValue)
        {
            _monsterList.Remove(monsterInBox.ValueOrFailure());
        }

    }
}

