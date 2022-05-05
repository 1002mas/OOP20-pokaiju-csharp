using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Storage;

public interface IMonsterStorage
{

    /// <summary>
    /// This function adds a monster in storage if current box isn't full.
    /// </summary>
    /// <param name="monster"></param>
    /// <returns>true if monster is added, false otherwise</returns>
    bool AddMonster(IMonster monster);

    /// <summary>
    /// This function tries to deposit a monster in the current monster box and to
    /// remove the monster form player team.
    /// </summary>
    /// <param name="monster"></param>
    /// <returns>true if is deposited, false otherwise</returns>
    bool DepositMonster(IMonster monster);

    /// <summary>
    /// This function tries to remove a monster from the current box and to add it
    /// in player team
    /// </summary>
    /// <param name="monsterId"></param>
    /// <returns>true if monster has been added in player team and removed from box,
    /// false otherwise</returns>
    bool WithdrawMonster(int monsterId);

    /// <summary>
    /// This function tries to exchange two monsters, one from player team to
    /// storage and one from the storage to player team.
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="monsterId"></param>
    /// <returns>true if the exchange was completed, false otherwise</returns>
    bool Exchange(IMonster monster, int monsterId);


    /// <summary>
    /// This function changes current box with the next one.
    /// </summary>
    void NextBox();

    /// <summary>
    /// This function changes current box with the previous one.
    /// </summary>
    void PreviousBox();

    /// <summary>
    /// This function returns current box name.
    /// </summary>
    /// <returns>box name</returns>
    string GetCurrentBoxName();

    /// <summary>
    /// This function returns the size of current box.
    /// </summary>
    /// <returns>size of current box</returns>
    int GetCurrentBoxSize();

    /// <summary>
    /// This function returns current box monsters list.
    /// </summary>
    /// <returns>monsters list</returns>
    IList<IMonster> GetCurrentBoxMonsters();

    /// <summary>
    /// This function returns maximum number of boxes in storage.
    /// </summary>
    /// <returns>maximum number of a box</returns>
    int GetMaxSizeOfBox();

    /// <summary>
    /// This function returns maximum number of monsters in each box.
    /// </summary>
    /// <returns>maximum number of monsters</returns>
    int GetMaxNumberOfBox();
}

