namespace Pokaiju.Barattini;

using Pierantoni;
using Optional;

public interface IMonsterSpecies
{
    /// <summary>
    /// This function returns monster's name.
    /// </summary>
    /// <returns>monster name</returns>
    string GetName();

    /// <summary>
    /// This function returns all the info of the monster.
    /// </summary>
    /// <returns>info</returns>
    string GetInfo();

    /// <summary>
    /// This function returns monster's type.
    /// </summary>
    /// <returns>monster type</returns>
    MonsterType GetMonsterType();

    /// <summary>
    /// This function returns monster's statistics.
    /// </summary>
    /// <returns>monster statistics</returns>
    IMonsterStats GetBaseStats();

    /// <summary>
    /// This function returns the evolution type.
    /// </summary>
    /// <returns>evolution type</returns>
    EvolutionType GetEvolutionType();

    /// <summary>
    /// This function returns the evolution of the current monster.
    /// </summary>
    /// <returns>next evolution monster</returns>
    Option<IMonsterSpecies> GetEvolution();

    /// <summary>
    /// This function returns a list of all learnable moves.
    /// </summary>
    /// <returns>a list of all learnable moves</returns>
    IList<IMoves> GetAllLearnableMoves();
}