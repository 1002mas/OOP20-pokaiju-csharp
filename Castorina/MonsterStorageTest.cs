using Guo.Player;
using Optional;
using Pokaiju.Barattini;
using Pokaiju.Castorina.Storage;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Castorina
{
    using NUnit.Framework;
    
    [TestFixture]
    
    public class MonsterStorageTest
    {
        private const int GenericValue = 3;
        private const int MaxBoxInStorage = 10;
        private const int MaxBoxSize = 10;

        private IMonsterStorage? _monsterStorage;
        private IMonsterBox? _monsterBox;
        private IMonster? _monster1;
        private IMonster? _monster2;
        private IMonster? _monster3;
        private IMonster? _monster4;
        private IMonster? _monster5;
        private IMonster? _monster6;
        private IMonster? _monster7;
        private IPlayer? _player;

        private void AddMonsterList(IMonsterStorage storage, int n,IMonster monster)
        {
            for (int i = 0; i < n; i++) 
            {
                storage.AddMonster(monster);
            }
        }

        [SetUp]
        public void InitFactory()
        {
            int posCoord = 1;
            Tuple<int,int> position = new Tuple<int, int>(posCoord, posCoord);
            this._player = new Player("player", Gender.Female, 0, position, null);
            // monster
            IMoves m1 = new Moves("mossa1", GenericValue, MonsterType.Grass, GenericValue);
            IMoves m2 = new Moves("mossa2", GenericValue, MonsterType.Fire, GenericValue);
            IList<Tuple<IMoves, int>> listOfMoves = new List<Tuple<IMoves, int>>();
            listOfMoves.Add(new Tuple<IMoves, int>(m1, GenericValue));
            IList<IMoves> listMoves = new List<IMoves>();
            listMoves.Add(m1);
            listMoves.Add(m2);
            IMonsterSpecies species = new MonsterSpeciesBuilder().Name("nome1").Info("Info1")
                .MonsterType(MonsterType.Fire).MovesList(listMoves).Build();

            this._monster1 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster2 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster3 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster4 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster5 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster6 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster7 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();
            this._monster1 = new MonsterBuilder().Health(GenericValue).Attack(GenericValue).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();

            this._player.AddMonster(this._monster2);
            this._monsterStorage = new MonsterStorage(this._player);
            this._monsterBox = new MonsterBox("Box1", this._monsterStorage.GetMaxSizeOfBox());

        }

        [Test]
        public void MonsterBox()
        {
            if (this._monster1 != null && this._monster2 != null)
            {
                this._monsterBox?.AddMonster(this._monster1);
                Assert.AreEqual(this._monster1, this._monsterBox?.GetAllMonsters().ElementAt(0));
                Assert.AreEqual(Option.Some(this._monster1), this._monsterBox?.GetMonster(this._monster1.Id));

                this._monsterBox?.Exchange(_monster2, this._monster1.Id);
                Assert.AreEqual(1, this._monsterBox?.GetAllMonsters().Count());
                Assert.AreEqual(Option.Some(this._monster2), this._monsterBox?.GetMonster(this._monster2.Id));

                this._monsterBox?.RemoveMonster(this._monster2.Id);
                Assert.False(this._monsterBox?.GetAllMonsters().Contains(this._monster2));
                Assert.AreEqual(Option.None<IMonsterBox>(), this._monsterBox?.GetMonster(this._monster2.Id));
                Assert.AreEqual(0, this._monsterBox?.GetAllMonsters().Count());
            }
            

        }

        [Test]
        public void MonsterStorage()
        {
            if (this._monster1 != null && this._monster2 != null && this._monster3 != null && 
                this._monster4 != null && this._monster5 != null && this._monster6 != null && 
                this._monster7 != null && this._player != null)
            {
                // add monster
            Assert.AreEqual("BOX1", this._monsterStorage?.GetCurrentBoxName());
            Assert.True(this._monsterStorage?.AddMonster(this._monster1));
            Assert.True(this._monsterStorage?.GetCurrentBoxMonsters().Contains(this._monster1));
            Assert.AreEqual("BOX1", this._monsterStorage?.GetCurrentBoxName());
            // previous box

            this._monsterStorage?.PreviousBox();
            Assert.AreEqual("BOX10", this._monsterStorage?.GetCurrentBoxName());
            // next box
            this._monsterStorage?.NextBox();
            Assert.AreEqual("BOX1", this._monsterStorage?.GetCurrentBoxName());

            AddMonsterList(this._monsterStorage, this._monsterStorage.GetMaxSizeOfBox(), this._monster1);
            this._monsterStorage.NextBox();

            Assert.True(this._monsterStorage.GetCurrentBoxMonsters().Contains(this._monster1));
            Assert.AreEqual(1, this._monsterStorage.GetCurrentBoxMonsters().Count());
            // exchange player _monster2 with box this._monster1
            Assert.True(this._monsterStorage.Exchange(this._monster2, this._monster1.Id));
            Assert.True(this._monsterStorage.GetCurrentBoxMonsters().Contains(this._monster2));
            Assert.True(this._player?.GetAllMonsters().Contains(this._monster1));

            this._player?.AddMonster(this._monster3); // player team: _monster3, this._monster1
            Assert.True(this._monsterStorage.DepositMonster(this._monster3));
            Assert.False(this._player.GetAllMonsters().Contains(this._monster3));
            Assert.True(this._monsterStorage.GetCurrentBoxMonsters().Contains(this._monster3));

            // player team: this._monster1
            // withDrawMonster
            Assert.True(this._monsterStorage.WithdrawMonster(_monster3.Id));
            Assert.False(this._monsterStorage.WithdrawMonster(_monster3.Id));

            // player team: this._monster1, _monster3
            this._player.AddMonster(this._monster4);
            this._player.AddMonster(this._monster5);
            this._player.AddMonster(this._monster6);
            this._player.AddMonster(this._monster7);
            // player team full

            // withDrawMonster
            Assert.False(this._monsterStorage.WithdrawMonster(this._monster2.Id));
            }
            
        }

        [Test]
        public void MonsterStorageWithList()
        {
            if (this._player != null)
            {
                IMonsterStorage monsterStorage2;
                IMonsterBox box2 = new MonsterBox("NEWBOX1", MaxBoxSize);
                IList<IMonsterBox> boxList = new List<IMonsterBox>();
                boxList.Add(box2);
                monsterStorage2 = new MonsterStorage(this._player, boxList);
                Assert.AreEqual("NEWBOX1", monsterStorage2.GetCurrentBoxName());   
            }
        }

        [Test]
        public void MonsterStorageWithList2()
        {
            
            if (this._player != null)
            {
                IMonsterStorage monsterStorage3;
                IList<IMonsterBox> boxList = new List<IMonsterBox>();
                for (int i = 0; i < MaxBoxInStorage + 1; i++)
                {
                    boxList.Add(new MonsterBox("BOX" + (i + 1), MaxBoxSize));
                }
                monsterStorage3 = new MonsterStorage(_player, boxList);
                monsterStorage3.PreviousBox();
                Assert.AreEqual("BOX10", monsterStorage3.GetCurrentBoxName());
            }
           
        }

        [Test]
        public void FullMonsterStorage()
        {
            if (this._player != null && this._monster1 != null && this._monster2 != null && this._monster3 != null)
            {
                IMonsterStorage monsterStorage4 = new MonsterStorage(this._player);
                for (int i = 0; i < MaxBoxSize; i++)
                {
                    AddMonsterList(monsterStorage4, 10, this._monster1);
                    monsterStorage4.NextBox();
                }

                Assert.False(monsterStorage4.AddMonster(this._monster2));
                Assert.True(monsterStorage4.WithdrawMonster(this._monster1.Id));
                Assert.True(monsterStorage4.DepositMonster(this._monster1));
                Assert.False(monsterStorage4.AddMonster(this._monster3));
            }


        }
    }
}

