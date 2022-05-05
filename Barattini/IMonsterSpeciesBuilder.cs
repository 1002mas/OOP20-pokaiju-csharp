namespace Pokaiju.Barattini;

using Guo.GameItem;
using Pierantoni;

using Microsoft.VisualBasic.CompilerServices;

public interface IMonsterSpeciesBuilder
{
    /// <summary>
    /// Build species name.
    /// </summary>
    /// <param name="name">name</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Name(string name);

    /// <summary>
    /// Build species info.
    /// </summary>
    /// <param name="info">info</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Info(string info);

    /// <summary>
    /// Build species type.
    /// </summary>
    /// <param name="type">type</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder MonsterType(MonsterType type);

    /// <summary>
    /// Build species evolution.
    /// </summary>
    /// <param name="evolution">evolution</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Evolution(IMonsterSpecies evolution);

    /// <summary>
    /// Build species evolutionLevel for evolving by level.
    /// </summary>
    /// <param name="level">level</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder EvolutionLevel(int level);

    /// <summary>
    /// Build species health.
    /// </summary>
    /// <param name="health">health</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Health(int health);

    /// <summary>
    /// Build species attack.
    /// </summary>
    /// <param name="attack">attack</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Attack(int attack);

    /// <summary>
    /// Build species defense.
    /// </summary>
    /// <param name="defense">defense</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Defense(int defense);

    /// <summary>
    /// Build species speed.
    /// </summary>
    /// <param name="speed">speed</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder Speed(int speed);

    /// <summary>
    /// Build species item
    /// </summary>
    /// <param name="gameItem">gameItem</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder GameItem(IGameItem gameItem);

    /// <summary>
    /// Build species moves.
    /// </summary>
    /// <param name="movesList">movesList</param>
    /// <returns>IMonsterSpeciesBuilder</returns>
    IMonsterSpeciesBuilder MovesList(IList<IMoves> movesList);

    /// <summary>
    /// MonsterSpecies.
    /// </summary>
    /// <exception cref="IncompleteInitialization">if the obligatory fields are missing</exception>
    /// <returns>IMonsterSpecies</returns>
    IMonsterSpecies Build();
}