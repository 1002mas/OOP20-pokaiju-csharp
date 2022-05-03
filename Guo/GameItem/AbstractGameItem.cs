using Pokaiju.Barattini;

namespace Pokaiju.Guo.GameItem;

public abstract class AbstractGameItem : IGameItem
{
    private readonly string _description;
    private readonly string _nameItem;
    private readonly GameItemTypes _type;

    /**
     * @param nameItem
     * @param description
     * @param type
     */
    protected AbstractGameItem(string nameItem, string description, GameItemTypes type)
    {
        _nameItem = nameItem;
        _description = description;
        _type = type;
    }

    /// <inheritdoc cref="IGameItem.GetNameItem" />
    public string GetNameItem()
    {
        return _nameItem;
    }


    public string GetDescription()
    {
        return _description;
    }


    public GameItemTypes GetGameType()
    {
        return _type;
    }


    public abstract bool Use(IMonster m);


    protected bool Equals(AbstractGameItem other)
    {
        return _nameItem == other._nameItem && _type == other._type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AbstractGameItem) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_nameItem, (int) _type);
    }

    public override string ToString()
    {
        return "nameItem=" + _nameItem + ", description=" + _description + ", type=" + _type;
    }
}