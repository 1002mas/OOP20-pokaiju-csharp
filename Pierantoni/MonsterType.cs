using System.Reflection;

namespace Pokaiju.Pierantoni;

class MonsterTypeAttribute : Attribute
{
    internal MonsterTypeAttribute(string name)
    {
        Name = name;
        string path = "res/data/" + Name + ".dat";
        Console.Out.WriteLine(File.ReadLines(path));
        try {
            foreach (string line in File.ReadLines(path))
            {
                string[] splittedLine;
                splittedLine = line.Split(" ");
                Console.Out.WriteLine(line);
                DamageMultiplier.Add(splittedLine[0], Double.Parse(splittedLine[1]));
            }
        } catch (FileNotFoundException e) {
            Console.WriteLine(e.StackTrace);
        } catch (IOException e1) {
            Console.WriteLine(e1.StackTrace);
        }
    }
    public String Name { get; }
    public  IDictionary<String, Double> DamageMultiplier = new Dictionary<String, Double>();
}

public  static class MonsterTypes
{

    private static MonsterTypeAttribute GetAtt(MonsterType type)
    {
        Attribute? res =  Attribute.GetCustomAttribute(ForValue(type), typeof(MonsterTypeAttribute));
        if (res  is not null)
        {
            return (MonsterTypeAttribute) res;
        }
        else
        {
            throw new InvalidOperationException();
        }

        
    }
    
    private static MemberInfo ForValue(MonsterType type)
    {
        string? name = Enum.GetName(typeof(MonsterType), type);
        if (name is not null)
        {
            FieldInfo? info = typeof(MonsterType).GetField(name);
            if (info is not null)
            {
                return info;
            }
            else
            {
                throw new InvalidOperationException(); 
            }
        }
        else
        {
            throw new InvalidOperationException(); 
        }
        

        
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