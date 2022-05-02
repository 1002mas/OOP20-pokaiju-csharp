using Guo.Player;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Guo.GameItem;

namespace Pokaiju.Guo.Player;

public class Player : IPlayer
{
    private static readonly int START_MONEY = 1000;
    private const int TeamSize = 6;
    private static readonly int STEP = 1;
    private readonly Dictionary<IGameItem, int> _gameItems;
    private readonly IGameMap _map;
    private readonly Gender _gender;
    private bool _hasMapChanged;
    private int _money;
    private readonly string _name;
    private Tuple<int, int> _position;
    private readonly List<Monster> _team;
    private readonly int _trainerNumber;
    private Optional<MonsterBattle> monsterBattle;
    private Optional<NpcSimple> npc;
    private MonsterStorage storage = new MonsterStorageImpl();

    public Player(Dictionary<IGameItem, int> gameItems, IGameMap map, Gender gender, bool hasMapChanged, int money, string name, Tuple<int, int> position, List<Monster> team, int trainerNumber, Optional<MonsterBattle> monsterBattle, Optional<NpcSimple> npc, bool isTriggeredEvent)
    {
        _gameItems = gameItems;
        _map = map;
        _gender = gender;
        _hasMapChanged = hasMapChanged;
        _money = money;
        _name = name;
        _position = position;
        _team = team;
        _trainerNumber = trainerNumber;
        this.monsterBattle = monsterBattle;
        this.npc = npc;
        IsTriggeredEvent = isTriggeredEvent;
    }

    public bool IsTeamFull => _team.Count >= TeamSize;

    public bool HasPlayerChangedMap()
    {
        return _hasMapChanged;
    }
    
    public bool IsTriggeredEvent { get; }

    public List<Monster> GetAllMonsters() => new List<Monster>(_team);
    
    public Dictionary<IGameItem, int> GetAllItems() => new Dictionary<IGameItem, int>(_gameItems);
    
    public string GetName() => _name;
    
    public int GetTrainerNumber() => _trainerNumber;

    public Gender GetGender() => _gender;

    public int GetMoney() => _money;

    public Tuple<int, int> GetPosition() => _position;
    
    public void AddItem(IGameItem i)
    {
        if (!_gameItems.ContainsKey(i))
        {
            _gameItems.Add(i,0);
        }
        _gameItems.Add(i,_gameItems[i]+1);
    }

    public void AddItem(IGameItem i, int quantity)
    {
        if (quantity > 0) {
            _gameItems.Add(i, quantity);
        }
    }

    public int GetItemQuantity(IGameItem gameItem) => _gameItems[gameItem];
  

    public void RemoveItem(IGameItem gameItem)
    {
        if (!_gameItems.ContainsKey(gameItem)) return;
        _gameItems.Add(gameItem, _gameItems[gameItem] - 1);
        if (_gameItems[gameItem] < 1) {
            _gameItems.Remove(gameItem);
        }
    }

    public void UseItem(IGameItem gameItem)
    {
        if (GetAllItems().ContainsKey(gameItem) && gameItem.Use(null)) {
            RemoveItem(gameItem);
        }
    }

    public void useItemOnMonster(IGameItem gameItem, Monster m)
    {
        if (GetAllItems().ContainsKey(gameItem) && gameItem.Use(m)) {
            RemoveItem(gameItem);
        }
    }

    public bool AddMonster(Monster m)
    {
        if (IsTeamFull) {
            storage.addMonster(m);
            return false;
        } else
        {
            _team.Add(m);
            return true;
        }
    }

    public bool RemoveMonster(Monster m)
    {
        return GetAllMonsters().Contains(m) && _team.Remove(m);
    }

    public void SetMoney(int money)
    {
        _money = money;
    }

    public void SetPosition(Tuple<int, int> position)
    {
        _position = position;
    }

    public void EvolveMonsters()
    {
        foreach (var m in _team.Where(m => m.CanEvolveByLevel()))
        {
            m.Evolve();
        }
    }

    public void EvolveMonster(Monster monster, IGameItem gameItem)
    {
        if (!monster.CanEvolveByItem(gameItem)) return;
        useItemOnMonster(gameItem, monster);
        monster.Evolve();
    }
    private bool Move( int x,  int y)
    {
        _hasMapChanged = false;
        return false;
    }

    public bool MoveUp()
    {
        throw new NotImplementedException();
    }

    public bool MoveDown()
    {
        throw new NotImplementedException();
    }

    public bool MoveLeft()
    {
        throw new NotImplementedException();
    }

    public bool MoveRight()
    {
        throw new NotImplementedException();
    }

    public IGameMap GetMap()
    {
        throw new NotImplementedException();
    }

    public bool InteractAt(Tuple<int, int> pos)
    {
        throw new NotImplementedException();
    }

    public Optional<NpcSimple> GetLastInteractionWithNpc()
    {
        throw new NotImplementedException();
    }

    public Optional<MonsterBattle> GetPlayerBattle()
    {
        throw new NotImplementedException();
    }

    public void SetStorage(MonsterStorage storage)
    {
        throw new NotImplementedException();
    }

    public MonsterStorage GetStorage()
    {
        throw new NotImplementedException();
    }

    public void SetGender(Gender gender)
    {
        throw new NotImplementedException();
    }

    public void SetTrainerNumber(int trainerNumber)
    {
        throw new NotImplementedException();
    }

    public List<Monster> GetMonster()
    {
        throw new NotImplementedException();
    }

    public void SetMonster(List<Monster> monster)
    {
        throw new NotImplementedException();
    }

    public List<IGameItem> GetItems()
    {
        throw new NotImplementedException();
    }

    public void SetName(string name)
    {
        throw new NotImplementedException();
    }
}