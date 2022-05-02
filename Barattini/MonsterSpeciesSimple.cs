using System.Collections.Immutable;
using Pokaiju.Pierantoni;

namespace Pokaiju.Barattini
{
    using Optional;

    public class MonsterSpeciesSimple : IMonsterSpecies
    {
        private readonly Option<IMonsterSpecies> _evolution;
        private readonly string _name;
        private readonly string _info;
        private readonly MonsterType _type;
        private readonly EvolutionType _evolutionType;
        private readonly IMonsterStats _stats;
        private readonly IList<IMoves> _movesList;

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
        protected MonsterSpeciesSimple(string name, string info, MonsterType type, IMonsterStats stats,
            Option<IMonsterSpecies> evolution, EvolutionType evolutionType, IList<IMoves> movesList)
        {
            _name = name;
            _info = info;
            _type = type;
            _stats = stats;
            _evolution = evolution;
            _evolutionType = evolutionType;
            _movesList = movesList;
        }

        /// <summary>
        /// MonsterSpeciesSimple public constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="info">info</param>
        /// <param name="type">type</param>
        /// <param name="stats">stats</param>
        /// <param name="movesList">moves list</param>
        public MonsterSpeciesSimple(string name, string info, MonsterType type,
            IMonsterStats stats, IList<IMoves> movesList)
            : this(name, info, type, stats, Option.None<IMonsterSpecies>(), EvolutionType.None, movesList)
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

        /// <inheritdoc cref="IMonsterSpecies.GetType"/>
         public MonsterType GetMonsterType()
        {
            return _type;
        }

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

        /// <inheritdoc cref="IMonsterSpecies.GetAllLearnableMoves"/>
         public IList<IMoves> GetAllLearnableMoves()
        {
            return _movesList.ToImmutableList();
        }

        public override string ToString()
        {
            return "Name: " + _name + /*"\nType: " + this._type + */"\nInfo: " + _info;
        }
    }
}