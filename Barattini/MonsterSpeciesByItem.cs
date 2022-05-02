using Pokaiju.Guo.GameItem;
using Pokaiju.Pierantoni;

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
        /// <param name="type">type</param>
        /// <param name="stats">stats</param>
        /// <param name="evolutionItem">evolutionItem</param>
        /// <param name="evolution">evolution</param>
        /// <param name="movesList">movesList</param>
        public MonsterSpeciesByItem(string name, string info, MonsterType type, IMonsterStats stats,
            IMonsterSpecies evolution, IGameItem evolutionItem, IList<IMoves> movesList)
            : base(name, info, type, stats, Option.Some(evolution), EvolutionType.Item , movesList)
        {
            _evolutionItem = evolutionItem;
        }
        
        public IGameItem GetItem()
        {
            return _evolutionItem;
        }
    }
}