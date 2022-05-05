namespace Pokaiju.Carafassi.Tests;

using NUnit.Framework;
using Optional.Unsafe;
using Barattini;
using GameEvents;
using GameMaps;
using Castorina.Npc;
using Guo.Player;
using Pierantoni;

[TestFixture]
public class TestGameEvents
{
    // used default! to to turn off non-null not initialized warning. They are initialized in SetUp()
    private IPlayer _player = default!;
    private IMonster _monsterA = default!;
    private IMonster _monsterB = default!;
    private IMonster _monsterC = default!;

    [SetUp]
    public void SetUp()
    {
        const int baseStat = 5;
        const int monsterLevel = 5;
        const int playerId = 4343;
        IGameMap map = new GameMap(new GameMapData(1, "map", 1, 100,
            new Dictionary<Tuple<int, int>, MapBlockType>(), new List<IMonsterSpecies>()));
        _player = new Player("Player", Gender.Man, playerId, new Tuple<int, int>(1, 0), map);
        IList<IMoves> movesSpecies = new List<IMoves> {new Moves("Slap", 20, MonsterType.Fire, 10)};
        IList<Tuple<IMoves, int>> moves = new List<Tuple<IMoves, int>> {new(movesSpecies[0], 2)};

        var monsterSpeciesA = new MonsterSpeciesBuilder().Name("MonsterA")
            .MovesList(movesSpecies)
            .MonsterType(MonsterType.Fire).Attack(baseStat).Speed(baseStat).Defense(baseStat).Health(baseStat)
            .Info("Fire monsterA").Build();
        _monsterA = new MonsterBuilder().Level(monsterLevel).Species(monsterSpeciesA).MovesList(moves)
            .Build();

        var monsterSpeciesB = new MonsterSpeciesBuilder().Name("MonsterB")
            .MovesList(movesSpecies)
            .MonsterType(MonsterType.Water).Attack(baseStat).Speed(baseStat).Defense(baseStat).Health(baseStat)
            .Info("Water monsterB").Build();
        _monsterB = new MonsterBuilder().Level(monsterLevel).Species(monsterSpeciesB).MovesList(moves)
            .Build();

        var monsterSpeciesC = new MonsterSpeciesBuilder().Name("MonsterA")
            .MovesList(movesSpecies)
            .MonsterType(MonsterType.Grass).Attack(monsterLevel).Speed(monsterLevel).Defense(monsterLevel)
            .Health(monsterLevel).Info("Grass monsterC").Build();
        _monsterC = new MonsterBuilder().Level(monsterLevel).Species(monsterSpeciesC).MovesList(moves)
            .Build();
    }

    [Test]
    public void NpcPositionChange()
    {
        var startingPosition = new Tuple<int, int>(10, 10);
        var newPosition = new Tuple<int, int>(2, 0);
        INpcSimple mario = new NpcSimple("Mario", new List<string> {"Licia? She is behind you"},
            new Tuple<int, int>(0, 0), true, true);
        INpcSimple licia = new NpcSimple("Licia", new List<string> {"BOO! Scared, aren't you?"},
            startingPosition, true, true);

        IGameEvent npcEvent = new NpcPositionChanger(0, true, false, false, licia, newPosition);

        Assert.AreEqual(licia.GetPosition(), startingPosition);

        mario.AddGameEvent(npcEvent);
        mario.InteractWith();

        Assert.AreEqual(licia.GetPosition(), newPosition);
        Assert.IsFalse(npcEvent.Active);
        Assert.IsFalse(npcEvent.IsReactivable);
    }

    private static IGameMap InitMonsterSpawnMap(Tuple<int, int> eventPosition, IEnumerable<INpcSimple> npcs,
        IGameEvent monsterEvent)
    {
        IDictionary<Tuple<int, int>, MapBlockType> blocks = new Dictionary<Tuple<int, int>, MapBlockType>();
        const int rows = 20;
        const int columns = 20;
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < columns; c++)
            {
                blocks.Add(new Tuple<int, int>(r, c), MapBlockType.Walk);
            }
        }

        IGameMapData map = new GameMapData(0, "map", 0, 100,
            blocks, new List<IMonsterSpecies>());
        foreach (var npcSimple in npcs)
        {
            map.AddNpc(npcSimple);
        }

        map.AddEventAt(monsterEvent, eventPosition);

        return new GameMap(map);
    }

    [Test]
    public void UniqueMonsterSpawn()
    {
        // interaction with npc then monster spawn available
        IGameEvent monsterEvent = new UniqueMonsterEvent(15, false, false, _monsterA);
        _player.AddMonster(_monsterB);

        var eventPosition = new Tuple<int, int>(10, 10);

        INpcSimple mario = new NpcSimple("Mario",
            new List<string> {"There is a special monster over there"}, new Tuple<int, int>(0, 0),
            true, true);

        IGameEvent npcEvent = new NpcActivityChanger(1, true, true, false, mario, true);
        npcEvent.AddSuccessiveGameEvent(monsterEvent);
        mario.AddGameEvent(npcEvent);

        var map = InitMonsterSpawnMap(eventPosition, new List<INpcSimple> {mario}, monsterEvent);

        Assert.False(monsterEvent.Active);
        mario.InteractWith();

        Assert.IsTrue(map.GetEventAt(eventPosition).ValueOrFailure().Active);
        monsterEvent.Activate();
        Assert.IsTrue(monsterEvent.IsBattle());

        monsterEvent.Active = true;

        Assert.IsFalse(monsterEvent.Active);
    }

    [Test]
    public void StarterMonsterSelection()
    {
        IList<string> teacherSentences = new List<string> {"Choose a monster", "Congrats"};
        INpcSimple teacher = new NpcSimple("Doc", teacherSentences, new Tuple<int, int>(0, 0), true, true);
        INpcSimple item1 = new NpcSimple("monsterAnpc", new List<string> {"You choose monsterA"},
            new Tuple<int, int>(0, 1), true, true);
        INpcSimple item2 = new NpcSimple("monsterBnpc", new List<string> {"You choose monsterB"},
            new Tuple<int, int>(0, 2), true, true);
        INpcSimple item3 = new NpcSimple("monsterCnpc", new List<string> {"You choose monsterC"},
            new Tuple<int, int>(0, 3), true, true);

        IGameEvent teacherTextChange = new NpcTextChanger(1, false, true, true, teacher, 1);

        IGameEvent npcEventV1 = new NpcVisibilityChanger(2, false, true, true, item1, false);
        IGameEvent npcEventEn1 = new NpcActivityChanger(3, false, true, true, item1, false);

        IGameEvent npcEventV2 = new NpcVisibilityChanger(4, false, true, true, item2, false);
        IGameEvent npcEventEn2 = new NpcActivityChanger(5, false, true, true, item2, false);

        IGameEvent npcEventV3 = new NpcVisibilityChanger(6, false, true, true, item3, false);
        IGameEvent npcEventEn3 = new NpcActivityChanger(7, false, true, true, item3, false);

        teacherTextChange.AddSuccessiveGameEvent(npcEventEn1);
        teacherTextChange.AddSuccessiveGameEvent(npcEventEn2);
        teacherTextChange.AddSuccessiveGameEvent(npcEventEn3);
        teacherTextChange.AddSuccessiveGameEvent(npcEventV1);
        teacherTextChange.AddSuccessiveGameEvent(npcEventV2);
        teacherTextChange.AddSuccessiveGameEvent(npcEventV3);

        IGameEvent eventA = new MonsterGift(8, true, true, false, new List<IMonster> {_monsterA}, _player);
        eventA.AddSuccessiveGameEvent(teacherTextChange);

        IGameEvent eventB = new MonsterGift(9, true, true, false, new List<IMonster> {_monsterB}, _player);
        eventB.AddSuccessiveGameEvent(teacherTextChange);

        IGameEvent eventC = new MonsterGift(10, true, true, false, new List<IMonster> {_monsterC}, _player);
        eventC.AddSuccessiveGameEvent(teacherTextChange);

        eventA.AddDependentGameEvent(eventB);
        eventA.AddDependentGameEvent(eventC);
        eventB.AddDependentGameEvent(eventA);
        eventB.AddDependentGameEvent(eventC);
        eventC.AddDependentGameEvent(eventA);
        eventC.AddDependentGameEvent(eventB);

        item1.AddGameEvent(eventA);
        item2.AddGameEvent(eventB);
        item3.AddGameEvent(eventC);

        Assert.AreEqual(teacherSentences[0], teacher.InteractWith().ValueOrFailure());
        Assert.AreEqual(teacherSentences[0], teacher.InteractWith().ValueOrFailure());

        Assert.AreEqual(item1.InteractWith().ValueOrFailure(), "You choose monsterA");

        Assert.IsFalse(item1.IsEnabled());
        Assert.IsFalse(item1.IsVisible());
        Assert.IsFalse(item2.IsEnabled());
        Assert.IsFalse(item2.IsVisible());
        Assert.IsFalse(item3.IsEnabled());
        Assert.IsFalse(item3.IsVisible());

        Assert.IsFalse(eventA.Active);
        Assert.IsFalse(eventB.Active);
        Assert.IsFalse(eventC.Active);

        Assert.AreEqual(teacherSentences[1], teacher.InteractWith().ValueOrFailure());
    }
}