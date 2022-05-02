using Pokaiju.Guo.GameItem;

namespace Pokaiju.Barattini
{
    using Optional.Unsafe;

    public class Monster : IMonster
    {
        /// <summary>
        /// Max Experience
        /// </summary>
        public const int ExpCap = 1000;

        /// <summary>
        /// Max level
        /// </summary>
        public const int MaxLvl = 100;

        /// <summary>
        /// Number of max moves
        /// </summary>
        public const int NumMaxMoves = 4;

        private const int MaxHpStep = 40;
        private const int MinHpStep = 10;
        private const int MaxStatsStep = 10;
        private const int MinStatsStep = 1;
        private int _exp;
        private int _level;
        private readonly bool _isWild;

        private IMonsterSpecies _species;
        //private List<Tuple<Moves, int>> _movesList;
        private readonly IMonsterStats _stats;
        private readonly IMonsterStats _maxStats;

        /// <summary>
        /// Constructor for MonsterImpl.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="stats">stats</param>
        /// <param name="exp">exp</param>
        /// <param name="level">level</param>
        /// <param name="isWild">isWild</param>
        /// <param name="species">species</param>
        public Monster(int id, IMonsterStats stats, int exp, int level, bool isWild,
            IMonsterSpecies species /*, List<Tuple<IMoves, int>> movesList*/)
        {
            Id = id;
            this._maxStats =
                new MonsterStats(stats.Health, stats.Attack, stats.Defense, stats.Speed);
            this._stats =
                new MonsterStats(_maxStats.Health, _maxStats.Attack, _maxStats.Defense, _maxStats.Speed);
            this._exp = exp;
            this._level = level;
            this._isWild = isWild;
            this._species = species;
            //this._movesList = new ArrayList(movesList);
        }


        /// <inheritdoc cref="IMonster.Id"/>
        public int Id { get; }

        /// <inheritdoc cref="IMonster.GetName"/>
        public string GetName()
        {
            return _species.GetName();
        }

        /// <inheritdoc cref="IMonster.SetHealth"/>
        public void SetHealth(int hlt)
        {
            this._stats.Health = hlt <= this.GetMaxHealth() ? hlt : this.GetMaxHealth();
            this._stats.Health = hlt <= 0 ? 0 : this._stats.Health;
        }

        /// <inheritdoc cref="IMonster.RestoreStats"/>
        public void RestoreStats()
        {
            var maxStatsMap = this._maxStats.GetStatsAsMap();
            var statsMap = this._stats.GetStatsAsMap();
            foreach (var s in maxStatsMap)
            {
                statsMap[s.Key] = s.Value;
            }
        }

        /// <inheritdoc cref="IMonster.GetMaxHealth"/>
        public int GetMaxHealth()
        {
            return this._maxStats.Health;
        }

        /// <inheritdoc cref="IMonster.GetInfo"/>
        public string GetInfo()
        {
            return this._species.GetInfo();
        }

        /// <inheritdoc cref="IMonster.GetLevel"/>
        public int GetLevel()
        {
            return this._level;
        }

        private void SetLevel(int level)
        {
            this._level = level <= MaxLvl ? level : MaxLvl;
        }

        /// <inheritdoc cref="IMonster.LevelUp"/>
        public void LevelUp()
        {
            this._level++;
            this._exp = 0;
            OnLevelUp(1);
        }

        /// <inheritdoc cref="IMonster.IncExp"/>
        public void IncExp(int exp)
        {
            var incLevel = (this._exp + exp) / ExpCap;
            var oldLevel = this._level;
            SetLevel(incLevel + _level);
            incLevel = this._level - oldLevel;
            this._exp = (this._exp + exp) % ExpCap;
            if (this._level == MaxLvl)
            {
                this._exp = 0;
            }

            if (incLevel > 0)
            {
                OnLevelUp(incLevel);
            }
        }

        private void OnLevelUp(int level)
        {
            var rand = new Random();
            for (var i = 0; i < level; i++)
            {
                this._maxStats.Health = this._maxStats.Health + rand.Next(MinHpStep, MaxHpStep);
                this._maxStats.Attack = this._maxStats.Attack + rand.Next(MinStatsStep, MaxStatsStep);
                this._maxStats.Defense = this._maxStats.Defense + rand.Next(MinStatsStep, MaxStatsStep);
                this._maxStats.Speed = this._maxStats.Speed + rand.Next(MinStatsStep, MaxStatsStep);
            }

            RestoreStats();
        }

        /// <inheritdoc cref="IMonster.GetExp"/>
        public int GetExp()
        {
            return this._exp;
        }

        /// <inheritdoc cref="IMonster.GetExpCap"/>
        public int GetExpCap()
        {
            return ExpCap;
        }

        /// <inheritdoc cref="IMonster.IsWild"/>
        public bool IsWild()
        {
            return this._isWild;
        }

        /// <inheritdoc cref="IMonster.IsAlive"/>
        public bool IsAlive()
        {
            return this._stats.Health > 0;
        }

        /*
         /// <inheritdoc cref="IMonster.GetMoves"/>
         public IMoves GetMoves(int index)
        {
            if(index is > GetNumberOfMoves() or < 0)
            {
                throw new ArgumentException();
            }
            return _movesList.Get(index).GetFirst();
        }*/

        /*/// <inheritdoc cref="IMonster.GetAllMoves"/>
         public List<IMoves> GetAllMoves()
        {
            return Collections.unmodifiableList(movesList.stream().map(i -> i.getFirst()).collect(Collectors.toList()));
        }*/

        /*/// <inheritdoc cref="IMonster.GetCurrentPpByMove"/>
         public int GetCurrentPpByMove(IMoves move)
        {
            return movesList.stream().filter(i -> i.getFirst().equals(move)).findAny().get().getSecond();
        }*/

        /*/// <inheritdoc cref="IMonster.IsOutOfPp"/>
         public bool IsOutOfPp(IMoves move)
        {
            return movesList.stream().filter(i -> i.getFirst().equals(move)).findAny().get().getSecond() <= 0;
        }*/

        /*/// <inheritdoc cref="IMonster.RestoreMovePp"/>
         public void RestoreMovePp(IMoves move)
        {
            final int index = getIndexOfMove(move);
            final Pair<Moves, Integer> p = movesList.get(index);
            movesList.remove(index);
            movesList.add(index, new Pair<>(p.getFirst(), p.getFirst().getPP()));
        }*/

        /*/// <inheritdoc cref="IMonster.RestoreAllMovesPp"/>
         public void RestoreAllMovesPp()
        {
            final List<Moves> moves = movesList.stream().map(i -> i.getFirst()).collect(Collectors.toList());
            for (final var p : moves) {
            restoreMovePP(p);
        }
        }*/

        /*/// <inheritdoc cref="IMonster.DecMovePp"/>
         public void DecMovePp(IMoves move)
        {
            final int index = getIndexOfMove(move);
            final Pair<Moves, Integer> p = movesList.get(index);
            movesList.remove(index);
            movesList.add(index, new Pair<>(p.getFirst(), p.getSecond() - 1));
        }*/
        
        /*
        private int GetIndexOfMove(IMoves move) {
            for (int i = 0; i < this.movesList.size(); i++) {
                if (movesList.get(i).getFirst().equals(move)) {
                    return i;
                }
            }
            return -1;
        }
        */

        /*/// <inheritdoc cref="IMonster.IsMoveSetFull"/>
         public bool IsMoveSetFull()
        {
            return this.movesList.size() == NUM_MAX_MOVES;
        }*/

        /*/// <inheritdoc cref="IMonster.GetNumberOfMoves"/>
         public int GetNumberOfMoves()
        {
            return this.movesList.size();
        }*/

        /*/// <inheritdoc cref="IMonster.GetType"/>
         public MonsterType GetType()
        {
            return this.species.getType();
        }*/

        /// <inheritdoc cref="IMonster.CanEvolveByLevel"/>
        public bool CanEvolveByLevel()
        {
            return _species.GetEvolution().HasValue && this._species.GetEvolutionType() == EvolutionType.Level &&
                   this._level >= ((MonsterSpeciesByLevel)_species).GetEvolutionLevel();
        }

        /// <inheritdoc cref="IMonster.CanEvolveByItem"/>
        public bool CanEvolveByItem(IGameItem item)
        {
            return _species.GetEvolution().HasValue && this._species.GetEvolutionType() == EvolutionType.Item
                                                    && item.Equals(((MonsterSpeciesByItem)_species).GetItem())
                                                    && item.GetGameType() == GameItemTypes.EvolutionTool;
        }

        /// <inheritdoc cref="IMonster.Evolve"/>
        public void Evolve()
        {
            if (this._species.GetEvolution().HasValue)
            {
                this._species = this._species.GetEvolution().ValueOrFailure();
            }
        }

        /// <inheritdoc cref="IMonster.GetSpecies"/>
        public IMonsterSpecies GetSpecies()
        {
            return this._species;
        }

        /// <inheritdoc cref="IMonster.GetStats"/>
        public IMonsterStats GetStats()
        {
            return this._stats;
        }

        /// <inheritdoc cref="IMonster.GetMaxStats"/>
        public IMonsterStats GetMaxStats()
        {
            return this._maxStats;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private bool Equals(IMonster other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((IMonster)obj);
        }

        public override string ToString()
        {
            return _species + "\nHealth: " + _stats.Health + "\nAttack: " + _stats.Attack
                   + "\nDefense: " + _stats.Defense + "\nSpeed: " + _stats.Speed + "\nLevel: "
                   + _level + "\nExp: " + _exp + /*"\nMoves:" + _movesList.ToString() +*/ "\nIsWild: "
                   + _isWild + "\n";
        }
    }
}