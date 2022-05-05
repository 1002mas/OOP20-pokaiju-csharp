using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameEvents;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Castorina.Tests;
using NUnit.Framework;

[TestFixture]
public class TestNpc
{
    private const int Monster1Health = 5;
    private const int Monster3Health = 75;
    private const int Monster2Health = 10;
    private const int Monster1Attack = 50;
    private const int Monster2Attack = 5;
    private const int Monster3Attack = 70;
    private const int GenericValue = 32;
    private const int Rows = 21;
    private const int Columns = 21;

    private INpcSimple? _npc1;
    private INpcSimple? _npc2;
    private INpcMerchant? _npc3;
    private INpcTrainer? _npc4;
    private INpcTrainer? _npc5;
    private Tuple<int, int>? _position;
    private IList<string>? _sentences;
    private IMonster? _monster1;
    private IPlayer? _player;
    private IList<IMonster>? _monsterList;
    private IList<Tuple<IGameItem, int>>? _list;
    private IGameEvent? _npcEvent;

    [SetUp]
    public void SetUp()
    {
        _sentences = new List<string>();
        _sentences.Add("Frase 1");
        _sentences.Add("Frase 2");
        _sentences.Add("Frase 3");
        _position = new Tuple<int, int>(1, 1);
        IDictionary<Tuple<int, int>, MapBlockType> map = new Dictionary<Tuple<int, int>, MapBlockType>();
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                map.Add(new Tuple<int, int>(r, c), MapBlockType.Walk);
            }
        }

        IGameMapData firstMap = new GameMapData(1, "MAP1", 1, 10, map, new List<IMonsterSpecies>());
        _player = new Player("player", Gender.Woman, 0, _position, new GameMap(firstMap));

        // item
        IGameItem item1 = new CaptureItem("pietra", "description");
        IGameItem item2 = new CaptureItem("carta", "description");
        var inventory = new Dictionary<IGameItem, int>
        {
            { item1, 4 },
            { item2, 3 }
        };
        // monster
        IMoves m1 = new Moves("move1", 3, MonsterType.Grass, 9);
        IMoves m2 = new Moves("move2", 3, MonsterType.Fire, 3);
        IList<Tuple<IMoves, int>> listOfMoves = new List<Tuple<IMoves, int>>();
        listOfMoves.Add(new Tuple<IMoves, int>(m1, 5));
        listOfMoves.Add(new Tuple<IMoves, int>(m2, 6));
        IList<IMoves> listMoves = new List<IMoves>();
        listMoves.Add(m1);
        listMoves.Add(m2);

        var species = new MonsterSpeciesBuilder().Name("nome1").Info("Info1")
            .MonsterType(MonsterType.Fire).MovesList(listMoves).Build();
        _monster1 = new MonsterBuilder().Health(Monster1Health).Attack(Monster1Attack)
            .Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();

        var monster2 = new MonsterBuilder().Health(Monster2Health).Attack(Monster2Attack).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();

        var monster3 = new MonsterBuilder().Health(Monster3Health).Attack(Monster3Attack).Defense(GenericValue)
            .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
            .MovesList(listOfMoves).Build();

        _monsterList = new List<IMonster>();
        _monsterList.Add(monster2);
        IList<IMonster> monsterListNpc5 = new List<IMonster>();
        monsterListNpc5.Add(monster3);
        _player.AddMonster(_monster1);
        // npc
        _npc1 = new NpcSimple("nome1", _sentences, _position, false, false);
        _npc2 = new NpcHealer("nome2", _sentences, _position, false, false, _player);
        _npc3 = new NpcMerchant("nome3", _sentences, _position, false, false, inventory);
        _list = new List<Tuple<IGameItem, int>>();
        _list.Add(new Tuple<IGameItem, int>(item1, 2));
        _npc4 = new NpcTrainer("nome4", _sentences, _position, false, false, _monsterList, false);
        _npc5 = new NpcTrainer("nome5", _sentences, _position, false, false, monsterListNpc5, false);
        IList<IMonster> npcEventList = new List<IMonster>();
        npcEventList.Add(_monster1);
        _npcEvent = new MonsterGift(8, true, true, false, npcEventList, _player);
        _npc1.AddGameEvent(_npcEvent);
    }

    [Test]
    public void CommonFields()
    {

        Assert.AreEqual("nome1", _npc1?.GetName());
        Assert.AreEqual(0, _npc1?.GetCurrentSetence());
        Assert.AreEqual(_position, _npc1?.GetPosition());
        // enabled : false
        Assert.AreEqual(Option.None<string>(), _npc1?.InteractWith());
        // enabled : true
        _npc1?.SetEnabled(true);
        Assert.AreEqual(Option.Some(_sentences?.ElementAt(0)), _npc1?.InteractWith());
        // enabled : true and event not active
        if (_npcEvent == null) return;
        _npcEvent.Active = false;
        _npc1?.InteractWith();
        Assert.AreEqual(Option.None<IGameEvent>(), _npc1?.GetTriggeredEvent());
        // enabled : true and event active
        _npcEvent.Active = true;
        _npc1?.InteractWith();
        Assert.AreEqual(Option.Some(_npcEvent), _npc1?.GetTriggeredEvent());

    }

    [Test]
    public void TestNpcHealer()
    {
        _npc2?.InteractWith();
        Assert.AreEqual(_monster1?.GetMaxHealth(), _player?.GetAllMonsters().ElementAt(0).GetStats().Health);
    }

    [Test]
    public void TestNpcTrainer()
    {
        if (_player == null || _npc4 == null || _npc5 == null) return;
        Assert.AreEqual(_monsterList, _npc4?.GetMonstersOwned());
        if (_npc4 == null) return;
        IMonsterBattle battle = new MonsterBattle(_player, _npc4);
        battle.MovesSelection(0);
        Assert.True(_npc4.IsDefeated());

        IMonsterBattle battle2 = new MonsterBattle(_player, _npc5);
        battle2.MovesSelection(0);
        Assert.False(_npc5.IsDefeated());
        battle2.EnemyAttack();
        Assert.True(battle.IsOver());
        Assert.False(_npc5.IsDefeated());


    }

    [Test]
    public void TestNpcMerchant()
    {
        if (_player == null || _list == null) return;
        var money = _player.GetMoney();
        Assert.AreEqual(8, _npc3?.GetTotalPrice(_list));
        Assert.True(_npc3?.BuyItem(_list, _player));
        Assert.AreEqual(money - 8, _player.GetMoney());

    }
}