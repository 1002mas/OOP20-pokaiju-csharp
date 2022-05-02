using Guo.Player;
using Pokaiju.Barattini;
using Pokaiju.Guo.GameItem;

namespace Pokaiju.Guo.Player;

public interface IPlayer
{
    /// <summary>
    ///     This function returns if team is full or not.
    /// </summary>
    bool IsTeamFull { get; }

    /// <summary>
    ///     This function returns if Player has changed Map after movement.
    /// </summary>
    bool HasPlayerChangedMap { get; }

    /**
     * This function returns if the player has triggered an event.
     * 
     * @return true if the player has triggered an event
     */
    bool IsTriggeredEvent { get; }

    /// <summary>
    ///     This function returns all player's monsters.
    /// </summary>
    List<Monster> GetAllMonsters();


    /// <summary>
    ///     This function returns all player's GameItems
    /// </summary>
    Dictionary<IGameItem, int> GetAllItems();

    /// <summary>
    ///     This function returns Player's name.
    /// </summary>
    string GetName();

    /// <summary>
    ///     This function returns Player's TrainerNumber.
    /// </summary>
    int GetTrainerNumber();

    /// <summary>
    ///     This function returns Player's gender.
    /// </summary>
    Gender GetGender();

    /// <summary>
    ///     This function returns Player's Money.
    /// </summary>
    int GetMoney();

    /// <summary>
    ///     This function returns the position of the Player.
    ///     @return Player's position
    /// </summary>
    Tuple<int, int> GetPosition();

    /// <summary>
    ///     This function adds new GameItem to player's bag.
    ///     <param name="i">The GameItem which will be added to player's bag</param>
    /// </summary>
    void AddItem(IGameItem i);

    /// <summary>
    ///     This function adds new GameItems to player's bag.
    ///     <param name="i">The GameItem which will be added to player's bag</param>
    ///     <param name="quantity">Quantity of GameItem</param>
    /// </summary>
    void AddItem(IGameItem i, int quantity);

    /// <summary>
    ///     This function returns GameItem's quantity.
    ///     <param name="gameItem"> The GameItem which want to know quantity</param>
    /// </summary>
    int GetItemQuantity(IGameItem gameItem);

    /// <summary>
    ///     This function removes GameItem from player's bag.
    ///     < param name="gameItem"> The GameItem which want to remove </param>
    /// </summary>
    void RemoveItem(IGameItem gameItem);

    /// <summary>
    ///     This function uses a GameItem.
    ///     <param name="gameItem">The GameItem which want to use</param>
    /// </summary>
    void UseItem(IGameItem gameItem);

    /// <summary>
    ///     This function uses a GameItem on Monster
    ///     <param name="gameItem">The GameItem which want to use</param>
    ///     <param name="m">The Monster which want to apply a GameItem</param>
    /// </summary>
    void useItemOnMonster(IGameItem gameItem, Monster m);

    /// <summary>
    ///     This function returns if add Monster is successful or not.
    ///     <param name="m">The Monster which will be added to player</param>
    /// </summary>
    bool AddMonster(Monster m);

    /// <summary>
    ///     This function returns if remove Monster is successful or not.
    ///     <param name="m">The Monster which will be removed from player</param>
    /// </summary>
    bool RemoveMonster(Monster m);

    /// <summary>
    ///     This function sets Player's Money.
    ///     <param name="money">Remaining money</param>
    /// </summary>
    void SetMoney(int money);

    /// <summary>
    ///     This function sets Player's position.
    ///     <param name="position">New position</param>
    /// </summary>
    void SetPosition(KeyValuePair<int, int> position);

    /// <summary>
    ///     This function evolves Monsters after a battle.
    /// </summary>
    void EvolveMonsters();

    /// <summary>
    ///     This function evolves Monster using giving GameItem.
    ///     <param name="monster">Monster to be evolved </param>
    ///     <param name="gameItem">Evolution Item to be used</param>
    /// </summary>
    void EvolveMonster(Monster monster, IGameItem gameItem);

    /// <summary>
    ///     Moves Player up.
    /// </summary>
    bool MoveUp();

    /// <summary>
    ///     Moves Player down.
    /// </summary>
    bool MoveDown();

    /// <summary>
    ///     Moves Player left.
    /// </summary>
    bool MoveLeft();

    /// <summary>
    ///     Moves Player right.
    /// </summary>
    bool MoveRight();

    /// <summary>
    ///     This function gets the GameMap.
    /// </summary>
    IGameMap GetMap();


     /// <summary>
     /// This function returns if Player has interaction with Npc.
     
     /// <param name="pos">Interact in the position</param>
     
     /// </summary>
    bool InteractAt(KeyValuePair<int, int> pos);

      /// <summary>
     /// This function returns if there was a interaction with Npc. 
      /// </summary>
    Optional<NpcSimple> GetLastInteractionWithNpc();

    /**
     * This function returns a battle if a wild monster attacked while player was
     * moving or the Player talked with a Npc.
     * 
     * @return a battle if any is present
     */
    Optional<MonsterBattle> GetPlayerBattle();

    /**
     * This function updates storage.
     * 
     * @param storage
     */
    void SetStorage(MonsterStorage storage);

    /**
     * This function gets storage.
     * 
     * @return storage
     */
    MonsterStorage GetStorage();

    /**
     * This function sets player's gender.
     * 
     * @param gender
     */
    void SetGender(Gender gender);

    /**
     * This function sets player's trainerNumber.
     * 
     * @param trainerNumber
     */
    void SetTrainerNumber(int trainerNumber);

    /**
     * This function gets a list of Monsters.
     * 
     * @return list of Monster
     */
    List<Monster> GetMonster();

    /**
     * This function sets a list of Monsters.
     * 
     * @param monster List of monster to set
     */
    void SetMonster(List<Monster> monster);

    /**
     * This function sets a list of GameItems.
     * 
     * @return list of GameItems
     */
    List<IGameItem> GetItems();

    /// <summary>
    ///     This function sets Player's name.
    ///     <param name="name">Set player's name</param>
    /// </summary>
    void SetName(string name);
}