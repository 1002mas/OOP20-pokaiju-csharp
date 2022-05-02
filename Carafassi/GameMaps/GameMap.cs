using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameEvents;

namespace Pokaiju.Carafassi.GameMaps
{
    public class GameMap : IGameMap
    {
        private const int MonsterSpawnRate = 5;
        private const int MaximumMonsterSpawnRate = 10;
        private IGameMapData _mapData;
        private Option<Tuple<int, int>> _enteringStartPosition;

        /// <summary>
        /// It manages maps and map switching.
        /// </summary>
        /// <param name="mapData">Initial map.</param>
        public GameMap(IGameMapData mapData)
        {
            _mapData = mapData;
            _enteringStartPosition = Option.None<Tuple<int, int>>();
        }

        /// <inheritdoc cref="IGameMap.GetCurrentMapId"/>>
        public int GetCurrentMapId()
        {
            return _mapData.MapId;
        }

        /// <inheritdoc cref="IGameMap.CanPassThrough"/>>
        public bool CanPassThrough(Tuple<int, int> block)
        {
            return _mapData.GetBlockType(block).CanPassThrough();
        }

        //TODO use Moves instead of string
        /*   private IList<Tuple<string, int>> GetRandomListMoves(IMonsterSpecies species)
           {
               int maxProb = 10;
               int prob = 5;
               IList<Tuple<string, int>> movesList = new List<Tuple<string, int>>();
               IList<string> learnableMoves = species.GetAllLearnableMoves();
               for (final Moves m :
               learnableMoves) {
                   if (movesList.size() < MonsterImpl.NUM_MAX_MOVES && ThreadLocalRandom.current().nextInt(maxProb) < prob)
                   {
                       movesList.add(new Pair<>(m, m.getPP()));
                   }
                   else
                   {
                       break;
                   }
               }
               if (movesList.isEmpty() && !learnableMoves.isEmpty())
               {
                   movesList.add(new Pair<>(learnableMoves.get(0), learnableMoves.get(0).getPP()));
               }
   
               return movesList;
           }
   */
        /// <inheritdoc cref="IGameMap.GetWildMonster"/>>
        public Option<IMonster> GetWildMonster(Tuple<int, int> pos)
        {
            IList<IMonsterSpecies> monsters = _mapData.GetMonstersInArea();
            Random rnd = new Random();


            if (!_mapData.GetBlockType(pos).CanMonstersSpawn() || monsters.Count > 0
                                                               || rnd.Next(MaximumMonsterSpawnRate) >
                                                               MonsterSpawnRate)
            {
                return Option.None<IMonster>();
            }

            IMonsterSpecies species = monsters[rnd.Next(monsters.Count)];
            int monsterLevel = rnd.Next(_mapData.GetWildMonsterLevelRange().Item1,
                _mapData.GetWildMonsterLevelRange().Item2);
            IMonster m = new MonsterBuilder().Species(species).Wild(true)
                .Exp(0).Level(monsterLevel).Build();
            /*  .MovesList(GetRandomListMoves(species))*/
            return Option.Some<>(m);
        }

        /// <inheritdoc cref="IGameMap.ChangeMap"/>>
        public void ChangeMap(Tuple<int, int> playerPosition)
        {
            if (CanChangeMap(playerPosition) && _mapData.GetNextMap(playerPosition).HasValue)
            {
                Tuple<IGameMapData, Tuple<int, int>> p = _mapData.GetNextMap(playerPosition).ValueOrFailure();
                _enteringStartPosition = Option.Some<>(p.Item2);
                _mapData = p.Item1;
            }
            else
            {
                throw new ArgumentException("Cannot change map for the given position.");
            }
        }

        /// <inheritdoc cref="IGameMap.CanChangeMap"/>>
        public bool CanChangeMap(Tuple<int, int> playerPosition)
        {
            return _mapData.GetNextMap(playerPosition).HasValue;
        }

        /// <inheritdoc cref="IGameMap.GetPlayerMapPosition"/>>
        public Option<Tuple<int, int>> GetPlayerMapPosition()
        {
            Option<Tuple<int, int>> temp = _enteringStartPosition;
            _enteringStartPosition = Option.None<Tuple<int, int>>();
            return temp;
        }

        /// <inheritdoc cref="IGameMap.GetNpcAt"/>>
        public Option<string> GetNpcAt(Tuple<int, int> position)
        {
            Option<string> npc = _mapData.GetNpc(position);
            //TODO npc.isEnabled
            if (npc.ValueOrFailure() is null)
            {
                return Option.None<string>();
            }

            return npc;
        }

        /// <inheritdoc cref="IGameMap.GetEventAt"/>>
        public Option<IGameEvent> GetEventAt(Tuple<int, int> position)
        {
            Option<IGameEvent> gameEvent = _mapData.GetEvent(position);
            return gameEvent.HasValue && gameEvent.ValueOrFailure().Active ? gameEvent : Option.None<IGameEvent>();
        }

        /// <inheritdoc cref="IGameMap.GetAllNpcsInCurrentMap"/>>
        public IList<string> GetAllNpcsInCurrentMap()
        {
            return _mapData.GetAllNpcs();
        }

        /// <inheritdoc cref="object.ToString"/>>
        public override string? ToString()
        {
            return _mapData.ToString();
        }
    }
}