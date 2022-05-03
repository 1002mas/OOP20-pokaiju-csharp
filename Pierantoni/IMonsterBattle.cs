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
    
    /// <summary>
    /// this function return if the battle is over.
    /// </summary>
    /// <returns>true if the battle is over, false otherwise</returns>
    bool IsOver();
 
    /// <summary>
    /// this function changes the monster that is fighting with another available.
    /// </summary>
    /// <param name="index">the position in the list of the player's monster</param>
    /// <returns>true if the change has been made, false otherwise</returns>
    bool PlayerChangeMonster(int index);
    
    /// <summary>
    /// this function tries to use a move of the monster that is fighting.
    /// </summary>
    /// <param name="moveIndex">the position in the list of the monster's moves</param>
    /// <returns>true if the move can be made, false otherwise</returns>
    bool MovesSelection(int moveIndex);
    
    /// <summary>
    /// this function return if the monster that is fighting is alive.
    /// </summary>
    /// <returns>true if the monster is alive, false otherwise</returns>
    bool IsCurrentMonsterAlive();
    
    /// <summary>
    /// this function tries to use an item on a player's monster.
    /// </summary>
    /// <param name="item">the item that the player wants to use</param>
    /// <returns>true if the item has been used, false otherwise</returns>
    bool UseItem(IGameItem item);
    
    /// <summary>
    /// the function return the player's monster that is fighting.
    /// </summary>
    /// <returns>the player's monster that is fighting</returns>
    IMonster GetCurrentPlayerMonster();
    
    /// <summary>
    /// the function return the enemy's monster that is fighting.
    /// </summary>
    /// <returns>the enemy's monster that is fighting</returns>
    IMonster GetCurrentEnemyMonster();
    
    /// <summary>
    /// the function return the player who is playing.
    /// </summary>
    /// <returns>the player who is playing</returns>
    IPlayer GetPlayer();
    
    /// <summary>
    /// this function return the enemy trainer (Npc).
    /// </summary>
    /// <returns>the enemy trainer</returns>
    Option<INpcTrainer> GetNpcEnemy();
    
    /// <summary>
    /// the function return if the player's monster finished the PP in all moves.
    /// </summary>
    /// <returns>true the PPs are over, false otherwise</returns>
    bool IsOverOfPp();
    
    /// <summary>
    /// the function tries to attack the enemy monster with the extra move.
    /// </summary>
    /// <returns>true the monster did it, false otherwise</returns>
    bool AttackWithExtraMove();
    
    /// <summary>
    /// the function return if all the player's monsters are death.
    /// </summary>
    /// <returns>true the player has lost, false otherwise</returns>
    bool HasPlayerLost();
    
    /// <summary>
    /// this function set the player data after the battle and restore the monsters.
    /// </summary>
    void EndingBattle();
}