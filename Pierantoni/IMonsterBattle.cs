using Optional;
using Pokaiju.Barattini;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Pierantoni;

public interface IMonsterBattle
{
    
    /// <summary>
    /// this function return a move that the enemy can do.
    /// </summary>
    /// <returns>an enemy's move</returns>
    IMoves EnemyAttack();

    
    /// <summary>
    /// this function tries to finish the battle.
    /// </summary>
    /// <returns>true if the player escaped, false otherwise</returns>
    bool Escape();
 
    /// <summary>
    /// this function tries to capture the enemy.
    /// </summary>
    /// <returns>true if the enemy has been captured, false otherwise</returns>
    bool Capture();

    /***
     * 
     * this function return if the battle is over.
     * 
     * @return true if the battle is over, false otherwise
     */
    /// <summary>
    /// this function return if the battle is over.
    /// </summary>
    /// <returns>true if the enemy has been captured, false otherwise</returns>
    bool IsOver();

    /***
     * 
     * this function changes the monster that is fighting with another available.
     * 
     * @param index the position in the list of the player's monster
     * 
     * @return true if the change has been made, false otherwise
     */
    bool PlayerChangeMonster(int index);

    /***
     * 
     * this function tries to use a move of the monster that is fighting.
     * 
     * @param moveIndex the position in the list of the monster's moves
     * 
     * @return true if the move can be made, false otherwise
     */
    bool MovesSelection(int moveIndex);

    /***
     * 
     * this function return if the monster that is fighting is alive.
     * 
     * @return true if the monster is alive, false otherwise
     */
    bool IsCurrentMonsterAlive();

    /***
     * 
     * this function tries to use an item on a player's monster.
     * 
     * @param item the item that the player wants to use
     * 
     * @return true if the item has been used, false otherwise
     */
    bool UseItem(IGameItem item);

    /***
     * 
     * the function return the player's monster that is fighting.
     * 
     * @return the player's monster that is fighting
     */
    IMonster GetCurrentPlayerMonster();

    /***
     * 
     * the function return the enemy's monster that is fighting.
     * 
     * @return the enemy's monster that is fighting
     */
    IMonster GetCurrentEnemyMonster();

    /***
     * 
     * the function return the player who is playing.
     * 
     * @return the player who is playing
     */
    IPlayer GetPlayer();

    /***
     * 
     * this function return the enemy trainer (Npc).
     * 
     * @return the enemy trainer
     */
    Option<INpcTrainer> GetNpcEnemy();

    /***
     * 
     * the function return if the player's monster finished the PP in all moves.
     * 
     * @return true the PPs are over, false otherwise
     */
    bool IsOverOfPp();

    /***
     * 
     * the function tries to attack the enemy monster with the extra move.
     * 
     * @return true the monster did it, false otherwise
     */
    bool AttackWithExtraMove();

    /***
     * 
     * the function return if all the player's monsters are death.
     * 
     * @return true the player has lost, false otherwise
     */
    bool HasPlayerLost();

    /***
     * 
     * this function set the player data after the battle and restore the monsters.
     * 
     */
    void EndingBattle();
}