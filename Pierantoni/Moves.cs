namespace Pokaiju.Pierantoni;

public class Moves : IMoves
{
    private readonly string _name;
    private readonly int _base;
    private readonly MonsterType _type;
    private readonly int _maxPp;

    internal Moves(string name, int baseAtt, MonsterType type, int pp) {
        _name = name;
        _base = baseAtt;
        _type = type;
        _maxPp = pp;

    }

    /// <inheritdoc cref="IMoves.GetName"/>
    public string GetName() {
        return _name;
    }

    /// <inheritdoc cref="IMoves.GetBase"/>
    public int GetBase() {
        return _base;
    }

    /// <inheritdoc cref="IMoves.GetMonsterType"/>
    public  MonsterType GetMonsterType() {
        return _type;
    }

    /// <inheritdoc cref="IMoves.GetPp"/>
    public int GetPp() {
        return _maxPp;
    }

   
    /// <inheritdoc cref="IMoves.GetDamage"/>
    public int GetDamage(MonsterType type) {
        return (int) _type.DamageTo(type);
    }

    private bool Equals(Moves other)
    {
        return _name == other._name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Moves) obj);
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }
}