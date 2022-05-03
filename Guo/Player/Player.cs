using Guo.Player;
using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina;
using Pokaiju.Castorina.Npc;
using Pokaiju.Castorina.Storage;
using Pokaiju.Guo.GameItem;
using Pokaiju.Pierantoni;

namespace Pokaiju.Guo.Player;

public class Player : IPlayer
{
    private const int StartMoney = 1000;
    private const int TeamSize = 6;
    private const int Step = 1;
    private readonly Dictionary<IGameItem, int> _gameItems;
    private readonly IGameMap _map;
    private Gender _gender;
    private bool _hasMapChanged;
    private int _money;
    private string _name;
    private Tuple<int, int> _position;
    private List<IMonster> _team;
    private readonly int _trainerNumber;
    private Option<IMonsterBattle> _monsterBattle;
    private Option<INpcSimple> _npc;
    private MonsterStorage _storage;
    private bool _triggeredEvent = false;

    public Player(IGameMap map, Gender gender,  string name, Tuple<int, int> position, int trainerNumber, Option<IMonsterBattle> monsterBattle, Option<INpcSimple> npc)
    {
        _gameItems = new Dictionary<IGameItem, int>();
        _map = map;
        _gender = gender;
        _money = StartMoney;
        _name = name;
        _position = position;
        _team = new List<IMonster>();
        _trainerNumber = trainerNumber;
        _monsterBattle = Option.None<IMonsterBattle>();
        _npc = Option.None<INpcSimple>();
        _storage = new MonsterStorage(this);
    }

    public bool IsTeamFull => _team.Count >= TeamSize;

    public bool HasPlayerChangedMap()
    {
        return _hasMapChanged;
    }

    public bool IsTriggeredEvent()
    {
        return _triggeredEvent;
    }

    public List<IMonster> GetAllMonsters() => new(_team);
    
    public Dictionary<IGameItem, int> GetAllItems() => new(_gameItems);
    
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

    public void useItemOnMonster(IGameItem gameItem, IMonster m)
    {
        if (GetAllItems().ContainsKey(gameItem) && gameItem.Use(m)) {
            RemoveItem(gameItem);
        }
    }

    public bool AddMonster(IMonster m)
    {
        if (IsTeamFull) {
            _storage.AddMonster(m);
            return false;
        } else
        {
            _team.Add(m);
            return true;
        }
    }

    public bool RemoveMonster(IMonster m)
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

    public void EvolveMonster(IMonster monster, IGameItem gameItem)
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
        return Move(0, -Step);
    }

    public bool MoveDown()
    {
        return Move(0, Step);
    }

    public bool MoveLeft()
    {
        return Move(-Step,0);
    }

    public bool MoveRight()
    {
        return Move(Step,0);
    }

    public IGameMap GetMap() => _map;

    public bool InteractAt(Tuple<int, int> pos)
    {
        _monsterBattle = Option.None<IMonsterBattle>();
        _npc = _map.GetNpcAt(pos);
        if (_npc.HasValue && _npc.ValueOrFailure().GetTypeOfNpc() == TypeOfNpc.TRAINER)
        {
            var trainer = (INpcTrainer) _npc.ValueOrFailure();
            if (!trainer.IsDefeated())
            {
                _monsterBattle = Option.Some(mons);
            }
        }

        return _npc.HasValue;
    }

    public Option<INpcSimple> GetLastInteractionWithNpc() => _npc;

    public Option<IMonsterBattle> GetPlayerBattle()
    {
        throw new NotImplementedException();
    }

    public void SetStorage(MonsterStorage storage) => _storage = storage;
    

    public MonsterStorage GetStorage() => _storage;


    public void SetGender(Gender gender) => _gender = gender;


    public void SetTrainerNumber(int trainerNumber)
    {
        throw new NotImplementedException();
    }

    public List<IMonster> GetMonster() => _team;


    public void SetMonster(List<IMonster> monster) => _team = monster;


    public List<IGameItem> GetItems() => new (_gameItems.Keys);


    public void SetName(string name) => _name = name;

}