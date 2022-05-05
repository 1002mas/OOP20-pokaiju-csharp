using NUnit.Framework;
using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina.Npc;
using Pokaiju.Pierantoni;

namespace Pokaiju.Carafassi.Tests
{
    [TestFixture]
    public class TestGameMaps
    {
        private const string MonsterName = "Kracez";
        private const int WildCoordX = 10;

        private readonly Tuple<int, int> _mapChangePosition = new Tuple<int, int>(8, 3);
        private readonly Tuple<int, int> _charSpawn = new Tuple<int, int>(0, 0);

        private IGameMap? _map;
        private INpcSimple? _npc;
        private INpcSimple? _npcHidden;
        private INpcSimple? _npcDisabled;

        [SetUp]
        public void SetUp()
        {
            int baseStat = 5;
            IList<IMoves> movesSpecies = new List<IMoves>() {new Moves("Slap", 20, MonsterType.Fire, 10)};
            IList<Tuple<IMoves, int>> moves = new List<Tuple<IMoves, int>>()
                {new Tuple<IMoves, int>(movesSpecies[0], 2)};

            IMonsterSpecies monsterSpeciesA = new MonsterSpeciesBuilder().Name(MonsterName)
                .MovesList(movesSpecies)
                .MonsterType(MonsterType.Fire).Attack(baseStat).Speed(baseStat).Defense(baseStat).Health(baseStat)
                .Info("Fire monsterA").Build();

            IGameMapData mapData = new GameMapData(1, "map", 1, 100,
                BuildMapBlocks(), new List<IMonsterSpecies>() {monsterSpeciesA});
            mapData.AddMapLink(new GameMapData(2, "SecondMap", 0, 0,
                    new Dictionary<Tuple<int, int>, MapBlockType>(), new List<IMonsterSpecies>()),
                _mapChangePosition, _charSpawn);

            _npc = new NpcSimple("npc", new List<string>(), new Tuple<int, int>(10, 10), true, true);
            _npcHidden = new NpcSimple("ninja", new List<string>(), new Tuple<int, int>(5, 5), false, true);
            _npcDisabled = new NpcSimple("expired", new List<string>(), new Tuple<int, int>(7, 7), false, false);
            mapData.AddNpc(_npc);
            mapData.AddNpc(_npcHidden);
            mapData.AddNpc(_npcDisabled);

            _map = new GameMap(mapData);
        }

        private IDictionary<Tuple<int, int>, MapBlockType> BuildMapBlocks()
        {
            IDictionary<Tuple<int, int>, MapBlockType> blocks = new Dictionary<Tuple<int, int>, MapBlockType>();
            int rows = 20;
            int columns = 20;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Tuple<int, int> coord = new Tuple<int, int>(r, c);
                    if (r == 0 || c == 0 || r == (rows - 1) || c == (columns - 1))
                    {
                        blocks.Add(coord, MapBlockType.Obstacle);
                    }
                    else if (_mapChangePosition.Item1 == r && _mapChangePosition.Item2 == c)
                    {
                        blocks.Add(coord, MapBlockType.MapChange);
                    }
                    else if (r == WildCoordX)
                    {
                        blocks.Add(coord, MapBlockType.Wild);
                    }
                    else
                    {
                        blocks.Add(coord, MapBlockType.Walk);
                    }
                }
            }

            return blocks;
        }

        [Test]
        public void CanWalkInMap()
        {
            Assert.IsTrue(_map.CanPassThrough(new Tuple<int, int>(1, 1)));
            Assert.IsFalse(_map.CanPassThrough(new Tuple<int, int>(0, 0)));

            Assert.IsFalse(_map.CanPassThrough(_npc.GetPosition()));
            Assert.IsFalse(_map.CanPassThrough(_npcHidden.GetPosition()));
            Assert.IsTrue(_map.CanPassThrough(_npcDisabled.GetPosition()));
        }

        [Test]
        public void WildMonsterSpawn()
        {
            const int maxTries = 30;
            Option<IMonster> monster = Option.None<IMonster>();
            for (int i = 0; i < maxTries; i++)
            {
                monster = _map.GetWildMonster(new Tuple<int, int>(WildCoordX, 1));
                if (monster.HasValue)
                {
                    break;
                }
            }

            Assert.IsTrue(monster.HasValue);
            Assert.AreEqual(MonsterName, monster.ValueOrFailure().GetName());
        }

        [Test]
        public void MapChange()
        {
            Assert.IsTrue(_map.CanChangeMap(_mapChangePosition));
            _map.ChangeMap(_mapChangePosition);
    
            Assert.AreEqual(2, _map.GetCurrentMapId());
            
            Option<Tuple<int, int>> playerPos = _map.GetPlayerMapPosition();
            Assert.IsTrue(playerPos.HasValue);
            Assert.AreEqual(_charSpawn, playerPos.ValueOrFailure());
        }
    }
}