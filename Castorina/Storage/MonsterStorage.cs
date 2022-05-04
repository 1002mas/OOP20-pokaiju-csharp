using Optional;
using Optional.Unsafe;
using Pokaiju.Barattini;
using Pokaiju.Guo.Player;

namespace Pokaiju.Castorina.Storage
{
    public class MonsterStorage : IMonsterStorage
    {
        /// <summary>
        /// Max number of monster box
        /// </summary>
        private const int MaxNumberOfBox = 10;
        /// <summary>
        /// max size of a monster box
        /// </summary>
        private const int MaxSizeOfBox = 10;
        /// <summary>
        /// initial box name
        /// </summary>
        private const string InitialBoxName = "BOX";

        private readonly IList<IMonsterBox> _monsterBoxes;
        private int _currentMonsterBoxIndex;
        private readonly IPlayer _player;
        
        /// <summary>
        /// Constructor of MonsterStorage
        /// </summary>
        /// <param name="player"></param>
        /// <param name="boxes"></param>
        public MonsterStorage(IPlayer player, IList<IMonsterBox> boxes) 
        {
            this._player = player;
            this._monsterBoxes = new List<IMonsterBox>(boxes);
            this._currentMonsterBoxIndex = 0;
            if (this._monsterBoxes.Count > MaxNumberOfBox)
            {
                var temp = new List<IMonsterBox>(_monsterBoxes);
                this._monsterBoxes = temp.GetRange(0, MaxNumberOfBox);
            }
            else
            {
                GenerateBoxs(this._monsterBoxes.Count);
            }
        }
        
        /// <summary>
        /// Constructor of MonsterStorage 
        /// </summary>
        /// <param name="player"></param>
        /// <exception cref="NotImplementedException"></exception>
        public MonsterStorage(IPlayer player) : this(player, new List<IMonsterBox>())
        {
          
        }


        private void GenerateBoxs(int n)
        {
            for (int i = n; i < MaxNumberOfBox; i++)
            {
                IMonsterBox box = new MonsterBox(InitialBoxName + (i + 1), MaxSizeOfBox);
                this._monsterBoxes.Add(box);
            }
        }

        private void CalculateNewIndex(int n)
        {
            this._currentMonsterBoxIndex = ((this._currentMonsterBoxIndex + n) % MaxSizeOfBox + 
                                            MaxSizeOfBox) % MaxSizeOfBox;
        }

        private IMonsterBox? GetFirstBoxFree()
        {
            for (var i = 0; i < MaxNumberOfBox; i++)
            {
                if (!this._monsterBoxes.ElementAt(i).IsFull())
                {
                    return this._monsterBoxes.ElementAt(i);
                }

            }
            return null;
        }

        /// <inheritdoc cref="IMonsterStorage.AddMonster"/>
        public bool AddMonster(IMonster monster)
        {
            if (this.GetCurrentBox().AddMonster(monster))
            {
                return true;
            }
            else
            {
                IMonsterBox? monsterBox = GetFirstBoxFree();
                if (monsterBox != null)
                {
                    return monsterBox.AddMonster(monster);
                }
            }

            return false;
        }
        /// <inheritdoc cref="IMonsterStorage.DepositMonster"/>
        public bool DepositMonster(IMonster monster)
        {
            if (this._player.GetAllMonsters().Count > 1 && this._player.GetAllMonsters().Contains(monster)
                                                        && GetCurrentBox().AddMonster(monster))
            {

                this._player.RemoveMonster(monster);
                return true;

            }

            return false;
        }
        /// <inheritdoc cref="IMonsterStorage.WithdrawMonster"/>
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
        /// <inheritdoc cref="IMonsterStorage.Exchange"/>
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
        /// <inheritdoc cref="IMonsterStorage.NextBox"/>
        public void NextBox()
        {
            CalculateNewIndex(1);
        }
        /// <inheritdoc cref="IMonsterStorage.PreviousBox"/>
        public void PreviousBox()
        {
            CalculateNewIndex(-1);
        }
        /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxName"/>
        public string GetCurrentBoxName()
        {
            return GetCurrentBox().GetName();
        }
        /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxMonsters"/>
        public IList<IMonster> GetCurrentBoxMonsters()
        {
            return GetCurrentBox().GetAllMonsters();
        }
        /// <inheritdoc cref="IMonsterStorage.GetMaxSizeOfBox"/>
        public int GetMaxSizeOfBox()
        {
            return MaxSizeOfBox;
        }
        /// <inheritdoc cref="IMonsterStorage.GetMaxNumberOfBox"/>
        public int GetMaxNumberOfBox()
        {
            return MaxNumberOfBox;
        }

        /// <inheritdoc cref="IMonsterStorage.GetCurrentBoxSize"/>
        public int GetCurrentBoxSize()
        {
            return this.GetCurrentBoxMonsters().Count();
        }

    }
}

