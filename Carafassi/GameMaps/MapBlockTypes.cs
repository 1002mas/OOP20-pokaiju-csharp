namespace Pokaiju.Carafassi.GameMaps;

using System.Reflection;

/// <summary>
/// MapBlockType method extension
/// </summary>
public static class MapBlockTypes
{
    /// <summary>
    /// It returns true if you can walk on the block.
    /// </summary>
    public static bool CanPassThrough(this MapBlockType block)
    {
        var blockAttr = GetBlockAttr(block);
        return blockAttr.CanPass;
    }

    /// <summary>
    /// It returns true if it is an area where wild monsters may spawn.
    /// </summary>
    public static bool CanMonstersSpawn(this MapBlockType block)
    {
        var blockAttr = GetBlockAttr(block);
        return blockAttr.CanMonstersSpawn;
    }

    private static MapBlockAttr GetBlockAttr(MapBlockType block)
    {
        var mapBlockAttr = (MapBlockAttr?)
            Attribute.GetCustomAttribute(ForValue(block), typeof(MapBlockAttr));
        if (mapBlockAttr is null)
        {
            throw new InvalidOperationException();
        }

        return mapBlockAttr;
    }

    private static MemberInfo ForValue(MapBlockType block)
    {
        var name = Enum.GetName(typeof(MapBlockType), block);
        if (name is null)
        {
            throw new InvalidOperationException();
        }

        var memberInfo = typeof(MapBlockType).GetField(name);
        if (memberInfo is null)
        {
            throw new MissingMemberException();
        }

        return memberInfo;
    }
}