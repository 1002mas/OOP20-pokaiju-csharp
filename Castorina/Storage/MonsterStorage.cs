using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Storage
{
    public class MonsterStorage 
    {
        private const int MaxNumberOfBox = 10;
        private const int MaxSizeOfBox = 10;
        private const string InitialBoxName = "BOX";

        private IList<IMonsterBox> _monsterBoxes;
        private int _currentMonsterBoxIndex;
        private IPlayer _player;

        public MonsterStorage(IPlayer player, IList<IMonsterBox> boxes) 
        {
            this._player = player;
            this._monsterBoxes = new List<IMonsterBox>(boxes);
            this._currentMonsterBoxIndex = 0;
            if (this._monsterBoxes.Count > MaxNumberOfBox)
            {
                this._monsterBoxes = this._monsterBoxes.subList(0, MaxNumberOfBox);
            }
            else
            {
                GenerateBoxs(this._monsterBoxes.Count);
            }
        }

        public MonsterStorage(IPlayer player) : this(player, new List<IMonsterBox>())
        {
            throw new NotImplementedException();
        }


        private void GenerateBoxs(int n)
        {
            IMonsterBox box;
            for (int i = n; i < MaxNumberOfBox; i++)
            {
                box = new MonsterBox(InitialBoxName + (i + 1), MaxSizeOfBox);
                this._monsterBoxes.Add(box);
            }
        }

        private void CalculateNewIndex(int n)
        {
            this._currentMonsterBoxIndex = ((this._currentMonsterBoxIndex + n) % MaxSizeOfBox + 
                                            MaxSizeOfBox) % MaxSizeOfBox;
        }

        private IMonsterBox GetFirstBoxFree()
        {
            for (int i = 0; i < MaxNumberOfBox; i++)
            {
                if (!this._monsterBoxes.ElementAt(i).IsFull())
                {
                    return this._monsterBoxes.ElementAt(i);
                }

            }
            return null;
        }

        
        public bool AddMonster(IMonster monster)
        {
            if (this.GetCurrentBox().AddMonster(monster))
            {
                return true;
            }
            else
            {
                IMonsterBox monsterBox = GetFirstBoxFree();
                if (monsterBox != null)
                {
                    return monsterBox.AddMonster(monster);
                }
            }

            return false;
        }
        
        public bool DdepositMonster(IMonster monster)
        {
            if (this._player.GetAllMonsters().Count > 1 && this._player.GetAllMonsters().Contains(monster)
                                                        && GetCurrentBox().AddMonster(monster))
            {

                this._player.RemoveMonster(monster);
                return true;

            }

            return false;
        }

        public bool WithdrawMonster(int monsterId)
        {
            if (IsInBox(monsterId) && !this._player.IsTeamFull)
            {
                this._player.AddMonster(GetCurrentBox().GetMonster(monsterId).ValueOrFailure());
                GetCurrentBox().RemoveMonster(monsterId);
                return true;

            }

            return false;
        }

        private bool IsInBox(int monsterId)
        {
            foreach (IMonster monster in GetCurrentBox().GetAllMonsters()) 
            {
                if (monster.Id == monsterId)
                {
                    return true;
                }
            }
            return false;
        }

        private IMonsterBox GetCurrentBox()
        {
            return this._monsterBoxes.ElementAt(_currentMonsterBoxIndex);
        }

        public bool Exchange(IMonster monster, int monsterId) {
            if (IsInBox(monsterId))
            {
                Option<IMonster> m = GetCurrentBox().Exchange(monster, monsterId);
                if (m.HasValue)
                {
                    this._player.AddMonster(m.ValueOrFailure());
                    this._player.RemoveMonster(monster);
                    return true;
                }
            }

            return false;
        }

        public void NextBox()
        {
            CalculateNewIndex(1);
        }

        public void PreviousBox()
        {
            CalculateNewIndex(-1);
        }

        public string GetCurrentBoxName()
        {
            return GetCurrentBox().GetName();
        }
        public IList<IMonster> GetCurrentBoxMonsters()
        {
            return GetCurrentBox().GetAllMonsters();
        }

        public int GetMaxSizeOfBox()
        {
            return MaxSizeOfBox;
        }

        public int GetMaxNumberOfBox()
        {
            return MaxNumberOfBox;
        }

     
        public int GetCurrentBoxSize()
        {
            return this.GetCurrentBoxMonsters().Count();
        }

    }
}

