namespace Pokaiju.Carafassi.GameMaps;

/// <summary>
/// MapBlockType represents the map blocks accessibility.
/// </summary>
public enum MapBlockType
{
    /// <summary>
    /// It's an area where you can change map.
    /// </summary>
    [MapBlockAttr(true, false)] MapChange,

    /// <summary>
    /// It's an area where you can't walk.
    /// </summary>
    [MapBlockAttr(false, false)] Obstacle,

    /// <summary>
    /// It's an area where you can walk.
    /// </summary>
    [MapBlockAttr(true, false)] Walk,

    /// <summary>
    /// It's an area where random monster may appears.
    /// </summary>
    [MapBlockAttr(true, true)] Wild
}

/// <summary>
/// MapBlockTypeAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal sealed class MapBlockAttr : Attribute
{
    internal MapBlockAttr(bool canPass, bool canMonstersSpawn)
    {
        CanPass = canPass;
        CanMonstersSpawn = canMonstersSpawn;
    }

    public bool CanPass { get; }
    public bool CanMonstersSpawn { get; }
}