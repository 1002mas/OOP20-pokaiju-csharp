using Guo.Player;
using Optional;
using Pokaiju.Barattini;
using Pokaiju.Carafassi.GameEvents;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;
using Pokaiju.Pierantoni;

namespace Pokaiju.Castorina
{
    using NUnit.Framework;

    [TestFixture]
    public class TestNpc
    {
        private const int Monster1Health = 5;
        private const int Monster3Health = 75;
        private const int Monster2Health = 10;
        private const int Monster1Attack = 50;
        private const int Monster2Attack = 5;
        private const int Monster3Attack = 70;
        private const int GenericValue = 32;

        private INpcSimple? _npc1;
        private INpcSimple? _npc2;
        private INpcMerchant? _npc3;
        private INpcTrainer? _npc4;
        private INpcTrainer? _npc5;
        private Tuple<int, int>? _position;
        private IList<string>? _sentences;
        private IMonster? _monster1;
        private IPlayer? _player;
        private IList<IMonster>? _monsterList;
        private IList<Tuple<IGameItem, int>>? _list;
        private IGameEvent? _npcEvent;

        [SetUp]
        public void SetUp()
        {
            IList<IMonster> monsterListNpc5;
            IGameItem item1;
             IGameItem item2;
            this._sentences = new List<string>();
            this._sentences.Add("Frase 1");
            this._sentences.Add("Frase 2");
            this._sentences.Add("Frase 3");
            this._position = new Tuple<int,int>(1, 1);
            this._player = new Player(null, Gender.Female, "player", _position, 0, null);
            IMonster monster2;
            IMonster monster3;

            // item
            item1 = new CaptureItem("pietra", "descrizione");
            item2 = new CaptureItem("carta", "descrizione");
            Dictionary<IGameItem, int> inventory = new Dictionary<IGameItem, int>();
            inventory.Add(item1, 4);  
            inventory.Add(item2, 3);
            // monster
             IMoves m1 = new Moves("mossa1", 3, MonsterType.Grass, 9);
             IMoves m2 = new Moves("mossa2", 3, MonsterType.Fire, 3);
             IList<Tuple<IMoves, int>> listOfMoves = new List<Tuple<IMoves, int>>();
             listOfMoves.Add(new Tuple<IMoves, int>(m1, 5));
             listOfMoves.Add(new Tuple<IMoves, int>(m2, 6));
             IList<IMoves> listMoves = new List<IMoves>();
             listMoves.Add(m1);
             listMoves.Add(m2);
             
             IMonsterSpecies species = new MonsterSpeciesBuilder().Name("nome1").Info("Info1")
                .MonsterType(MonsterType.Fire).MovesList(listMoves).Build();
            this._monster1 = new MonsterBuilder().Health(Monster1Health).Attack(Monster1Attack)
                .Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();

            monster2 = new MonsterBuilder().Health(Monster2Health).Attack(Monster2Attack).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();

            monster3 = new MonsterBuilder().Health(Monster3Health).Attack(Monster3Attack).Defense(GenericValue)
                .Speed(GenericValue).Exp(GenericValue).Level(GenericValue).Wild(false).Species(species)
                .MovesList(listOfMoves).Build();

            _monsterList = new List<IMonster>();
            _monsterList.Add(monster2);
            monsterListNpc5 = new List<IMonster>();
            monsterListNpc5.Add(monster3);
            this._player.AddMonster(_monster1);
            // npc
            this._npc1 = new NpcSimple("nome1", _sentences, _position, false, false);
            this._npc2 = new NpcHealer("nome2", _sentences, _position,  false, false, _player);
            this._npc3 = new NpcMerchant("nome3", _sentences, _position, false, false, inventory);
            _list = new List<Tuple<IGameItem, int>>();
            _list.Add(new Tuple<IGameItem, int>(item1, 2));
            this._npc4 = new NpcTrainer("nome4", _sentences, _position, false, false, _monsterList, false);
            this._npc5 = new NpcTrainer("nome5", _sentences, _position, false, false, monsterListNpc5, false);
            IList<IMonster> npcEventList = new List<IMonster>();
            npcEventList.Add(this._monster1);
            this._npcEvent = new MonsterGift(8, true, true, false, npcEventList, _player);
            _npc1.AddGameEvent(this._npcEvent);
        }
        
        [Test]
        public void CommonFields() 
        {
            Assert.AreEqual("nome1", this._npc1?.GetName());
            Assert.AreEqual(0, this._npc1?.GetCurrentSetence());
            Assert.AreEqual(this._position, this._npc1?.GetPosition());
            // enabled : false
            Assert.AreEqual(Option.None<string>(), this._npc1?.InteractWith());
            // enabled : true
            this._npc1?.SetEnabled(true);
            Assert.AreEqual(Option.Some(this._sentences?.ElementAt(0)), this._npc1?.InteractWith());
            // enabled : true and event not active
            _npcEvent.Active = false;
            _npc1?.InteractWith();
            Assert.AreEqual(Option.None<IGameEvent>(), this._npc1?.GetTriggeredEvent());
            // enabled : true and event active
            _npcEvent.Active = true;
            _npc1?.InteractWith();
            Assert.AreEqual(Option.Some(_npcEvent), this._npc1?.GetTriggeredEvent());
        }

       [Test]
        public void TestNpcHealer() 
        {
            _npc2?.InteractWith();
           Assert.AreEqual(_monster1?.GetMaxHealth(), this._player?.GetAllMonsters().
               ElementAt(0).GetStats().Health);
        }

        [Test]
        public void TestNpcTrainer() 
        {
            if (this._player != null && this._npc4 != null && this._npc5 != null)
            {
                Assert.AreEqual(_monsterList, _npc4?.GetMonstersOwned());
                IMonsterBattle battle = new MonsterBattle(_player, _npc4);
                battle.MovesSelection(0);
                Assert.True(_npc4.IsDefeated());
                
                IMonsterBattle battle2 = new MonsterBattle(_player, _npc5);
                battle2.MovesSelection(0);
                Assert.False(_npc5.IsDefeated());
                battle2.EnemyAttack();
                Assert.True(battle.IsOver());
                Assert.False(_npc5.IsDefeated());
            }
            

        }

       [Test]
        public void TestNpcMerhant() 
        {
            if (this._player!= null && this._list != null )
            {
                int money = this._player.GetMoney();
                Assert.AreEqual(8, _npc3?.GetTotalPrice(_list));
                Assert.True(_npc3?.BuyItem(_list, _player));
                Assert.AreEqual(money - 8, this._player.GetMoney());
            }
            
        }
    }
}