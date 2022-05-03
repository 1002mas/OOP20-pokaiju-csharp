namespace Pokaiju.Pierantoni;

public class Moves : IMoves
{
    private readonly string _name;
    private int _base;
    private readonly MonsterType _type;
    private readonly int _maxPp;

    internal Moves(string name, int baseAtt, MonsterType type, int pp) {
        _name = name;
        _base = baseAtt;
        _type = type;
        _maxPp = pp;

    }

    /***
     * {@inheritDoc}.
     */
    public String GetName() {
        return _name;
    }

    /***
     * {@inheritDoc}.
     */
    public int GetBase() {
        return _base;
    }

    /***
     * {@inheritDoc}.
     */
    public  MonsterType GetMonsterType() {
        return _type;
    }

    /***
     * {@inheritDoc}.
     */
    public int GetPp() {
        return _maxPp;
    }

   
    /***
     * {@inheritDoc}.
     */
    public int GetDamage(MonsterType type) {
        return (int) this._type.DamageTo(type);
    }

    protected bool Equals(Moves other)
    {
        return _name == other._name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Moves) obj);
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }
}