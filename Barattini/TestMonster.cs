using Optional;

namespace Pokaiju.Barattini
{
    using NUnit.Framework;
    
    [TestFixture]
    public class TestMonster
    {
        private IMonster _monster;
        
        [SetUp]
        public void SetUp()
        {
            _monster = new Monster(5, new MonsterStats(1, 1, 1, 1), 50, 1, true, null);
        }

        [Test]
        public void Stats()
        {
            Assert.Equals(_monster.GetStats().Health, 1);
            Assert.True(_monster.IsAlive());
        }
    }
}

