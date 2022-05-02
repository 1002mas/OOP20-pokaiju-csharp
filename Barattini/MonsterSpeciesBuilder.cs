namespace Pokaiju.Barattini
{
    using Microsoft.VisualBasic.CompilerServices;
    using Optional;
    using Optional.Unsafe;

    public class MonsterSpeciesBuilder : IMonsterSpeciesBuilder
    {
        private string _name;

        private string _info;

        //private IMonsterType _type;
        private Option<int> _evolutionLevel = Option.None<int>();
        private readonly IMonsterStats _stats = new MonsterStats(1, 1, 1, 1);

        private IMonsterSpecies _evolution;
        //private Option<IGameItem> _gameItem = Option.None<IGameItem>();
        //private List<IMoves> _movesList = new ArrayList<IMoves>();

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Name"/>
        public IMonsterSpeciesBuilder Name(string name)
        {
            this._name = name;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Info"/>
        public IMonsterSpeciesBuilder Info(string info)
        {
            this._info = info;
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
            this._evolution = evolution;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.EvolutionLevel"/>
        public IMonsterSpeciesBuilder EvolutionLevel(int level)
        {
            this._evolutionLevel = Option.Some(level);
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Health"/>
        public IMonsterSpeciesBuilder Health(int health)
        {
            this._stats.Health = health;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Attack"/>
        public IMonsterSpeciesBuilder Attack(int attack)
        {
            this._stats.Attack = attack;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Defense"/>
        public IMonsterSpeciesBuilder Defense(int defense)
        {
            this._stats.Defense = defense;
            return this;
        }

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Speed"/>
        public IMonsterSpeciesBuilder Speed(int speed)
        {
            this._stats.Speed = speed;
            return this;
        }

        /*/// <inheritdoc cref="IMonsterSpeciesBuilder.GameItem"/>
         public IMonsterSpeciesBuilder GameItem(IGameItem gameItem)
        {
            this._gameItem = Option.Some(gameItem);
            return this;
        }*/

        /*/// <inheritdoc cref="IMonsterSpeciesBuilder.MovesList"/>
         public IMonsterSpeciesBuilder MovesList(List<Moves> movesList)
        {
            this._movesList = movesList;
            return this;
        }*/

        /// <inheritdoc cref="IMonsterSpeciesBuilder.Build"/>
        public IMonsterSpecies Build()
        {
            if (_name == null || this._info == null /*|| this._type == null*/ ||
                this._evolution != null && !this._evolutionLevel.HasValue /*&& !this._gameItem.HasValue ||
            this._movesList.IsEmpty()*/)
            {
                throw new IncompleteInitialization();
            }

            if (this._evolutionLevel.HasValue)
            {
                return new MonsterSpeciesByLevel(this._name, this._info, this._stats, this._evolution,
                    this._evolutionLevel.ValueOrFailure() /*, movesList*/);
            }

            /*if (this._gameItem.HasValue())
            {
                return new MonsterSpeciesByItem(this._name, this._info, this._stats, this._evolution, this._gameItem.ValueOrFailure(), this.movesList);
            }*/
            return new MonsterSpeciesSimple(this._name, this._info, /*this._type,*/ this._stats /*, this._movesList*/);
        }
    }
}