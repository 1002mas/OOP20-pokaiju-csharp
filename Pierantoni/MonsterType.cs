using System.Reflection;

namespace Pokaiju.Pierantoni;

[AttributeUsage(AttributeTargets.All)]
internal class MonsterTypeAttribute : Attribute
{
    internal MonsterTypeAttribute(string name)
    {
        Name = name;
        var path = "res/data/" + Name + ".dat";
        try {
            foreach (var line in File.ReadLines(path))
            {
                var splittedLine = line.Split(" ");
                DamageMultiplier.Add(splittedLine[0], double.Parse(splittedLine[1]));
            }
        } catch (FileNotFoundException e) {
            Console.WriteLine(e.StackTrace);
        } catch (IOException e1) {
            Console.WriteLine(e1.StackTrace);
        }
    }
    public string Name { get; }
    public readonly IDictionary<string, double> DamageMultiplier = new Dictionary<string, double>();
}

public  static class MonsterTypes
{

    private static MonsterTypeAttribute GetAtt(MonsterType type)
    {
        var res =  Attribute.GetCustomAttribute(ForValue(type), typeof(MonsterTypeAttribute));
        if (res  is not null)
        {
            return (MonsterTypeAttribute) res;
        }

        throw new InvalidOperationException();


    }
    
    private static MemberInfo ForValue(MonsterType type)
    {
        var name = Enum.GetName(typeof(MonsterType), type);
        if (name is null) throw new InvalidOperationException();
        var info = typeof(MonsterType).GetField(name);
        if (info is not null)
        {
            return info;
        }

        throw new InvalidOperationException();



    }

    public static double ResistanceTo(this MonsterType type, MonsterType enemyType) {
        return 1 / GetAtt(type).DamageMultiplier[GetAtt(enemyType).Name];
    }

    public static double DamageTo(this MonsterType type, MonsterType enemyType) {
        return GetAtt(type).DamageMultiplier[GetAtt(enemyType).Name];
    }
}
public enum MonsterType
{
    /***
     * No Type.
     */
    [MonsterTypeAttribute("none")] None,
    /***
     * Fire Type.
     */
    [MonsterTypeAttribute("fire")]Fire,
    /***
     * Grass type.
     */
    [MonsterTypeAttribute("grass")]Grass,
    /***
     * water type.
     */
    [MonsterTypeAttribute("water")]Water

   
}