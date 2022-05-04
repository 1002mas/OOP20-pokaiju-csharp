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
    private int _trainerNumber;
    private Option<IMonsterBattle> _monsterBattle;
    private Option<INpcSimple> _npc;
    private MonsterStorage _storage;
    private bool _triggeredEvent;

    public Player(string name, Gender gender, int trainerNumber, Tuple<int, int> position, IGameMap map )
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
    
    /// <inheritdoc cref="IPlayer.IsTeamFull"/>
    public bool IsTeamFull => _team.Count >= TeamSize;

    /// <inheritdoc cref="IPlayer.HasPlayerChangedMap"/>
    public bool HasPlayerChangedMap()
    {
        return _hasMapChanged;
    }
    /// <inheritdoc cref="IPlayer.IsTriggeredEvent"/>
    public bool IsTriggeredEvent()
    {
        return _triggeredEvent;
    }
    /// <inheritdoc cref="IPlayer.GetAllMonsters"/>
    public List<IMonster> GetAllMonsters() => new(_team);
    
    /// <inheritdoc cref="IPlayer.GetAllItems"/>
    public Dictionary<IGameItem, int> GetAllItems() => new(_gameItems);
    
    /// <inheritdoc cref="IPlayer.GetName"/>
    public string GetName() => _name;
    
    /// <inheritdoc cref="IPlayer.GetTrainerNumber"/>
    public int GetTrainerNumber() => _trainerNumber;

    /// <inheritdoc cref="IPlayer.GetGender"/>
    public Gender GetGender() => _gender;

    /// <inheritdoc cref="IPlayer.GetMoney"/>
    public int GetMoney() => _money;

    /// <inheritdoc cref="IPlayer.GetPosition"/>
    public Tuple<int, int> GetPosition() => _position;
    
    /// <inheritdoc cref="IPlayer.AddItem(Pokaiju.Guo.GameItem.IGameItem)"/>
    public void AddItem(IGameItem i)
    {
        if (!_gameItems.ContainsKey(i))
        {
            _gameItems.Add(i,0);
        }
        _gameItems.Add(i,_gameItems[i]+1);
    }

    /// <inheritdoc cref="IPlayer.AddItem(Pokaiju.Guo.GameItem.IGameItem)"/>
    public void AddItem(IGameItem i, int quantity)
    {
        if (quantity > 0) {
            _gameItems.Add(i, quantity);
        }
    }

    /// <inheritdoc cref="IPlayer.GetItemQuantity"/>
    public int GetItemQuantity(IGameItem gameItem) => _gameItems[gameItem];
  
    /// <inheritdoc cref="IPlayer.RemoveItem"/>
    public void RemoveItem(IGameItem gameItem)
    {
        if (!_gameItems.ContainsKey(gameItem)) return;
        _gameItems.Add(gameItem, _gameItems[gameItem] - 1);
        if (_gameItems[gameItem] < 1) {
            _gameItems.Remove(gameItem);
        }
    }

    /// <inheritdoc cref="IPlayer.UseItem"/>
    public void UseItem(IGameItem gameItem)
    {
        if (GetAllItems().ContainsKey(gameItem) && gameItem.Use(null)) {
            RemoveItem(gameItem);
        }
    }

    /// <inheritdoc cref="IPlayer.useItemOnMonster"/>
    public void useItemOnMonster(IGameItem gameItem, IMonster m)
    {
        if (GetAllItems().ContainsKey(gameItem) && gameItem.Use(m)) {
            RemoveItem(gameItem);
        }
    }

    /// <inheritdoc cref="IPlayer.AddMonster"/>
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

    /// <inheritdoc cref="IPlayer.RemoveMonster"/>
    public bool RemoveMonster(IMonster m)
    {
        return GetAllMonsters().Contains(m) && _team.Remove(m);
    }

    /// <inheritdoc cref="IPlayer.SetMoney"/>
    public void SetMoney(int money)
    {
        _money = money;
    }

    /// <inheritdoc cref="IPlayer.SetPosition"/>
    public void SetPosition(Tuple<int, int> position)
    {
        _position = position;
    }

    /// <inheritdoc cref="IPlayer.EvolveMonsters"/>
    public void EvolveMonsters()
    {
        foreach (var m in _team.Where(m => m.CanEvolveByLevel()))
        {
            m.Evolve();
        }
    }
    
    /// <inheritdoc cref="IPlayer.EvolveMonster"/>
    public void EvolveMonster(IMonster monster, IGameItem gameItem)
    {
        if (!monster.CanEvolveByItem(gameItem)) return;
        useItemOnMonster(gameItem, monster);
        monster.Evolve();
    }
    
     ///<summary>
     /// move operation.
     ///<param name="x">New x coordinate</param>
     ///<param name="y">New y coordinate</param>
     /// <returns>True if Player has moved</returns>
     ///</summary>
    private bool Move( int x,  int y)
    {
        _hasMapChanged = false;
        _monsterBattle = Option.None<IMonsterBattle>();
        var nextPosition = new Tuple<int, int>(_position.Item1 + x, _position.Item2 + y);
        var canMove = _map.CanPassThrough(nextPosition);

        if (canMove) {
            _position = nextPosition; 
            var gameEvent = _map.GetEventAt(_position);
            if (gameEvent.HasValue && gameEvent.ValueOrFailure().IsBattle()) {
                _monsterBattle = Option.Some<IMonsterBattle>(new MonsterBattle(this, gameEvent.ValueOrFailure().GetMonster()[0]));
                gameEvent.ValueOrFailure().Activate();
            }
            if (_map.CanChangeMap(nextPosition)) {
                _hasMapChanged = true;
                _map.ChangeMap(nextPosition);
                _position = _map.GetPlayerMapPosition().ValueOrFailure();
            }
        }
        var monster = _map.GetWildMonster(_position);
        if (monster.HasValue) {
            _monsterBattle = Option.Some<IMonsterBattle>(new MonsterBattle(this, monster.ValueOrFailure()));
        }
        _triggeredEvent = _map.GetEventAt(_position).HasValue;
        return canMove;
    }

    /// <inheritdoc cref="IPlayer.MoveUp"/>
    public bool MoveUp()
    {
        return Move(0, -Step);
    }

    /// <inheritdoc cref="IPlayer.MoveDown"/>
    public bool MoveDown()
    {
        return Move(0, Step);
    }

    /// <inheritdoc cref="IPlayer.MoveLeft"/>
    public bool MoveLeft()
    {
        return Move(-Step,0);
    }

    /// <inheritdoc cref="IPlayer.MoveRight"/>
    public bool MoveRight()
    {
        return Move(Step,0);
    }

    /// <inheritdoc cref="IPlayer.GetMap"/>
    public IGameMap GetMap() => _map;

    /// <inheritdoc cref="IPlayer.InteractAt"/>
    public bool InteractAt(Tuple<int, int> pos)
    {
        _monsterBattle = Option.None<IMonsterBattle>();
        _npc = _map.GetNpcAt(pos);
        if (_npc.HasValue && _npc.ValueOrFailure().GetTypeOfNpc() == TypeOfNpc.TRAINER)
        {
            var trainer = (INpcTrainer) _npc.ValueOrFailure();
            if (!trainer.IsDefeated())
            {
                _monsterBattle = Option.Some<IMonsterBattle>(new MonsterBattle(this,trainer));
            }
        }
        return _npc.HasValue;
    }

    /// <inheritdoc cref="IPlayer.GetLastInteractionWithNpc"/>
    public Option<INpcSimple> GetLastInteractionWithNpc() => _npc;

    /// <inheritdoc cref="IPlayer.GetPlayerBattle"/>
    public Option<IMonsterBattle> GetPlayerBattle() => _monsterBattle;

    /// <inheritdoc cref="IPlayer.SetStorage"/>
    public void SetStorage(MonsterStorage storage) => _storage = storage;
    
    /// <inheritdoc cref="IPlayer.GetStorage"/>
    public MonsterStorage GetStorage() => _storage;

    /// <inheritdoc cref="IPlayer.SetGender"/>
    public void SetGender(Gender gender) => _gender = gender;

    /// <inheritdoc cref="IPlayer.SetTrainerNumber"/>
    public void SetTrainerNumber(int trainerNumber) => _trainerNumber = trainerNumber;

    /// <inheritdoc cref="IPlayer.GetMonster"/>
    public List<IMonster> GetMonster() => _team;

    /// <inheritdoc cref="IPlayer.SetMonster"/>
    public void SetMonster(List<IMonster> monster) => _team = monster;

    /// <inheritdoc cref="IPlayer.GetItems"/>
    public List<IGameItem> GetItems() => new (_gameItems.Keys);

    /// <inheritdoc cref="IPlayer.SetName"/>
    public void SetName(string name) => _name = name;

}