namespace Pokaiju.Barattini;

using Guo.GameItem;
using Pierantoni;
using NUnit.Framework;

[TestFixture]
public class TestMonster
{
    private const int FirstEvolutionLevel = 14;
    private const int SecondEvolutionLevel = 30;
    private const int Hlt = 50;
    private const int Atk = 10;
    private const int Dfs = 10;
    private const int Spd = 10;
    private const int HighExp = 5_863_655;
    private const string MonsterName = "Paperino";

    private IMonster? _monster;
    private IMonster? _monsterByItem;
    private IMonster? _monsterByLevelAndItem;

    [SetUp]
    public void SetUp()
    {
        IMoves m1 = new Moves("Braciere", 50, MonsterType.Fire, 10);
        IMoves m2 = new Moves("Attacco", 10, MonsterType.Fire, 10);
        IMoves m3 = new Moves("Volo", 50, MonsterType.Fire, 10);
        IMoves m4 = new Moves("Fossa", 50, MonsterType.Fire, 10);

        IList<Tuple<IMoves, int>> listOfMoves = new List<Tuple<IMoves, int>>
        {
            new Tuple<IMoves, int>(m1, 10), new Tuple<IMoves, int>(m2, 10),
            new Tuple<IMoves, int>(m3, 10), new Tuple<IMoves, int>(m4, 10)
        };

        IList<IMoves> monsterSpeciesMoves = new List<IMoves> { m1, m2, m3, m4 };

        // LEVEL TEST INITIALIZATION
        var secondEvolution = new MonsterSpeciesBuilder().Name("Pippo3").Info("Info3")
            .MonsterType(MonsterType.Fire).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .MovesList(monsterSpeciesMoves).Build();

        var firstEvolution = new MonsterSpeciesBuilder().Name("Pippo2").Info("Info2")
            .MonsterType(MonsterType.Fire).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Evolution(secondEvolution).EvolutionLevel(SecondEvolutionLevel).MovesList(monsterSpeciesMoves)
            .Build();

        var species = new MonsterSpeciesBuilder().Name("Pippo").Info("Info")
            .MonsterType(MonsterType.Fire).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Evolution(firstEvolution)
            .EvolutionLevel(FirstEvolutionLevel).MovesList(monsterSpeciesMoves).Build();

        _monster = new MonsterBuilder().Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd).Wild(false)
            .Species(species).MovesList(listOfMoves).Build();

        // ITEM TEST INITIALIZATION
        IGameItem neededItem = new EvolutionItem("Pietra" + MonsterName, "desc1");

        var firstEvolutionByItem = new MonsterSpeciesBuilder().Name(MonsterName + "2")
            .Info("Info2")
            .MonsterType(MonsterType.Water).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .MovesList(monsterSpeciesMoves).Build();

        var speciesByItem = new MonsterSpeciesBuilder().Name(MonsterName).Info("Info")
            .MonsterType(MonsterType.Water).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Evolution(firstEvolutionByItem).GameItem(neededItem).MovesList(monsterSpeciesMoves).Build();

        _monsterByItem = new MonsterBuilder().Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd).Wild(false)
            .Species(speciesByItem).MovesList(listOfMoves).Build();

        // FIRST EVOLUTION WITH LEVEL, SECOND WITH ITEM INITIALIZATION
        var secondEvolutionByLevelAndItem = new MonsterSpeciesBuilder().Name("Topolino3")
            .Info("Info3").MonsterType(MonsterType.Grass).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .MovesList(monsterSpeciesMoves).Build();

        var firstEvolutionByLevelAndItem = new MonsterSpeciesBuilder().Name("Topolino2")
            .Info("Info2").MonsterType(MonsterType.Grass).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Evolution(secondEvolutionByLevelAndItem).GameItem(neededItem).MovesList(monsterSpeciesMoves)
            .Build();

        var speciesByLevelAndItem = new MonsterSpeciesBuilder().Name("Topolino").Info("Info")
            .MonsterType(MonsterType.Grass).Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Evolution(firstEvolutionByLevelAndItem).EvolutionLevel(FirstEvolutionLevel)
            .MovesList(monsterSpeciesMoves).Build();

        _monsterByLevelAndItem = new MonsterBuilder().Health(Hlt).Attack(Atk).Defense(Dfs).Speed(Spd)
            .Wild(false).Species(speciesByLevelAndItem).MovesList(listOfMoves).Build();
    }

    [Test]
    public void FirstStats()
    {
        Assert.AreEqual(1, _monster?.GetLevel());
        Assert.AreEqual(0, _monster?.GetExp());
        Assert.AreEqual(Hlt, _monster?.GetStats().Health);
    }

    [Test]
    public void EvolvingByLevel()
    {
        _monster?.IncExp((FirstEvolutionLevel - 2) * Monster.ExpCap);
        Assert.AreEqual("Pippo", _monster?.GetName());
        _monster?.IncExp(Monster.ExpCap);
        if ((bool)_monster?.CanEvolveByLevel())
        {
            _monster.Evolve();
        }

        Assert.AreEqual("Pippo2", _monster.GetName());
        _monster.IncExp((SecondEvolutionLevel - _monster.GetLevel() - 1) * Monster.ExpCap);
        _monster.IncExp(Monster.ExpCap);
        if (_monster.CanEvolveByLevel())
        {
            _monster.Evolve();
        }

        Assert.AreEqual("Pippo3", _monster.GetName());
        Assert.AreEqual(SecondEvolutionLevel, _monster.GetLevel());
        Assert.AreEqual(0, _monster.GetExp());
        _monster.IncExp(HighExp);
        Assert.AreEqual(Monster.MaxLvl, _monster.GetLevel());
        Assert.AreEqual(0, _monster.GetExp());
    }

    [Test]
    public void EvolutionByRightItem()
    {
        var holdedItemRight = new EvolutionItem("PietraPaperino", "desc2");
        Assert.AreEqual("Paperino", _monsterByItem?.GetName());
        Assert.True(_monsterByItem?.CanEvolveByItem(holdedItemRight));
        if ((bool)_monsterByItem?.CanEvolveByItem(holdedItemRight))
        {
            _monsterByItem.Evolve();
        }

        Assert.AreEqual(MonsterName + "2", _monsterByItem.GetName());
    }

    [Test]
    public void EvolutionByWrongItem()
    {
        var holdedItemWrong = new EvolutionItem("PietraPippo", "desc3");
        Assert.AreEqual("Paperino", _monsterByItem?.GetName());
        Assert.False(_monsterByItem?.CanEvolveByItem(holdedItemWrong));
        if ((bool)_monsterByItem?.CanEvolveByItem(holdedItemWrong))
        {
            _monsterByItem.Evolve();
        }

        Assert.AreNotEqual(MonsterName + "2", _monsterByItem.GetName());
        Assert.AreEqual(MonsterName, _monsterByItem.GetName());
    }

    [Test]
    public void EvolveByLevelAndItem()
    {
        var holdedItem = new EvolutionItem("Pietra" + MonsterName, "desc4");
        _monsterByLevelAndItem?.IncExp((FirstEvolutionLevel - 2) * Monster.ExpCap);
        Assert.AreEqual("Topolino", _monsterByLevelAndItem?.GetName());
        _monsterByLevelAndItem?.IncExp(Monster.ExpCap);
        if ((bool)_monsterByLevelAndItem?.CanEvolveByLevel())
        {
            _monsterByLevelAndItem.Evolve();
        }

        Assert.AreEqual("Topolino2", _monsterByLevelAndItem.GetName());
        _monsterByLevelAndItem
            .IncExp((SecondEvolutionLevel - _monsterByLevelAndItem.GetLevel() - 1) * Monster.ExpCap);
        _monsterByLevelAndItem.IncExp(Monster.ExpCap);
        Assert.AreEqual("Topolino2", _monsterByLevelAndItem.GetName());
        Assert.AreEqual(SecondEvolutionLevel, _monsterByLevelAndItem.GetLevel());
        Assert.AreEqual(0, _monsterByLevelAndItem.GetExp());
        _monsterByLevelAndItem.IncExp(HighExp);
        Assert.AreEqual(Monster.MaxLvl, _monsterByLevelAndItem.GetLevel());
        Assert.AreEqual(0, _monsterByLevelAndItem.GetExp());
        if (_monsterByLevelAndItem.CanEvolveByItem(holdedItem))
        {
            _monsterByLevelAndItem.Evolve();
        }

        Assert.AreEqual("Topolino3", _monsterByLevelAndItem.GetName());
    }

    [Test]
    public void SpeciesBuilderTest()
    {
        Assert.AreEqual(_monster?.GetSpecies().GetName(), "Pippo");
    }
}