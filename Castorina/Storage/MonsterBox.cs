using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Storage
{
    public class MonsterBox : IMonsterBox
    {
        
        private readonly string _name;
        private readonly int _boxSize;
        private readonly IList<IMonster> _monsterList;
 
        /// <summary>
        /// Constructor for MonsterBox.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boxSize"></param>
        public MonsterBox(string name, int boxSize)
        {
            this._name = name;
            this._boxSize = boxSize;
            this._monsterList = new List<IMonster>();
        }

        /// <summary>
        /// Constructor for MonsterBox.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="monsters"></param>
        /// <param name="boxSize"></param>
        public MonsterBox( string name, IList<IMonster> monsters, int boxSize) : this(name, boxSize)
        {
            foreach (IMonster monster in monsters) 
            {
                AddMonster(monster);
            }
        }
        
        /// <inheritdoc cref="IMonsterBox.GetAllMonsters"/>
        public IList<IMonster> GetAllMonsters()
        {
            IList<IMonster> monsterList = new List<IMonster>();
            foreach (IMonster monster in this._monsterList) 
            {
                monsterList.Add(monster);
            }
            return monsterList;
        }
        
        /// <inheritdoc cref="IMonsterBox.AddMonster"/>
        public bool AddMonster(IMonster monster)
        {
            if (!IsFull())
            {
                this._monsterList.Add(monster);
                return true;
            }
            return false;
        }
        
        /// <inheritdoc cref="IMonsterBox.GetMonster"/>
        public Option<IMonster> GetMonster(int monsterId)
        {
            foreach (IMonster monster in this._monsterList) 
            {
                if (monster.Id == monsterId)
                {
                    return Option.Some(monster);
                }
            }
            return Option.None<IMonster>();
        }
        
        /// <inheritdoc cref="IMonsterBox.IsFull"/>
        public bool IsFull()
        {
            return this._monsterList.Count >= this._boxSize;
        }
        
        /// <inheritdoc cref="IMonsterBox.Exchange"/>
        public Option<IMonster> Exchange(IMonster toBox,int monsterId) 
        {
            Option<IMonster> monsterInBox = GetMonster(monsterId);
            if (monsterInBox.HasValue)
            {
                this._monsterList.Remove(monsterInBox.ValueOrFailure());
                AddMonster(toBox);
            }
            return monsterInBox;
        }
        
        /// <inheritdoc cref="IMonsterBox.GetName"/>
        public string GetName()
        {
            return this._name;
        }
        
        /// <inheritdoc cref="IMonsterBox.RemoveMonster"/>
        public void RemoveMonster(int monsterId)
        {
            Option<IMonster> monsterInBox = GetMonster(monsterId);
            if (monsterInBox.HasValue)
            {
                //this._monsterList.Remove(monsterInBox.get());
            }

        }
    }
}

