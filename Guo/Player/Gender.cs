namespace Pokaiju.Guo.Player;
using System.Reflection;

class GenderAttr : Attribute
{
    internal GenderAttr(string name)
    {
        _name = name;
    }

    private string _name;
}

public static class GameItemType
{
    private static GenderAttr GetAttr(Gender p)
    {
        var genderAtt = (GenderAttr?) Attribute.GetCustomAttribute(ForValue(p), typeof(GenderAttr));
        if (genderAtt is null) throw new ArgumentNullException();

        return genderAtt;
    }

    private static MemberInfo ForValue(Gender p)
    {
        var name = Enum.GetName(typeof(Gender), p);
        if (name is null) throw new ArgumentException();
        MemberInfo? memberInfo = typeof(Gender).GetField(name);
        if (memberInfo is null) throw new ArgumentException();
        return memberInfo;
    }
}

public enum Gender
{
    [GenderAttr("male")] Man,
    [GenderAttr("female")] Woman
}