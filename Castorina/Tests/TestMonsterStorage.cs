using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina.Storage;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Castorina.Tests;
using NUnit.Framework;

[TestFixture]

public class TestMonsterStorage
{
    private const int GenericValue = 3;
    private const int MaxBoxInStorage = 10;
    private const int MaxBoxSize = 10;
    private const int Rows = 21;
    private const int Columns = 21;

    private IMonsterStorage? _monsterStorage;
    private IMonsterBox? _monsterBox;
    private IMonster? _monster1;
    private IMonster? _monster2;
    private IMonster? _monster3;
    private IMonster? _monster4;
    private IMonster? _monster5;
    private IMonster? _monster6;
    private IMonster? _monster7;
    private IPlayer? _player;

    private void AddMonsterList(IMonsterStorage storage, int n, IMonster monster)
    {
        for (var i = 0; i < n; i++)
        {
            storage.AddMonster(monster);
        }
    }

    [SetUp]
    public void InitFactory()
    {
        var position = new Tuple<int, int>(1, 1);
        IDictionary<Tuple<int, int>, MapBlockType> map = new Dictionary<Tuple<int, int>, MapBlockType>();
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                map.Add(new Tuple<int, int>(r, c), MapBlockType.Walk);
            }
        }

        IGameMapData firstMap = new GameMapData(1, "MAP1", 1, 10, map, new List<IMonsterSpecies>());


        _player = new Player("player", Gender.Woman, 0, position, new GameMap(firstMap));
        // monster
        IMoves m1 = new Moves("move1", GenericValue, MonsterType.Grass, GenericValue);
        IMoves m2 = new Moves("move2", GenericValue, MonsterType.Fire, GenericValue);
        IList<Tuple<IMoves, int>> listOfMoves = new List<Tuple<IMoves, int>>();
        listOfMoves.Add(new Tuple<IMoves, int>(m1, GenericValue));
        IList<IMoves> listMoves = new List<IMoves>();
        listMoves.Add(m1);
        listMoves.Add(m2);
        var species = new MonsterSpeciesBuilder().Name("nome1").Info("Info1")
            .MonsterType(MonsterType.Fire).MovesList(listMoves).Build();

        _monster1 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster2 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster3 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster4 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster5 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster6 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster7 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();
        _monster1 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();

        _player.AddMonster(_monster2);
        _monsterStorage = new MonsterStorage(_player);
        _monsterBox = new MonsterBox("Box1", _monsterStorage.GetMaxSizeOfBox());

    }

    [Test]
    public void MonsterBox()
    {
        if (_monster1 == null || _monster2 == null) return;
        _monsterBox?.AddMonster(_monster1);
        Assert.AreEqual(_monster1, _monsterBox?.GetAllMonsters().ElementAt(0));
        Assert.AreEqual(Option.Some(_monster1), _monsterBox?.GetMonster(_monster1.Id));

        _monsterBox?.Exchange(_monster2, _monster1.Id);
        Assert.AreEqual(1, _monsterBox?.GetAllMonsters().Count);
        Assert.AreEqual(Option.Some(_monster2), _monsterBox?.GetMonster(_monster2.Id));

        _monsterBox?.RemoveMonster(_monster2.Id);
        Assert.False(_monsterBox?.GetAllMonsters().Contains(_monster2));
        Assert.AreEqual(Option.None<IMonster>(), _monsterBox?.GetMonster(_monster2.Id));
        Assert.AreEqual(0, _monsterBox?.GetAllMonsters().Count);


    }

    [Test]
    public void MonsterStorage()
    {
        if (_monster1 == null || _monster2 == null || _monster3 == null || _monster4 == null || _monster5 == null ||
            _monster6 == null || _monster7 == null || _player == null) return;
        // add monster
        Assert.AreEqual("BOX1", _monsterStorage?.GetCurrentBoxName());
        Assert.True(_monsterStorage?.AddMonster(_monster1));
        Assert.True(_monsterStorage?.GetCurrentBoxMonsters().Contains(_monster1));
        Assert.AreEqual("BOX1", _monsterStorage?.GetCurrentBoxName());
        // previous box

        _monsterStorage?.PreviousBox();
        Assert.AreEqual("BOX10", _monsterStorage?.GetCurrentBoxName());
        // next box
        _monsterStorage?.NextBox();
        Assert.AreEqual("BOX1", _monsterStorage?.GetCurrentBoxName());

        if (_monsterStorage == null) return;
        AddMonsterList(_monsterStorage, _monsterStorage.GetMaxSizeOfBox(), _monster1);
        _monsterStorage.NextBox();

        Assert.True(_monsterStorage.GetCurrentBoxMonsters().Contains(_monster1));
        Assert.AreEqual(1, _monsterStorage.GetCurrentBoxMonsters().Count);
        // exchange player _monster2 with box _monster1
        Assert.True(_monsterStorage.Exchange(_monster2, _monster1.Id));
        Assert.True(_monsterStorage.GetCurrentBoxMonsters().Contains(_monster2));
        Assert.True(_player?.GetAllMonsters().Contains(_monster1));

        _player?.AddMonster(_monster3); // player team: _monster3, _monster1
        if (_player == null) return;
        Assert.True(_monsterStorage.DepositMonster(_monster3));
        Assert.False(_player.GetAllMonsters().Contains(_monster3));
        Assert.True(_monsterStorage.GetCurrentBoxMonsters().Contains(_monster3));

        // player team: _monster1
        // withDrawMonster
        Assert.True(_monsterStorage.WithdrawMonster(_monster3.Id));
        Assert.False(_monsterStorage.WithdrawMonster(_monster3.Id));

        // player team: _monster1, _monster3
        _player.AddMonster(_monster4);
        _player.AddMonster(_monster5);
        _player.AddMonster(_monster6);
        _player.AddMonster(_monster7);
        // player team full

        // withDrawMonster
        Assert.False(_monsterStorage.WithdrawMonster(_monster2.Id));

    }

    [Test]
    public void MonsterStorageWithList()
    {
        if (_player == null) return;
        IMonsterBox box2 = new MonsterBox("NEWBOX1", MaxBoxSize);
        IList<IMonsterBox> boxList = new List<IMonsterBox>();
        boxList.Add(box2);
        IMonsterStorage monsterStorage2 = new MonsterStorage(_player, boxList);
        Assert.AreEqual("NEWBOX1", monsterStorage2.GetCurrentBoxName());
    }

    [Test]
    public void MonsterStorageWithList2()
    {
        if (_player == null) return;
        IList<IMonsterBox> boxList = new List<IMonsterBox>();
        for (var i = 0; i < MaxBoxInStorage + 1; i++)
        {
            boxList.Add(new MonsterBox("BOX" + (i + 1), MaxBoxSize));
        }

        IMonsterStorage monsterStorage3 = new MonsterStorage(_player, boxList);
        monsterStorage3.PreviousBox();
        Assert.AreEqual("BOX10", monsterStorage3.GetCurrentBoxName());

    }

    [Test]
    public void FullMonsterStorage()
    {
        if (_player == null || _monster1 == null || _monster2 == null || _monster3 == null) return;
        IMonsterStorage monsterStorage4 = new MonsterStorage(_player);
        for (var i = 0; i < MaxBoxSize; i++)
        {
            AddMonsterList(monsterStorage4, 10, _monster1);
            monsterStorage4.NextBox();
        }

        Assert.False(monsterStorage4.AddMonster(_monster2));
        Assert.True(monsterStorage4.WithdrawMonster(_monster1.Id));
        Assert.True(monsterStorage4.DepositMonster(_monster1));
        Assert.False(monsterStorage4.AddMonster(_monster3));


    }
}    


