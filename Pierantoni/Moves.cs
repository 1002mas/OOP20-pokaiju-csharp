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
    public  MonsterType GetType() {
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

}