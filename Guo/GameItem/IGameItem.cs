using Pokaiju.Barattini;

namespace Pokaiju.Guo.GameItem;

public interface IGameItem
{
    /// <summary>
    ///     This function returns the name of GameItem.
    /// </summary>
    string GetNameItem();

    /// <summary>
    ///     This function returns the type of GameItem.
    /// </summary>
    GameItemTypes GetGameType ();

    /// <summary>
    ///     This function returns GameItem's Description.
    /// </summary>
    string GetDescription ();

    /// <summary>
    ///     This function returns if GameItem is used.
    ///     <param name="m">The monster which you want to use Item for</param>
    /// </summary>
    bool Use(IMonster? m);
}