using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;

namespace Pokaiju.Castorina.Storage
{
    public class MonsterBox : IMonsterBox
    {
        private readonly string _name;
        private int _boxSize;
        private IList<IMonster> _monsterList;
 
        /// <summary>
        /// Constructor for MonsterBoxImpl.
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
        /// Constructor for MonsterBoxImpl.
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

        public IList<IMonster> GetAllMonsters()
        {
            IList<IMonster> monsterList = new List<IMonster>();
            foreach (IMonster monster in this._monsterList) 
            {
                monsterList.Add(monster);
            }
            return monsterList;
        }

        public bool AddMonster(IMonster monster)
        {
            if (!IsFull())
            {
                this._monsterList.Add(monster);
                return true;
            }
            return false;
        }

        public Option<IMonster> GetMonster(int monsterId)
        {
            foreach (IMonster monster in this._monsterList) 
            {
                if (monster.Id == monsterId)
                {
                    return Option.Some<IMonster>(monster);
                }
            }
            return Option.None<IMonster>();
        }

        public bool IsFull()
        {
            return this._monsterList.Count >= this._boxSize;
        }

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

        public string GetName()
        {
            return this._name;
        }

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

