namespace Pokaiju.Barattini;

using Pierantoni;
using Microsoft.VisualBasic.CompilerServices;

public interface IMonsterBuilder
{
    /// <summary>
    /// Build monster level.
    /// </summary>
    /// <param name="level">level</param>
    /// <exception cref="ArgumentException">if level is under minimum level or over max level</exception>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Level(int level);

    /// <summary>
    /// Build monster health.
    /// </summary>
    /// <param name="health">health</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Health(int health);

    /// <summary>
    /// Build monster attack.
    /// </summary>
    /// <param name="atk">attack</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Attack(int atk);

    /// <summary>
    /// Build monster defense
    /// </summary>
    /// <param name="dfs">defense</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Defense(int dfs);

    /// <summary>
    /// Build monster speed.
    /// </summary>
    /// <param name="spd">speed</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Speed(int spd);

    /// <summary>
    /// Build monster's experience.
    /// </summary>
    /// <param name="exp">experience</param>
    /// <exception cref="ArgumentException">if experience is under zero or over experience cap</exception>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Exp(int exp);

    /// <summary>
    /// Build monster isWild parameter.
    /// </summary>
    /// <param name="isWild">isWild</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Wild(bool isWild);

    /// <summary>
    /// Build monster's moves.
    /// </summary>
    /// <param name="movesList">movesList</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder MovesList(IList<Tuple<IMoves, int>> movesList);

    /// <summary>
    /// Build monster's species.
    /// </summary>
    /// <param name="species">species</param>
    /// <returns>IMonsterBuilder</returns>
    IMonsterBuilder Species(IMonsterSpecies species);

    /// <summary>
    /// Build monster.
    /// </summary>
    /// <exception cref="IncompleteInitialization">when obligatory parameters are missing</exception>
    /// <returns>IMonster</returns>
    IMonster Build();
}