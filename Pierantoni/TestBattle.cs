using NUnit.Framework;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.Player;

namespace Pokaiju.Pierantoni;

public class TestBattle
{
    
    private IMonster? _playerMonster2;
    private IMonster? _wildMonster;
    private INpcTrainer? _enemyTrainer;
    private IPlayer? _player;
    private const int Attack = 50;
    private const int Health = 40;
    private const int Speed = 30;
    private const int Defense = 20;
    private const int FirstLevel = 1;
    private const int ExpBase = 0;
    private const int Pp = 10;

    [SetUp]
    public void SetUp()
    {
        IMoves m1 = new Moves("Braciere", Attack, MonsterType.Fire, Pp);
        IMoves m2 = new Moves("Attacco", Attack, MonsterType.Fire, Pp);
        IMoves m3 = new Moves("Volo", Attack, MonsterType.Fire, Pp);
        IMoves m4 = new Moves("Fossa", Attack, MonsterType.Fire, Pp);

        IList<Tuple<IMoves, int>> firstListOfMoves = new List<Tuple<IMoves, int>>
            {new(m1, 10), new(m2, 10)};
        IList<Tuple<IMoves, int>> secondListOfMoves = new List<Tuple<IMoves, int>>
            {new (m3, 10), new(m4, 10)};
        IList<IMoves> firstMonsterSpeciesMoves = new List<IMoves> {m1, m2};
        IList<IMoves> secondMonsterSpeciesMoves = new List<IMoves> {m3, m4};

        var species = new MonsterSpeciesBuilder().Name("bibol").Info("Info1")
            .MonsterType(MonsterType.Fire).Health(Health).Attack(Pp).Defense(Defense).Speed(Pp)
            .MovesList(firstMonsterSpeciesMoves).Build();

        var playerMonster1 = new MonsterBuilder().Health(Health).Attack(Attack * 2).Defense(Defense).Speed(Speed)
            .Exp(ExpBase).Level(FirstLevel).Wild(false).Species(species).MovesList(firstListOfMoves).Build();

        species = new MonsterSpeciesBuilder().Name("greyfish").Info("Info").MonsterType(MonsterType.Water)
            .Health(Attack).Attack(10).Defense(10).Speed(10).MovesList(firstMonsterSpeciesMoves).Build();

        _playerMonster2 = new MonsterBuilder().Health(Health).Attack(Attack).Defense(Defense).Speed(Speed).Exp(ExpBase)
            .Level(FirstLevel).Wild(false).Species(species).MovesList(firstListOfMoves).Build();

        species = new MonsterSpeciesBuilder().Name("kracez").Info("Info").MonsterType(MonsterType.Fire)
            .Health(Attack).Attack(10).Defense(10).Speed(10).MovesList(secondMonsterSpeciesMoves).Build();

        _wildMonster = new MonsterBuilder().Health(10).Attack(Attack).Defense(0).Speed(Speed).Exp(ExpBase)
            .Level(FirstLevel).Wild(true).Species(species).MovesList(secondListOfMoves).Build();

        species = new MonsterSpeciesBuilder().Name("pirin").Info("Info").MonsterType(MonsterType.Grass)
            .Health(Attack).Attack(10).Defense(10).Speed(10).MovesList(secondMonsterSpeciesMoves).Build();

        var enemyTrainerMonster = new MonsterBuilder().Health(Health).Attack(Attack).Defense(Defense)
            .Speed(Speed).Exp(ExpBase).Level(FirstLevel).Wild(true).Species(species).MovesList(secondListOfMoves)
            .Build();


        _player = new Player("Paolo", Gender.Man, 0, new Tuple<int, int>(0, 0), new GameMap(new GameMapData(0," ",0,100,new Dictionary<Tuple<int, int>, MapBlockType>(), new List<IMonsterSpecies>())));

        _player.AddMonster(playerMonster1);
        _player.AddMonster(_playerMonster2);
        _enemyTrainer = new NpcTrainer("Luca", new List<string> { "test"}, new Tuple<int, int>(0, 0), true, true, new List<IMonster>{enemyTrainerMonster}, false);
    }

    [Test]
    public void SimpleBattle() {

        IMonsterBattle battle = new MonsterBattle(_player ?? throw new InvalidOperationException(), _wildMonster  ?? throw new InvalidOperationException());
        battle.MovesSelection(0);
        Assert.True(_wildMonster.IsAlive()); 
    }

    [Test]
    public void Capture() {
        var isCaptured = false;
        IMonsterBattle battle = new MonsterBattle(_player ?? throw new InvalidOperationException(), _wildMonster ?? throw new InvalidOperationException());
        while (!isCaptured) {
            isCaptured = battle.Capture();
        }
        Assert.AreEqual(_wildMonster, _player.GetAllMonsters()[2]);
    }

    [Test]
    public void Escape() {
        var escaped = false;
        IMonsterBattle battle = new MonsterBattle(_player ?? throw new InvalidOperationException(), _wildMonster ?? throw new InvalidOperationException());
        while (!escaped) {
            escaped = battle.Escape();
        }
        Assert.True(escaped);
    }

    [Test]
    public void ChangeMonsters() {
        IMonsterBattle battle = new MonsterBattle(_player ?? throw new InvalidOperationException(), _wildMonster?? throw new InvalidOperationException());
        if (_playerMonster2 is null) return;
        battle.PlayerChangeMonster(_playerMonster2.Id);
        Assert.AreEqual(battle.GetCurrentPlayerMonster(), _playerMonster2);
    }

    [Test]
    public void BattleWithEnemyTrainer() {
        if (_enemyTrainer != null)
        {
            if (_player != null)
            {
                IMonsterBattle battle = new MonsterBattle(_player, _enemyTrainer);
                battle.MovesSelection(0);
            }
        }

        Assert.IsFalse(_enemyTrainer != null && _enemyTrainer.IsDefeated());
    }

}