using Pokaiju.Guo.GameItem;

namespace Pokaiju.Barattini
{
    using Microsoft.VisualBasic.CompilerServices;
    using Optional;
    using Optional.Unsafe;

    public class MonsterSpeciesBuilder : IMonsterSpeciesBuilder
    {
        private string? _name;
        private string? _info;
        //private IMonsterType _type;
        private Option<int> _evolutionLevel = Option.None<int>();
        private readonly IMonsterStats _stats = new MonsterStats(1, 1, 1, 1);
        private IMonsterSpecies? _evolution;
        private Option<IGameItem> _gameItem = Option.None<IGameItem>();
        //private List<IMoves> _movesList = new ArrayList<IMoves>();

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Name"/>
        public IMonsterSpeciesBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Info"/>
        public IMonsterSpeciesBuilder Info(string info)
        {
            _info = info;
            return this;
        }

        /*/// <inheritdoc cref="IMonsterSpeciesBuilder.MonsterType"/>
         public IMonsterSpeciesBuilder MonsterType(MonsterType type)
        {
            this._type = type;
            return this;
        }*/

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Evolution"/>
        public IMonsterSpeciesBuilder Evolution(IMonsterSpecies evolution)
        {
            _evolution = evolution;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.EvolutionLevel"/>
        public IMonsterSpeciesBuilder EvolutionLevel(int level)
        {
            _evolutionLevel = Option.Some(level);
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Health"/>
        public IMonsterSpeciesBuilder Health(int health)
        {
            _stats.Health = health;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Attack"/>
        public IMonsterSpeciesBuilder Attack(int attack)
        {
            _stats.Attack = attack;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Defense"/>
        public IMonsterSpeciesBuilder Defense(int defense)
        {
            _stats.Defense = defense;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Speed"/>
        public IMonsterSpeciesBuilder Speed(int speed)
        {
            _stats.Speed = speed;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.GameItem"/>
         public IMonsterSpeciesBuilder GameItem(IGameItem gameItem)
        {
            _gameItem = Option.Some(gameItem);
            return this;
        }

        /*/// <inheritdoc cref="IMonsterSpeciesBuilder.MovesList"/>
         public IMonsterSpeciesBuilder MovesList(List<Moves> movesList)
        {
            _movesList = movesList;
            return this;
        }*/

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Build"/>
        public IMonsterSpecies Build()
        {
            if (_name is null || _info is null /*|| _type is null*/ ||
                _evolution is not null && !_evolutionLevel.HasValue && !_gameItem.HasValue /*||
            _movesList.IsEmpty()*/)
            {
                throw new IncompleteInitialization();
            }

            if (_evolution is null)
            {
                return new MonsterSpeciesSimple(_name, _info, /*_type,*/ _stats /*, _movesList*/);
            }

            if (_evolutionLevel.HasValue)
            {
                return new MonsterSpeciesByLevel(_name, _info, _stats, _evolution,
                    _evolutionLevel.ValueOrFailure() /*, _movesList*/);
            }

            if (_gameItem.HasValue)
            {
                return new MonsterSpeciesByItem(_name, _info, _stats, _evolution,
                    _gameItem.ValueOrFailure() /*, _movesList*/);
            }
            
            throw new IncompleteInitialization();
        }
    }
}