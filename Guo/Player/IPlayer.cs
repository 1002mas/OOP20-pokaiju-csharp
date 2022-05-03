using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameMaps;
using Pokaiju.Castorina.Npc;
using Pokaiju.Castorina.Storage;
using Pokaiju.Guo.GameItem;
using Pokaiju.Pierantoni;

namespace Pokaiju.Guo.Player;

public interface IPlayer
{
    /// <summary>
    ///     This function returns if team is full or not.
    /// </summary>
    bool IsTeamFull { get;}

    /// <summary>
    ///     This function returns if Player has changed Map after movement.
    /// </summary>
    bool HasPlayerChangedMap();

    /**
     * This function returns if the player has triggered an event.
     * 
     * @return true if the player has triggered an event
     */
    bool IsTriggeredEvent();

    /// <summary>
    ///     This function returns all player's monsters.
    /// </summary>
    List<IMonster> GetAllMonsters();


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
    void useItemOnMonster(IGameItem gameItem, IMonster m);

    /// <summary>
    ///     This function returns if add Monster is successful or not.
    ///     <param name="m">The Monster which will be added to player</param>
    /// </summary>
    bool AddMonster(IMonster m);

    /// <summary>
    ///     This function returns if remove Monster is successful or not.
    ///     <param name="m">The Monster which will be removed from player</param>
    /// </summary>
    bool RemoveMonster(IMonster m);

    /// <summary>
    ///     This function sets Player's Money.
    ///     <param name="money">Remaining money</param>
    /// </summary>
    void SetMoney(int money);

    /// <summary>
    ///     This function sets Player's position.
    ///     <param name="position">New position</param>
    /// </summary>
    void SetPosition(Tuple<int, int> position);

    /// <summary>
    ///     This function evolves Monsters after a battle.
    /// </summary>
    void EvolveMonsters();

    /// <summary>
    ///     This function evolves Monster using giving GameItem.
    ///     <param name="monster">Monster to be evolved </param>
    ///     <param name="gameItem">Evolution Item to be used</param>
    /// </summary>
    void EvolveMonster(IMonster monster, IGameItem gameItem);

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
    ///     This function returns if Player has interaction with Npc.
    ///     <param name="pos">Interact in the position</param>
    /// </summary>
    bool InteractAt(Tuple<int, int> pos);

    /// <summary>
    ///     This function returns if there was a interaction with Npc.
    /// </summary>
    Option<INpcSimple> GetLastInteractionWithNpc();

    /// <summary>
    ///     This function returns a battle if a wild monster attacked while player was
    ///     moving or the Player talked with a Npc.
    /// </summary>
    Option<IMonsterBattle> GetPlayerBattle();

    /// <summary>
    ///     This function updates storage.
    ///     <param name="storage">Storage to be updated</param>
    /// </summary>
    void SetStorage(MonsterStorage storage);

    /// <summary>
    ///     This function gets storage.
    /// </summary>
    MonsterStorage GetStorage();

    /// <summary>
    ///     This function sets player's gender.
    ///     <param name="gender">Gender to be set</param>
    /// </summary>
    void SetGender(Gender gender);

    /// <summary>
    ///     This function sets player's trainerNumber.
    ///     <param name="trainerNumber">TrainerNumber to be set</param>
    /// </summary>
    void SetTrainerNumber(int trainerNumber);

    /// <summary>
    ///     This function gets a list of Monsters.
    /// </summary>
    List<IMonster> GetMonster();

    /// <summary>
    ///     This function sets a list of Monsters.
    ///     <param name="monster">  List of monster to set</param>
    /// </summary>
    void SetMonster(List<IMonster> monster);

    /// <summary>
    ///     This function gets a list of GameItems.
    /// </summary>
    List<IGameItem> GetItems();

    /// <summary>
    ///     This function sets Player's name.
    ///     <param name="name">Set player's name</param>
    /// </summary>
    void SetName(string name);
}