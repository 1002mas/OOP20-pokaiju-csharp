namespace Pokaiju.Pierantoni;

public interface IMoves
{
    /**
     * 
     * this function returns the name of the move.
     * 
     * @return Move's name
     */
    string GetName();

    /**
     * 
     * this function returns the basic attack of the move.
     * 
     * @return Move's basic attack
     */
    int GetBase();

    /**
     * 
     * this function returns the type of the move.
     * 
     * @return Move's type
     */
    MonsterType GetMonsterType();

    /**
     * 
     * this function returns the max value of PP of the move.
     * 
     * @return Move's max PP
     */
    int GetPp();
    /**
     * 
     *  this function returns additional damage based on enemy's type and move's type.
     * 
     * @param type enemy's type
     * 
     * @return additional damage
     */
    int GetDamage(MonsterType type);
}