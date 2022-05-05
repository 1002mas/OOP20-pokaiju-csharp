using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Storage;

public class MonsterStorage : IMonsterStorage
{
    /// <summary>
    /// Max number of monster box
    /// </summary>
    private const int MaxNumberOfBox = 10;

    /// <summary>
    /// max size of a monster box
    /// </summary>
    private const int MaxSizeOfBox = 10;

    /// <summary>
    /// initial box name
    /// </summary>
    private const string InitialBoxName = "BOX";

    private readonly IList<IMonsterBox> _monsterBoxes;
    private int _currentMonsterBoxIndex;
    private readonly IPlayer _player;

    /// <summary>
    /// Constructor of MonsterStorage
    /// </summary>
    /// <param name="player"></param>
    /// <param name="boxes"></param>
    public MonsterStorage(IPlayer player, IList<IMonsterBox> boxes)
    {
        _player = player;
        _monsterBoxes = new List<IMonsterBox>(boxes);
        _currentMonsterBoxIndex = 0;
        if (_monsterBoxes.Count > MaxNumberOfBox)
        {
            var temp = new List<IMonsterBox>(_monsterBoxes);
            _monsterBoxes = temp.GetRange(0, MaxNumberOfBox);
        }
        else
        {
            GenerateBoxs(_monsterBoxes.Count);
        }
    }

    /// <summary>
    /// Constructor of MonsterStorage 
    /// </summary>
    /// <param name="player"></param>
    /// <exception cref="NotImplementedException"></exception>
    public MonsterStorage(IPlayer player) : this(player, new List<IMonsterBox>())
    {

    }


    private void GenerateBoxs(int n)
    {
        for (var i = n; i < MaxNumberOfBox; i++)
        {
            IMonsterBox box = new MonsterBox(InitialBoxName + (i + 1), MaxSizeOfBox);
            _monsterBoxes.Add(box);
        }
    }

    private void CalculateNewIndex(int n)
    {
        _currentMonsterBoxIndex = ((_currentMonsterBoxIndex + n) % MaxSizeOfBox +
                                        MaxSizeOfBox) % MaxSizeOfBox;
    }

    private IMonsterBox? GetFirstBoxFree()
    {
        for (var i = 0; i < MaxNumberOfBox; i++)
        {
            if (!_monsterBoxes.ElementAt(i).IsFull())
            {
                return _monsterBoxes.ElementAt(i);
            }

        }

        return null;
    }

    /// <inheritdoc cref="IMonsterStorage.AddMonster"/>
    public bool AddMonster(IMonster monster)
    {
        if (GetCurrentBox().AddMonster(monster))
        {
            return true;
        }

        var monsterBox = GetFirstBoxFree();
        return monsterBox != null && monsterBox.AddMonster(monster);
    }

    /// <inheritdoc cref="IMonsterStorage.DepositMonster"/>
    public bool DepositMonster(IMonster monster)
    {
        if (_player.GetAllMonsters().Count <= 1 || !_player.GetAllMonsters().Contains(monster) ||
            !GetCurrentBox().AddMonster(monster)) return false;
        _player.RemoveMonster(monster);
        return true;

    }

    /// <inheritdoc cref="IMonsterStorage.WithdrawMonster"/>
    public bool WithdrawMonster(int monsterId)
    {
        if (!IsInBox(monsterId) || _player.IsTeamFull) return false;
        _player.AddMonster(GetCurrentBox().GetMonster(monsterId).ValueOrFailure());
        GetCurrentBox().RemoveMonster(monsterId);
        return true;

    }

    private bool IsInBox(int monsterId)
    {
        return GetCurrentBox().GetAllMonsters().Any(monster => monster.Id == monsterId);
    }

    private IMonsterBox GetCurrentBox()
    {
        return _monsterBoxes.ElementAt(_currentMonsterBoxIndex);
    }

    /// <inheritdoc cref="IMonsterStorage.Exchange"/>
    public bool Exchange(IMonster monster, int monsterId)
    {
        if (!IsInBox(monsterId)) return false;
        var m = GetCurrentBox().Exchange(monster, monsterId);
        if (!m.HasValue) return false;
        _player.AddMonster(m.ValueOrFailure());
        _player.RemoveMonster(monster);
        return true;

    }

    /// <inheritdoc cref="IMonsterStorage.NextBox"/>
    public void NextBox()
    {
        CalculateNewIndex(1);
    }

    /// <inheritdoc cref="IMonsterStorage.PreviousBox"/>
    public void PreviousBox()
    {
        CalculateNewIndex(-1);
    }

    /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxName"/>
    public string GetCurrentBoxName()
    {
        return GetCurrentBox().GetName();
    }

    /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxMonsters"/>
    public IList<IMonster> GetCurrentBoxMonsters()
    {
        return GetCurrentBox().GetAllMonsters();
    }

    /// <inheritdoc cref="IMonsterStorage.GetMaxSizeOfBox"/>
    public int GetMaxSizeOfBox()
    {
        return MaxSizeOfBox;
    }

    /// <inheritdoc cref="IMonsterStorage.GetMaxNumberOfBox"/>
    public int GetMaxNumberOfBox()
    {
        return MaxNumberOfBox;
    }

    /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxSize"/>
    public int GetCurrentBoxSize()
    {
        return GetCurrentBoxMonsters().Count;
    }
}

