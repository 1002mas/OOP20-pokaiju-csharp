using NUnit.Framework;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Guo;

public class TestPlayer
{
    private IPlayer _player;
    private IMonster _monsterA;
    private IMonster _monsterB;
    private IMonster _monsterC;
    private IGameItem _monsterBall;
    private IGameItem _healingPotion;
    private const int DefaultStats = 50;
    private const int Pp1 = 20;
    private const int Pp2 = 10;
    private const int StartExp = 0;
    private const int StartLevel = 1;

    [SetUp]
    public void SetUp() {
        var trainerNumber = 100_001;
        IMonsterSpecies species;
        IList<IMoves> firstMonsterSpeciesMoves;
        IList<IMoves> secondMonsterSpeciesMoves;
        IList<Tuple<IMoves, int>> firstListOfMoves;
        IList<Tuple<IMoves, int>> secondListOfMoves;
        IDictionary<Tuple<int, int>, MapBlockType> map= new Dictionary<Tuple<int, int>, MapBlockType>();
        const int rows = 21;
        const int columns = 21;
        for (var r = 0; r < rows; r++) {
            for (var c = 0; c < columns; c++) {
                map.Add(new Tuple<int,int>(r, c), MapBlockType.Walk);
            }
        }

        IGameMapData firstMap = new GameMapData(1, "MAP1", 1, 10, map, new List<IMonsterSpecies>());
        _player = new Player.Player("Joker", Gender.Man, trainerNumber, new Tuple<int, int>(1, 0), new GameMap(firstMap));

        IMoves m1 = new Moves("Water tornado", DefaultStats, MonsterType.Water, Pp2);
        IMoves m2 = new Moves("Flame punch", Pp2, MonsterType.Fire, Pp1);
        firstMonsterSpeciesMoves = new List<IMoves> {m1};
        secondMonsterSpeciesMoves = new List<IMoves> {m2};
        firstListOfMoves = new List<Tuple<IMoves, int>> {new Tuple<IMoves, int>(m1, Pp2)};
        secondListOfMoves = new List<Tuple<IMoves, int>> {new Tuple<IMoves, int>(m2, Pp1)};
        _monsterBall = new CaptureItem("Monster Ball", "Can capture a monster");
        _healingPotion = new HealingItem("Healing potion", "Restore 50Hp");

        species = new MonsterSpeciesBuilder().Name("bibol").Info("Info1").MonsterType(MonsterType.Fire)
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
    public void MonsterListCheck() {
        _player.AddMonster(_monsterA);
        _player.AddMonster(_monsterB);
        Assert.AreEqual(2, _player.GetAllMonsters().Count);
        Assert.False(_player.GetAllMonsters().Contains(_monsterC));
        Assert.Equals("bibol", _player.GetAllMonsters()[0].GetName());
        Assert.AreNotEqual("bibol", _player.GetAllMonsters()[1].GetName());
        Assert.True(_player.RemoveMonster(_monsterB));
        Assert.False(_player.RemoveMonster(_monsterC));
    }

    [Test]
    public void itemListCheck() {
        this.player.addItem(monsterBall);
        assertTrue("Monster Ball",
                this.player.getAllItems().entrySet().stream().anyMatch(e -> e.getKey().equals(monsterBall)));
        assertFalse(this.player.getAllItems().containsKey(healingPotion));
        this.player.addItem(healingPotion);
        assertTrue(this.player.getAllItems().containsKey(healingPotion));
        this.player.useItem(monsterBall);
        assertFalse(this.player.getAllItems().containsKey(monsterBall));
        this.player.addItem(healingPotion);
        assertEquals(2, this.player.getAllItems().get(healingPotion).intValue());
    }

    [Test]
    public void useItemCheck() {
        this.player.addMonster(monsterA);
        this.player.addItem(healingPotion);
        this.player.getAllMonsters().get(0).setHealth(0);
        assertEquals(0, this.player.getAllMonsters().get(0).getStats().getHealth());
        this.player.useItemOnMonster(healingPotion, monsterA);
        assertEquals(this.player.getAllMonsters().get(0).getMaxHealth(),
                this.player.getAllMonsters().get(0).getStats().getHealth());
        this.player.addItem(healingPotion);
        this.player.useItemOnMonster(healingPotion, monsterA);
        assertTrue(this.player.getAllItems().containsKey(healingPotion));
    }

    [Test]
    public void playerPositionCheck() {
        final Pair<Integer, Integer> limitPosition = new Pair<>(20, 20);
        assertEquals(new Pair<>(1, 0), this.player.getPosition());
        this.player.moveUp();
        assertNotEquals(new Pair<>(1, -1), this.player.getPosition());
        this.player.moveRight();
        assertEquals(new Pair<>(2, 0), this.player.getPosition());
        this.player.moveDown();
        assertEquals(new Pair<>(2, 1), this.player.getPosition());
        this.player.moveLeft();
        assertEquals(new Pair<>(1, 1), this.player.getPosition());
        assertNotEquals(new Pair<>(10, 10), this.player.getPosition());
        this.player.setPosition(limitPosition);
        assertEquals(limitPosition, this.player.getPosition());
        assertFalse(this.player.moveDown());
        assertFalse(this.player.moveRight());
        assertTrue(this.player.moveUp());
        assertTrue(this.player.moveLeft());

    }
}

