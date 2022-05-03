using Optional;
using Pokaiju.Barattini;
using Pokaiju.Castorina.Npc;
using Pokaiju.Guo.GameItem;
using Pokaiju.Guo.Player;

namespace Pokaiju.Pierantoni;

public class MonsterBattle : IMonsterBattle
{
    private static readonly int ExpMultipler = 100;
    private static readonly int CaptureRange = 10;
    private static readonly int CaptureDifficult = 3;
    private static readonly int MoneyWon = 70;
    private static readonly int MoneyLost = 50;
    private static readonly int ExtraMoveAttack = 30;
    private static readonly int ExtraMovePp = 999;

    private bool _battleStatus; // true if the battle enemy/player team is defeat, false otherwise
    private bool _areEndPp;
    private bool _playerLose;
    private IMonster _playerCurrentMonster;
    private IMonster _enemy;
    private readonly IList<IMonster> _playerTeam;
    private readonly IList<IMonster> _enemyTeam;
    private readonly IPlayer _trainer;

    private INpcTrainer? _enemyTrainer;

    private readonly Moves _extraMoves;

    private MonsterBattle( IPlayer trainer, IList<IMonster> enemyTeam) {
        
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

    /***
     * {@inheritDoc}.
     */

    public IMoves EnemyAttack()
    {
        int x = new Random().Next(_enemy.GetNumberOfMoves());
        while (_enemy.IsOutOfPp(_enemy.GetMoves(x))) {
            x = (x + 1) % this._enemy.GetNumberOfMoves();
        }
        return _enemy.GetMoves(x);
    }

    /***
     * {@inheritDoc}.
     */
    
    public bool Capture() {
        ThrowExceptionIfItIsOver();
        if (!_enemy.IsWild()) {
            return false;
        }

        int attempt = new Random().Next(CaptureRange);
        if (attempt <= CaptureDifficult) {
            this._trainer.AddMonster(_enemy);
            this._battleStatus = false;
            return true;
        }
        return false;

    }

    /***
     * {@inheritDoc}.
     */
    
    public bool Escape() {
        ThrowExceptionIfItIsOver();
        if (!_enemy.IsWild()) {

            return false;
        } else {
            this._battleStatus = false;
            return true;
        }

    }

    /***
     * {@inheritDoc}.
     */
    public bool PlayerChangeMonster(int index) {
        ThrowExceptionIfItIsOver();
        IMonster? changingMonster = null;
        if (index == _playerCurrentMonster.Id) {
            return false;
        } else {
            foreach( var monster in _playerTeam) {
                if (monster.Id == index) {
                    changingMonster = monster;
                }
            }

            if (changingMonster is not null)
            {
                if (changingMonster.IsAlive()) {
                                this._playerCurrentMonster = changingMonster;
                
                                return true; 
                }
            }

            

            return false;
        }

    }

    /***
     * {@inheritDoc}.
     */
    public bool MovesSelection(int moveIndex) {

        if (!_playerCurrentMonster.IsOutOfPp(_playerCurrentMonster.GetMoves(moveIndex)) && this._battleStatus
                && this._playerCurrentMonster.IsAlive()) {
            Turn(this._playerCurrentMonster.GetMoves(moveIndex));

            return true;
        }
        ThrowExceptionIfItIsOver();
        return false;
    }

    private void Turn( IMoves monsterMove) {
        if (this._playerCurrentMonster.GetStats().Speed > this._enemy.GetStats().Speed) {
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
            this._battleStatus = false;
            this._playerLose = true;

        }

        if (!AreThereEnemies()) {
            // ending battle

            if (_enemyTrainer is not null ){
                _enemyTrainer.SetDefeated(true); 
            }
            this._battleStatus = false;
        } else {
            foreach (var monster in _enemyTeam)
            {
                if (monster.IsAlive())
                {
                    _enemy = monster;
                    break;
                }
            }
        }

    }

    private void MonsterAttack(IMonster m1, IMonster m2, IMoves move) {

        int damage = move.GetDamage(m2.GetMonsterType()) * move.GetBase() + m1.GetStats().Attack - m2.GetStats().Defense;
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

    private void RestoreAllMonsters( IList<IMonster> monsters) {
        foreach (var monster in monsters)
        {
            monster.RestoreAllMovesPp();
            monster.RestoreStats();
        }
        
    }

    /***
     * {@inheritDoc}.
     */
    public bool IsCurrentMonsterAlive() {
        return this._playerCurrentMonster.IsAlive();
    }

    private bool AllPlayerMonsterDefeated() {
        return _playerTeam.Any(m => m.IsAlive());

    }

    /***
     * {@inheritDoc}.
     */
    public bool IsOver() {

        return !_battleStatus;
    }

    private void ThrowExceptionIfItIsOver() {
        if (!this._battleStatus) {
           throw  new InvalidOperationException(); 
        }
    }

    /***
     * {@inheritDoc}.
     */
    public bool UseItem(IGameItem item) {
        return item.Use(_playerCurrentMonster);
    }

    /***
     * {@inheritDoc}.
     */
    public IMonster GetCurrentPlayerMonster() {

        return _playerCurrentMonster;
    }

    /***
     * {@inheritDoc}.
     */
    public IMonster GetCurrentEnemyMonster() {

        return _enemy;
    }

    /***
     * {@inheritDoc}.
     */
    public IPlayer GetPlayer() {
        return _trainer;
    }

    /***
     * {@inheritDoc}.
     */
    public Option<INpcTrainer> GetNpcEnemy() {
        if (_enemyTrainer is not null)
        {
            return Option.Some(_enemyTrainer);
        }

        return Option.None<INpcTrainer>();
    }

    /***
     * {@inheritDoc}.
     */
    public bool IsOverOfPp() {
        this._areEndPp = true;
        for (int c = 0; c < this._playerCurrentMonster.GetNumberOfMoves(); c++) {
            if (!this._playerCurrentMonster.IsOutOfPp(_playerCurrentMonster.GetMoves(c))) {
                this._areEndPp = false;
            }
        }
        return this._areEndPp;
    }

    /***
     * {@inheritDoc}.
     */
    public bool AttackWithExtraMove() {

        if (this._battleStatus && this._playerCurrentMonster.IsAlive()) {

            Turn(_extraMoves);

            return true;
        }
        ThrowExceptionIfItIsOver();
        return false;

    }

    /***
     * {@inheritDoc}.
     */
    public bool HasPlayerLost() {
        return this._playerLose;
    }

    /***
     * {@inheritDoc}.
     */
    public void EndingBattle() {
        if (HasPlayerLost()) {
            this._trainer.SetMoney(_trainer.GetMoney() - MoneyLost);
            RestoreAllMonsters(_playerTeam);
            RestoreAllMonsters(_enemyTeam);
        } else {
            this._trainer.SetMoney(_trainer.GetMoney() + MoneyWon);
        }

        this._trainer.EvolveMonsters();
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