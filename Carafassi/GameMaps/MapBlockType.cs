using System.Reflection;

namespace Pokaiju.Carafassi.GameMaps
{
    public enum MapBlockType
    {
        [MapBlockAttr(true, false)] MapChange,
        [MapBlockAttr(false, false)] Obstacle,
        [MapBlockAttr(true, false)] Walk,
        [MapBlockAttr(true, true)] Wild
    }

    class MapBlockAttr : Attribute
    {
        internal MapBlockAttr(bool canPass, bool canMonstersSpawn)
        {
            CanPass = canPass;
            CanMonstersSpawn = canMonstersSpawn;
        }

        public bool CanPass { get; private set; }
        public bool CanMonstersSpawn { get; private set; }
    }

    public static class MapBlockTypes
    {
        public static bool CanPassThrough(this MapBlockType block)
        {
            MapBlockAttr blockAttr = GetBlockAttr(block);
            return blockAttr.CanPass;
        }

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