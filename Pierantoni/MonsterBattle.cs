using Optional;
using Pokaiju.Barattini;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Pierantoni;

public class MonsterBattle : IMonsterBattle
{
    private const int ExpMultipler = 100;
    private const int CaptureRange = 10;
    private const int CaptureDifficult = 3;
    private const int MoneyWon = 70;
    private const int MoneyLost = 50;
    private const int ExtraMoveAttack = 30;
    private const int ExtraMovePp = 999;

    private bool _battleStatus; // true if the battle enemy/player team is defeat, false otherwise
    private bool _areEndPp;
    private bool _playerLose;
    private IMonster _playerCurrentMonster;
    private IMonster _enemy;
    private readonly IList<IMonster> _playerTeam;
    private readonly IList<IMonster> _enemyTeam;
    private readonly IPlayer _trainer;

    private readonly INpcTrainer? _enemyTrainer;

    private readonly Moves _extraMoves;

    private MonsterBattle( IPlayer trainer, IEnumerable<IMonster> enemyTeam) {
        
        _trainer = trainer;
        _battleStatus = true;
        _playerLose = false;
        _enemyTrainer = null;
        _playerTeam = _trainer.GetAllMonsters();
        _playerCurrentMonster = ChooseStartingMonster();
        _enemyTeam = new List<IMonster>(enemyTeam);
        _enemy = _enemyTeam[0];
        _extraMoves = new Moves("Testata", ExtraMoveAttack, MonsterType.None, ExtraMovePp);
        _areEndPp = false;
    }

    public MonsterBattle(IPlayer trainer,  INpcTrainer enemyTrainer)

        : this(trainer, enemyTrainer.GetMonstersOwned())
    {
        _enemyTrainer = enemyTrainer;
    }

    public MonsterBattle( IPlayer trainer, IMonster wildMonster) 
        : this(trainer, new List<IMonster>{wildMonster})
    {
    }

    /// <inheritdoc cref="IMonsterBattle.EnemyAttack"/>
    public IMoves EnemyAttack()
    {
        var x = new Random().Next(_enemy.GetNumberOfMoves());
        while (_enemy.IsOutOfPp(_enemy.GetMoves(x))) {
            x = (x + 1) % _enemy.GetNumberOfMoves();
        }
        return _enemy.GetMoves(x);
    }
    
    /// <inheritdoc cref="IMonsterBattle.Capture"/>
    public bool Capture() {
        ThrowExceptionIfItIsOver();
        if (!_enemy.IsWild()) {
            return false;
        }

        var attempt = new Random().Next(CaptureRange);
        if (attempt > CaptureDifficult) return false;
        _trainer.AddMonster(_enemy);
        _battleStatus = false;
        return true;

    }
    
    /// <inheritdoc cref="IMonsterBattle.Escape"/>
    public bool Escape() {
        ThrowExceptionIfItIsOver();
        if (!_enemy.IsWild()) {

            return false;
        }

        _battleStatus = false;
        return true;

    }
    
    /// <inheritdoc cref="IMonsterBattle.PlayerChangeMonster"/>
    public bool PlayerChangeMonster(int index) {
        ThrowExceptionIfItIsOver();
        IMonster? changingMonster = null;
        if (index == _playerCurrentMonster.Id) {
            return false;
        }

        foreach( var monster in _playerTeam) {
            if (monster.Id == index) {
                changingMonster = monster;
            }
        }

        if (changingMonster is null) return false;
        if (!changingMonster.IsAlive()) return false;
        _playerCurrentMonster = changingMonster;
                
        return true;

    }
    
    /// <inheritdoc cref="IMonsterBattle.MovesSelection"/>
    public bool MovesSelection(int moveIndex) {

        if (!_playerCurrentMonster.IsOutOfPp(_playerCurrentMonster.GetMoves(moveIndex)) && _battleStatus
                && _playerCurrentMonster.IsAlive()) {
            Turn(_playerCurrentMonster.GetMoves(moveIndex));

            return true;
        }
        ThrowExceptionIfItIsOver();
        return false;
    }

    private void Turn( IMoves monsterMove) {
        if (_playerCurrentMonster.GetStats().Speed > _enemy.GetStats().Speed) {
            MonsterAttack(_playerCurrentMonster, _enemy, monsterMove);
            if (_enemy.IsAlive()) {
                MonsterAttack(_enemy, _playerCurrentMonster, EnemyAttack());
            }
        } else {
            MonsterAttack(_enemy, _playerCurrentMonster, EnemyAttack());
            if (_playerCurrentMonster.IsAlive()) {
                MonsterAttack(_playerCurrentMonster, _enemy, monsterMove);
            }
        }
        if (!_enemy.IsAlive()) {
            _playerCurrentMonster.IncExp(_enemy.GetLevel() * ExpMultipler);
        }
        if (AllPlayerMonsterDefeated()) { // player's team defeated
            _battleStatus = false;
            _playerLose = true;

        }

        if (!AreThereEnemies()) {
            // ending battle

            _enemyTrainer?.SetDefeated(true);
            _battleStatus = false;
        } else {
            foreach (var monster in _enemyTeam)
            {
                if (!monster.IsAlive()) continue;
                _enemy = monster;
                break;
            }
        }

    }

    private void MonsterAttack(IMonster m1, IMonster m2, IMoves move) {

        var damage = move.GetDamage(m2.GetMonsterType()) * move.GetBase() + m1.GetStats().Attack - m2.GetStats().Defense;
        if (damage < 1) {
            damage = 1;
        }
        if (!move.Equals(_extraMoves)) {
            m1.DecMovePp(move);
        }

        m2.SetHealth(m2.GetStats().Health - damage);
    }

    private bool AreThereEnemies() {

        return _enemyTeam.Any(m => m.IsAlive());
    }

    private static void RestoreAllMonsters( IEnumerable<IMonster> monsters) {
        foreach (var monster in monsters)
        {
            monster.RestoreAllMovesPp();
            monster.RestoreStats();
        }
        
    }
    
    /// <inheritdoc cref="IMonsterBattle.IsCurrentMonsterAlive"/>
    public bool IsCurrentMonsterAlive() {
        return _playerCurrentMonster.IsAlive();
    }

    private bool AllPlayerMonsterDefeated() {
        return _playerTeam.Any(m => m.IsAlive());

    }
    
    /// <inheritdoc cref="IMonsterBattle.IsOver"/>
    public bool IsOver() {

        return !_battleStatus;
    }

    private void ThrowExceptionIfItIsOver() {
        if (!_battleStatus) {
           throw  new InvalidOperationException(); 
        }
    }
    
    /// <inheritdoc cref="IMonsterBattle.UseItem"/>
    public bool UseItem(IGameItem item) {
        return item.Use(_playerCurrentMonster);
    }
    
    /// <inheritdoc cref="IMonsterBattle.GetCurrentPlayerMonster"/>
    public IMonster GetCurrentPlayerMonster() {

        return _playerCurrentMonster;
    }
    
    /// <inheritdoc cref="IMonsterBattle.GetCurrentEnemyMonster"/>
    public IMonster GetCurrentEnemyMonster() {

        return _enemy;
    }
    
    /// <inheritdoc cref="IMonsterBattle.GetPlayer"/>
    public IPlayer GetPlayer() {
        return _trainer;
    }
    
    /// <inheritdoc cref="IMonsterBattle.GetNpcEnemy"/>
    public Option<INpcTrainer> GetNpcEnemy()
    {
        return _enemyTrainer is not null ? Option.Some(_enemyTrainer) : Option.None<INpcTrainer>();
    }
    
    /// <inheritdoc cref="IMonsterBattle.IsOverOfPp"/>
    public bool IsOverOfPp() {
        _areEndPp = true;
        for (var c = 0; c < _playerCurrentMonster.GetNumberOfMoves(); c++) {
            if (!_playerCurrentMonster.IsOutOfPp(_playerCurrentMonster.GetMoves(c))) {
                _areEndPp = false;
            }
        }
        return _areEndPp;
    }
    
    /// <inheritdoc cref="IMonsterBattle.AttackWithExtraMove"/>
    public bool AttackWithExtraMove() {

        if (_battleStatus && _playerCurrentMonster.IsAlive()) {

            Turn(_extraMoves);

            return true;
        }
        ThrowExceptionIfItIsOver();
        return false;

    }
    
    /// <inheritdoc cref="IMonsterBattle.HasPlayerLost"/>
    public bool HasPlayerLost() {
        return _playerLose;
    }
    
    /// <inheritdoc cref="IMonsterBattle.EndingBattle"/>
    public void EndingBattle() {
        if (HasPlayerLost()) {
            _trainer.SetMoney(_trainer.GetMoney() - MoneyLost);
            RestoreAllMonsters(_playerTeam);
            RestoreAllMonsters(_enemyTeam);
        } else {
            _trainer.SetMoney(_trainer.GetMoney() + MoneyWon);
        }

        _trainer.EvolveMonsters();
    }

    private IMonster ChooseStartingMonster() {
        foreach(var monster in _playerTeam) {
            if (monster.IsAlive()) {
                return monster;
            }
        }
        throw new InvalidOperationException(); 
        
    }
}