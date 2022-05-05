namespace Pokaiju.Barattini;

public interface IMonsterStats
{
    /// <summary>
    /// Getter and setter for monster health
    /// </summary>
    int Health { get; set; }

    /// <summary>
    /// Getter and setter for monster attack
    /// </summary>
    int Attack { get; set; }

    /// <summary>
    /// Getter and setter for monster defense
    /// </summary>
    int Defense { get; set; }

    /// <summary>
    /// Getter and setter for monster speed
    /// </summary>
    int Speed { get; set; }

    /// <summary>
    /// This function returns monster statistics as map.
    /// </summary>
    /// <returns>statistics map</returns>
    IDictionary<string, int> GetStatsAsMap();
}