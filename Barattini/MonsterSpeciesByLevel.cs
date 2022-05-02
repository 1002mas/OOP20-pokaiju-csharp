using Pokaiju.Pierantoni;

namespace Pokaiju.Barattini
{
    using Optional;

    public class MonsterSpeciesByLevel : MonsterSpeciesSimple
    {
        private readonly int _evolutionLevel;

        /// <summary>
        /// MonsterSpeciesByLevel constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="info">info</param>
        /// <param name="type">type</param>
        /// <param name="stats">stats</param>
        /// <param name="evolution">evolution</param>
        /// <param name="evolutionLevel">evolutionLevel</param>
        /// <param name="movesList">movesList</param>
        public MonsterSpeciesByLevel(string name, string info, MonsterType type, IMonsterStats stats,
            IMonsterSpecies evolution, int evolutionLevel, IList<IMoves> movesList) 
            : base(name, info, type, stats, Option.Some<>(evolution), EvolutionType.Level, movesList)
        {
            this._evolutionLevel = evolutionLevel;
        }
        
        public int GetEvolutionLevel()
        {
            return this._evolutionLevel;
        }
    }
}


