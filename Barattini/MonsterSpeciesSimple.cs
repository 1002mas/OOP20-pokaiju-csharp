namespace Pokaiju.Barattini
{
    using Optional;

    public class MonsterSpeciesSimple : IMonsterSpecies
    {
        private readonly Option<IMonsterSpecies> _evolution;
        private readonly string _name;

        private readonly string _info;

        //private IMonsterType _type;
        private readonly EvolutionType _evolutionType;

        private readonly IMonsterStats _stats;
        //private List<Moves> _movesList;

        /// <summary>
        /// MonsterSpeciesSimple protected constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="info">info</param>
        /// <param name="type">type</param>
        /// <param name="stats">stats</param>
        /// <param name="evolution">evolution</param>
        /// <param name="evolutionType">evolutionType</param>
        /// <param name="movesList">movesList</param>
        protected MonsterSpeciesSimple(string name, string info, /*IMonsterType type,*/ IMonsterStats stats,
            Option<IMonsterSpecies> evolution, EvolutionType evolutionType /*, List<Moves> movesList*/)
        {
            _name = name;
            _info = info;
            //_type = type;
            _stats = stats;
            _evolution = evolution;
            _evolutionType = evolutionType;
            //_movesList = movesList;
        }

        /// <summary>
        /// MonsterSpeciesSimple public constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="info">info</param>
        /// <param name="type">type</param>
        /// <param name="stats">stats</param>
        /// <param name="movesList">movesList</param>
        public MonsterSpeciesSimple(string name, string info, /*IMonsterType type,*/
            IMonsterStats stats /*, List<Moves> movesList*/)
            : this(name, info, stats, Option.None<IMonsterSpecies>(), EvolutionType.None)
        {
        }

        /// <inheritdoc cref="IMonsterSpecies.GetName"/>
        public string GetName()
        {
            return _name;
        }

        /// <inheritdoc cref="IMonsterSpecies.GetInfo"/>
        public string GetInfo()
        {
            return _info;
        }

        /*/// <inheritdoc cref="IMonsterSpecies.GetType"/>
         public MonsterType getType()
        {
            throw new NotImplementedException();
        }*/

        /// <inheritdoc cref="IMonsterSpecies.GetBaseStats"/>
        public IMonsterStats GetBaseStats()
        {
            return _stats;
        }

        /// <inheritdoc cref="IMonsterSpecies.GetEvolutionType"/>
        public EvolutionType GetEvolutionType()
        {
            return _evolutionType;
        }

        /// <inheritdoc cref="IMonsterSpecies.GetEvolution"/>
        public Option<IMonsterSpecies> GetEvolution()
        {
            return _evolution;
        }

        /*/// <inheritdoc cref="IMonsterSpecies.GetAllLearnableMoves"/>
         public List<IMoves> GetAllLearnableMoves()
        {
            throw new NotImplementedException();
        }*/

        public override string ToString()
        {
            return "Name: " + _name + /*"\nType: " + this._type + */"\nInfo: " + _info;
        }
    }
}