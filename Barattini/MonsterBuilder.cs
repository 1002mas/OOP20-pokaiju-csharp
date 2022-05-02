namespace Pokaiju.Barattini
{
    using Microsoft.VisualBasic.CompilerServices;

    public class MonsterBuilder : IMonsterBuilder
    {
        private const string LastIdPath = "data/generatedId.dat";
        private const int MinLevel = 1;
        private static int _id;
        private int _exp;
        //private List<Tuple<Moves, int>> _movesList;
        private bool _isWild;
        private int _level = MinLevel;
        private IMonsterSpecies? _species;
        private readonly IMonsterStats _stats = new MonsterStats(-1, -1, -1, -1);

        static MonsterBuilder()
        {
            try
            {
                var id = File.ReadAllText(LastIdPath);
                _id = int.Parse(id);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <inheritdoc cref="IMonsterBuilder.Level"/>
        public IMonsterBuilder Level(int level)
        {
            if (level is > Monster.MaxLvl or < MinLevel)
            {
                throw new ArgumentException("Illegal argument");
            }

            _level = level;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Health"/>
        public IMonsterBuilder Health(int health)
        {
            _stats.Health = health;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Attack"/>
        public IMonsterBuilder Attack(int atk)
        {
            _stats.Attack = atk;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Defense"/>
        public IMonsterBuilder Defense(int dfs)
        {
            _stats.Defense = dfs;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Speed"/>
        public IMonsterBuilder Speed(int spd)
        {
            _stats.Speed = spd;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Exp"/>
        public IMonsterBuilder Exp(int exp)
        {
            if (exp is > Monster.ExpCap or < 0)
            {
                throw new ArgumentException("Illegal argument");
            }

            _exp = exp;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Wild"/>
        public IMonsterBuilder Wild(bool isWild)
        {
            this._isWild = isWild;
            return this;
        }

        /*/// <inheritdoc cref="IMonsterBuilder.MovesList"/>
         public IMonsterBuilder MovesList(List<Tuple<IMoves, int>> movesList)
        {
            _movesList = new ArrayList(movesList);
            _movesList = this._movesList.subList(0,
                movesList.Count < Monster.NumMaxMoves ? movesList.Count : Monster.NumMaxMoves);
            return this;
        }*/

        /// <inheritdoc cref="IMonsterBuilder.Species"/>
        public IMonsterBuilder Species(IMonsterSpecies species)
        {
            _species = species;
            return this;
        }

        /// <inheritdoc cref="IMonsterBuilder.Build"/>
        public IMonster Build()
        {
            if (_id <= 0 || _species == null /*|| _movesList.isEmpty()*/)
            {
                throw new IncompleteInitialization();
            }

            IMonster monster = new Monster(_id, _species.GetBaseStats(), 0, MinLevel, _isWild,
                _species /*, _movesList*/);
            _id++;
            for (var i = MinLevel; i < _level; i++)
            {
                monster.LevelUp();
            }
            monster.IncExp(_exp);
            foreach (var v in _stats.GetStatsAsMap().Where(stat => stat.Value > 0))
            {
                monster.GetStats().GetStatsAsMap()[v.Key] = v.Value;
                monster.GetMaxStats().GetStatsAsMap()[v.Key] = v.Value;
            }
            return monster;
        }
    }
}