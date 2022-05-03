namespace Pokaiju.Pierantoni;

public interface IMoves
{
    
    /// <summary>
    /// this function returns the name of the move.
    /// </summary>
    /// <returns>Move's name</returns>
    string GetName();
    
    /// <summary>
    /// this function returns the basic attack of the move.
    /// </summary>
    /// <returns>Move's basic attack</returns>
    int GetBase();
    
    /// <summary>
    /// this function returns the type of the move.
    /// </summary>
    /// <returns>Move's type</returns>
    MonsterType GetMonsterType();
    
    /// <summary>
    /// this function returns the max value of PP of the move.
    /// </summary>
    /// <returns>Move's max PP</returns>
    int GetPp();
    
    /// <summary>
    /// this function returns additional damage based on enemy's type and move's type.
    /// </summary>
    /// <param name="type">type enemy's type</param>
    /// <returns>additional damage</returns>
    int GetDamage(MonsterType type);
}