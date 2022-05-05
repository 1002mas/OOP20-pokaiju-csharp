namespace Pokaiju.Guo.GameItem;
using Barattini;

public abstract class AbstractGameItem : IGameItem
{
    private readonly string _description;
    private readonly string _nameItem;
    private readonly GameItemTypes _type;

    /// <summary>
    ///     <param name="nameItem">Name of gameItem</param>
    ///     <param name="description">Description of gameItem</param>
    ///     <param name="type">Type of gameItem</param>
    /// </summary>
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

    /// <inheritdoc cref="IGameItem.GetDescription" />
    public string GetDescription()
    {
        return _description;
    }

    /// <inheritdoc cref="IGameItem.GetGameType" />
    public GameItemTypes GetGameType()
    {
        return _type;
    }

    /// <inheritdoc cref="IGameItem.Use" />
    public abstract bool Use(IMonster? m);


    protected bool Equals(AbstractGameItem other)
    {
        return _nameItem == other._nameItem && _type == other._type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
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