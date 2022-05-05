namespace Pokaiju.Barattini;

using Guo.GameItem;
using Pierantoni;

public interface IMonster
{
    /// <summary>
    /// This function returns the id of the monster.
    /// </summary>
    /// <returns>monster id</returns>
    int Id { get; }

    /// <summary>
    /// This function returns the name of the monster.
    /// </summary>
    /// <returns>monster's name</returns>
    string GetName();

    /// <summary>
    /// This function set the health of the monster.
    /// </summary>
    /// <param name="hlt">health</param>
    void SetHealth(int hlt);

    /// <summary>
    /// This function restore the statistics of the monster.
    /// </summary>
    void RestoreStats();

    /// <summary>
    /// This function returns monster's max health.
    /// </summary>
    /// <returns>monster's max health</returns>
    int GetMaxHealth();

    /// <summary>
    /// This function returns all the info of the monster.
    /// </summary>
    /// <returns>monster's info</returns>
    string GetInfo();

    /// <summary>
    /// This function returns the level of the monster.
    /// </summary>
    /// <returns>monster's level</returns>
    int GetLevel();

    /// <summary>
    /// This function gives one level to the monster.
    /// </summary>
    void LevelUp();

    /// <summary>
    /// This function increase the monster's experience points.
    /// </summary>
    /// <param name="exp">experience</param>
    void IncExp(int exp);

    /// <summary>
    /// This function returns the monster's experience points.
    /// </summary>
    /// <returns>monster's experience</returns>
    int GetExp();

    /// <summary>
    /// This function returns the experience point cap.
    /// </summary>
    /// <returns>experience cap</returns>
    int GetExpCap();

    /// <summary>
    /// This function returns if the monster is wild.
    /// </summary>
    /// <returns>if the monster is wild</returns>
    bool IsWild();

    /// <summary>
    /// This function returns if the monster is alive.
    /// </summary>
    /// <returns>if the monster is alive</returns>
    bool IsAlive();


    /// <summary>
    /// This function returns a move from a list of moves.
    /// </summary>
    /// <param name="index">index of the moves</param>
    /// <exception cref="ArgumentException">if index is below zero or over max number of moves</exception>
    /// <returns>a move</returns>
    IMoves GetMoves(int index);

    /// <summary>
    /// This function returns the list of all moves.
    /// </summary>
    /// <returns>all monster moves</returns>
    IList<IMoves> GetAllMoves();

    /// <summary>
    /// This function returns the current PP of the move.
    /// </summary>
    /// <param name="move">move</param>
    /// <returns>current PP of the move</returns>
    int GetCurrentPpByMove(IMoves move);

    /// <summary>
    /// This function returns if the monster have finished move's PP.
    /// </summary>
    /// <param name="move">move</param>
    /// <returns>if the monster have finished move's PP</returns>
    bool IsOutOfPp(IMoves move);

    /// <summary>
    /// This function restore all PP of a move.
    /// </summary>
    /// <param name="move">move</param>
    void RestoreMovePp(IMoves move);

    /// <summary>
    /// This function restore all PP of all moves.
    /// </summary>
    void RestoreAllMovesPp();

    /// <summary>
    /// This function decrease move's PP.
    /// </summary>
    /// <param name="move">move</param>
    void DecMovePp(IMoves move);

    /// <summary>
    /// This function return true if the move set is full, false otherwise.
    /// </summary>
    /// <returns>if the move set is full</returns>
    bool IsMoveSetFull();

    /// <summary>
    /// This function returns the numbers of moves owned by the monster.
    /// </summary>
    /// <returns>numbers of moves owned by the monster</returns>
    int GetNumberOfMoves();

    /// <summary>
    /// This function returns the type of the monster.
    /// </summary>
    /// <returns>type of the monster</returns>
    MonsterType GetMonsterType();

    /// <summary>
    /// This function returns true if monster evolve by level, false otherwise.
    /// </summary>
    /// <returns>if monster can evolve by level</returns>
    bool CanEvolveByLevel();

    /// <summary>
    /// This function returns if the monster is evolving by item.
    /// </summary>
    /// <param name="item">evolution item</param>
    /// <returns>if monster evolves by item</returns>
    bool CanEvolveByItem(IGameItem item);

    /// <summary>
    /// This function evolve the monster.
    /// </summary>
    void Evolve();

    /// <summary>
    /// This function returns monster's species.
    /// </summary>
    /// <returns>a monsterSpecies</returns>
    IMonsterSpecies GetSpecies();

    /// <summary>
    /// This function returns monster's statistics.
    /// </summary>
    /// <returns>monster's statistics</returns>
    IMonsterStats GetStats();

    /// <summary>
    /// This function returns the max statistics of the monster.
    /// </summary>
    /// <returns>max statistics of the monster</returns>
    IMonsterStats GetMaxStats();
}