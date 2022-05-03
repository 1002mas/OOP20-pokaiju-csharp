using NUnit.Framework;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Guo;

public class TestPlayer
{
    private const int DefaultStats = 50;
    private const int Pp1 = 20;
    private const int Pp2 = 10;
    private const int StartExp = 0;
    private const int StartLevel = 1;
    private IGameItem _healingPotion;
    private IMonster _monsterA;
    private IMonster _monsterB;
    private IGameItem _monsterBall;
    private IMonster _monsterC;
    private IPlayer _player;

    [SetUp]
    public void SetUp()
    {
        const int trainerNumber = 100_001;
        IDictionary<Tuple<int, int>, MapBlockType> map = new Dictionary<Tuple<int, int>, MapBlockType>();
        const int rows = 21;
        const int columns = 21;
        for (var r = 0; r < rows; r++)
        for (var c = 0; c < columns; c++)
            map.Add(new Tuple<int, int>(r, c), MapBlockType.Walk);

        IGameMapData firstMap = new GameMapData(1, "MAP1", 1, 10, map, new List<IMonsterSpecies>());
        _player = new Player.Player("Joker", Gender.Man, trainerNumber, new Tuple<int, int>(1, 0),
            new GameMap(firstMap));

        IMoves m1 = new Moves("Water tornado", DefaultStats, MonsterType.Water, Pp2);
        IMoves m2 = new Moves("Flame punch", Pp2, MonsterType.Fire, Pp1);
        IList<IMoves> firstMonsterSpeciesMoves = new List<IMoves> {m1};
        IList<IMoves> secondMonsterSpeciesMoves = new List<IMoves> {m2};
        IList<Tuple<IMoves, int>> firstListOfMoves = new List<Tuple<IMoves, int>> {new(m1, Pp2)};
        IList<Tuple<IMoves, int>> secondListOfMoves = new List<Tuple<IMoves, int>> {new(m2, Pp1)};
        _monsterBall = new CaptureItem("Monster Ball", "Can capture a monster");
        _healingPotion = new HealingItem("Healing potion", "Restore 50Hp");

        var species = new MonsterSpeciesBuilder().Name("bibol").Info("Info1").MonsterType(MonsterType.Fire)
            .Health(DefaultStats).Attack(Pp2).Defense(Pp2).Speed(Pp2).MovesList(firstMonsterSpeciesMoves).Build();

        _monsterA = new MonsterBuilder().Health(DefaultStats).Attack(DefaultStats).Defense(DefaultStats)
            .Speed(DefaultStats).Exp(StartExp).Level(StartLevel).Wild(false).Species(species)
            .MovesList(firstListOfMoves).Build();

        species = new MonsterSpeciesBuilder().Name("greyfish").Info("Info2").MonsterType(MonsterType.Water)
            .Health(DefaultStats).Attack(10).Defense(10).Speed(10).MovesList(secondMonsterSpeciesMoves).Build();

        _monsterB = new MonsterBuilder().Health(DefaultStats).Attack(DefaultStats).Defense(DefaultStats)
            .Speed(DefaultStats).Exp(StartExp).Level(StartLevel).Wild(false).Species(species)
            .MovesList(secondListOfMoves).Build();

        species = new MonsterSpeciesBuilder().Name("Pipochu").Info("Info3").MonsterType(MonsterType.Grass)
            .Health(DefaultStats).Attack(10).Defense(10).Speed(10).MovesList(secondMonsterSpeciesMoves).Build();

        _monsterC = new MonsterBuilder().Health(DefaultStats).Attack(DefaultStats).Defense(DefaultStats)
            .Speed(DefaultStats).Exp(StartExp).Level(StartLevel).Wild(false).Species(species)
            .MovesList(secondListOfMoves).Build();
    }

    [Test]
    public void MonsterListCheck()
    {
        _player.AddMonster(_monsterA);
        _player.AddMonster(_monsterB);
        Assert.AreEqual(2, _player?.GetAllMonsters().Count);
        Assert.False(_player.GetAllMonsters().Contains(_monsterC));
        Assert.Equals("bibol", _player.GetAllMonsters()[0].GetName());
        Assert.AreNotEqual("bibol", _player.GetAllMonsters()[1].GetName());
        Assert.True(_player.RemoveMonster(_monsterB));
        Assert.False(_player.RemoveMonster(_monsterC));
    }

    [Test]
    public void ItemListCheck()
    {
        _player.AddItem(_monsterBall);
        Assert.True(_player.GetAllItems().Keys.Any(item => item.Equals(_monsterBall)),
            "Monster Ball doesn't exist in bag");
        Assert.False(_player.GetAllItems().ContainsKey(_healingPotion));
        _player.AddItem(_healingPotion);
        Assert.True(_player.GetAllItems().ContainsKey(_healingPotion));
        _player.UseItem(_monsterBall);
        Assert.False(_player.GetAllItems().ContainsKey(_monsterBall));
        _player.AddItem(_healingPotion);
        Assert.AreEqual(2, _player.GetAllItems()[_healingPotion]);
    }

    [Test]
    public void UseItemCheck()
    {
        _player.AddMonster(_monsterA);
        _player.AddItem(_healingPotion);
        _player.GetAllMonsters()[0].SetHealth(0);
        Assert.AreEqual(0, _player.GetAllMonsters()[0].GetStats().Health);
        _player.useItemOnMonster(_healingPotion, _monsterA);
        Assert.AreEqual(_player.GetAllMonsters()[0].GetMaxHealth(),
            _player.GetAllMonsters()[0].GetStats().Health);
        _player.AddItem(_healingPotion);
        _player.useItemOnMonster(_healingPotion, _monsterA);
        Assert.True(_player.GetAllItems().ContainsKey(_healingPotion));
    }

    [Test]
    public void PlayerPositionCheck()
    {
        var limitPosition = new Tuple<int, int>(20, 20);
        Assert.AreEqual(new Tuple<int, int>(1, 0), _player.GetPosition());
        _player.MoveUp();
        Assert.AreNotEqual(new Tuple<int, int>(1, -1), _player.GetPosition());
        _player.MoveRight();
        Assert.AreEqual(new Tuple<int, int>(2, 0), _player.GetPosition());
        _player.MoveDown();
        Assert.AreEqual(new Tuple<int, int>(2, 1), _player.GetPosition());
        _player.MoveLeft();
        Assert.AreEqual(new Tuple<int, int>(1, 1), _player.GetPosition());
        Assert.AreNotEqual(new Tuple<int, int>(10, 10), _player.GetPosition());
        _player.SetPosition(limitPosition);
        Assert.AreEqual(limitPosition, _player.GetPosition());
        Assert.False(_player.MoveDown());
        Assert.False(_player.MoveRight());
        Assert.True(_player.MoveUp());
        Assert.True(_player.MoveLeft());
    }
}