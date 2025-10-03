using System.Collections.Generic;
using UnityEngine;

public class BattleLoopHandler : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    [SerializeField] private PlayerData playerData;
    private BattleUIController _uiController = default;

    public Queue<BattleUnitBase> BattleUnitQueue = default;
    [HideInInspector] public BattleUnitBase CurrentBattleUnit = null;
    private bool _playerOneMoreFlg = false;
    private BattleState _battleState;
    private ResultQTE _resultQte = global::ResultQTE.Stop;
    private BattleEventController _battleEvents;

    public BattleState BattleState
    {
        get { return _battleState; }
        set { _battleState = value; }
    }

    public ResultQTE ResultQTE
    {
        get => _resultQte;
        set => _resultQte = value;
    }
    public bool PlayerOneMoreFlg
    {
        get => _playerOneMoreFlg;
        set => _playerOneMoreFlg = value;
    }

    void Start()
    {
        _uiController = _objects.UIController;
        _battleEvents = gameObject.GetComponent<BattleEventController>();
        _battleState = BattleState.Init;

        InitializePlayersFromData(); // 追加
    }

    void Update()
    {
        switch (_battleState)
        {
            case BattleState.Init:
                _battleState = BattleState.Busy;
                _battleEvents.InitBattleData();
                break;
            case BattleState.Intro:
                _battleState = BattleState.Busy;
                _battleEvents.PlayBattleIntro();
                break;
            case BattleState.QTE:
                _battleState = BattleState.WaitForCommand;
                _battleEvents.QTEStart();
                break;
            case BattleState.QTEFinished:
                _battleState = BattleState.Busy;
                _battleEvents.SortBattleUnits();
                _battleEvents.InitTurnTable();
                break;
            case BattleState.TurnStart:
                _battleState = BattleState.WaitForCommand;
                InitializeOnTurnStart();
                CurrentBattleUnit = BattleUnitQueue.Peek().GetComponent<BattleUnitBase>();
                _battleEvents.UpdateTurnTable();
                _battleEvents.BattleUnitTakeAction();
                break;
            case BattleState.JudgeUnitSurvive:
                _battleState = BattleState.Busy;
                _battleEvents.JudgeUnitsSurvive();
                break;
            case BattleState.JudgeTurnEnd:
                _battleState = BattleState.Busy;
                JudgeTurnEnd();
                break;
            case BattleState.TurnEnd:
                _battleState = BattleState.Busy;
                EndTurn();
                break;
            case BattleState.Victory:
                _battleState = BattleState.Busy;
                _uiController.ShowVictoryText();
                UpdatePlayerStatus(); // 追加
                UpdatePlayerDataFromPlayers(); // 追加
                break;
            case BattleState.GameOver:
                _battleState = BattleState.Busy;
                _uiController.ShowGameOverText();
                break;
            case BattleState.Busy:
            case BattleState.WaitForCommand:
            default:
                break;
        }
    }

    public void InitializeOnTurnStart()
    {
        _playerOneMoreFlg = false;
    }

    public void EndTurn()
    {
        BattleUnitBase unitToDequeue = BattleUnitQueue.Dequeue();
        BattleUnitQueue.Enqueue(unitToDequeue);
        _battleState = BattleState.TurnStart;
    }

    public void JudgeTurnEnd()
    {
        if (CurrentBattleUnit is BattleUnitEnemyBase)
        {
            _battleState = BattleState.TurnEnd;
            CommonUtils.LogDebugLine(this, "JudgeTurnEnd()", "敵側。結果：" + _battleState);
            return;
        }

        if (_playerOneMoreFlg)
        {
            _battleState = BattleState.TurnStart;
        }
        else
        {
            _battleState = BattleState.TurnEnd;
        }
        CommonUtils.LogDebugLine(this, "JudgeTurnEnd()", "プレイヤー。結果：" + _battleState);
    }

    public void JudgeBattleEnd()
    {
        bool playerWipeout = true;
        bool enemyWipeout = true;

        foreach (BattleUnitEnemyBase unit in _objects.EnemyUnits)
        {
            if (!unit.IsDead)
            {
                enemyWipeout = false;
                break;
            }
        }

        foreach (BattleUnitPlayer unit in _objects.PlayerUnits)
        {
            if (!unit.IsDead)
            {
                playerWipeout = false;
                break;
            }
        }

        if (playerWipeout)
        {
            _battleState = BattleState.GameOver;
        }
        else if (enemyWipeout)
        {
            _battleState = BattleState.Victory;
        }
        else
        {
            _battleState = BattleState.JudgeTurnEnd;
        }

        Debug.Log("JudgeBattleEnd実行。結果：" + _battleState);
    }

    public void UpdatePlayerStatus()
    {
        int totalExp = 0; // この戦闘で獲得する総経験値
        foreach (var enemy in _objects.EnemyUnits)
            totalExp += enemy.ExpReward;

        foreach (var player in _objects.PlayerUnits)
        {
            player.ExpAmmount += totalExp;
        }
    }

    void InitializePlayersFromData() // 追加
    {
        foreach (var player in _objects.PlayerUnits)
        {
            player.Hp = playerData.hp;
            player.MaxHp = playerData.maxHp;
            player.ExpAmmount = playerData.exp;
            player.Level = playerData.level;
        }
        Debug.Log("Player初期化: Exp=" + playerData.exp + ", HP=" + playerData.hp);
    }

    void UpdatePlayerDataFromPlayers() // 追加
    {
        foreach (var player in _objects.PlayerUnits)
        {
            playerData.exp = player.ExpAmmount;
            playerData.level = player.Level;
            playerData.hp = player.Hp;
            playerData.maxHp = player.MaxHp;
        }
        Debug.Log("PlayerData更新: Exp=" + playerData.exp + ", HP=" + playerData.hp);
    }
}
