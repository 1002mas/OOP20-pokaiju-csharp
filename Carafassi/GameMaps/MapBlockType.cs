using System.Reflection;

namespace Pokaiju.Carafassi.GameMaps
{
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
    class MapBlockAttr : Attribute
    {
        internal MapBlockAttr(bool canPass, bool canMonstersSpawn)
        {
            CanPass = canPass;
            CanMonstersSpawn = canMonstersSpawn;
        }

        public bool CanPass { get; }
        public bool CanMonstersSpawn { get; }
    }

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
            MapBlockAttr blockAttr = GetBlockAttr(block);
            return blockAttr.CanPass;
        }

        /// <summary>
        /// It returns true if it is an area where wild monsters may spawn.
        /// </summary>
        public static bool CanMonstersSpawn(this MapBlockType block)
        {
            MapBlockAttr blockAttr = GetBlockAttr(block);
            return blockAttr.CanMonstersSpawn;
        }

        private static MapBlockAttr GetBlockAttr(MapBlockType block)
        {
            MapBlockAttr? mapBlockAttr = (MapBlockAttr?)
                Attribute.GetCustomAttribute(ForValue(block), typeof(MapBlockAttr));
            if (mapBlockAttr is null)
            {
                throw new ArgumentNullException();
            }

            return mapBlockAttr;
        }

        private static MemberInfo ForValue(MapBlockType block)
        {
            string? name = Enum.GetName(typeof(MapBlockType), block);
            if (name is null)
            {
                throw new ArgumentException();
            }

            MemberInfo? memberInfo = typeof(MapBlockType).GetField(name);
            if (memberInfo is null)
            {
                throw new ArgumentException();
            }

            return memberInfo;
        }
    }
}