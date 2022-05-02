using Pokaiju.Guo.GameItem;

namespace Pokaiju.Barattini
{
    using Optional;

    public class MonsterSpeciesByItem : MonsterSpeciesSimple
    {
        private readonly IGameItem _evolutionItem;

        /// <summary>
        /// MonsterSpeciesByItem constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="info">info</param>
        /// <param name="stats">stats</param>
        /// <param name="evolutionItem">evolutionItem</param>
        /// <param name="evolution">evolution</param>
        public MonsterSpeciesByItem(string name, string info, /*IMonsterType type,*/ IMonsterStats stats,
            IMonsterSpecies evolution, IGameItem evolutionItem/*, List<Moves> movesList*/)
            : base(name, info, /*type,*/ stats, Option.Some(evolution), EvolutionType.Item /*, movesList*/)
        {
            _evolutionItem = evolutionItem;
        }
        
        public IGameItem GetItem()
        {
            return _evolutionItem;
        }
    }
}