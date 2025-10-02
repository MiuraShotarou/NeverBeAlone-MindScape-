using System.Collections.Generic;
using UnityEngine;


public class BattleLoopHandler : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
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
    }

    void Update()
    {
        switch (_battleState)
        {
            case BattleState.Init:
                _battleState = BattleState.Busy;
                // 初期設定
                _battleEvents.InitBattleData();
                break;
            case BattleState.Intro:
                _battleState = BattleState.Busy;
                // 戦闘開始Animation再生
                _battleEvents.PlayBattleIntro();
                break;
            case BattleState.QTE:
                _battleState = BattleState.WaitForCommand;
                // QTE実行
                _battleEvents.QTEStart();
                break;
            case BattleState.QTEFinished:
                _battleState = BattleState.Busy;
                // 行動順決定
                _battleEvents.SortBattleUnits();
                // ターンテーブルUI表示
                _battleEvents.InitTurnTable(); //_statusPanel下のオブジェクトを触るように変更する必要がある
                break;
            case BattleState.TurnStart:
                _battleState = BattleState.WaitForCommand;
                InitializeOnTurnStart();
                // 行動Unit取得
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
                // ターン終了処理
                EndTurn();
                break;
            case BattleState.Victory:
                _battleState = BattleState.Busy;
                _uiController.ShowVictoryText();
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
    /// <summary>
    /// ターン開始時の初期処理
    /// </summary>
    public void InitializeOnTurnStart()
    {
        _playerOneMoreFlg = false;
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    public void EndTurn()
    {
        // 行動隊列の先頭unitを行動配列の最後尾に移動
        BattleUnitBase unitToDequeue = BattleUnitQueue.Dequeue();
        BattleUnitQueue.Enqueue(unitToDequeue);
        _battleState = BattleState.TurnStart;
    }

    /// <summary>
    /// ターン終了判定処理
    /// </summary>
    public void JudgeTurnEnd()
    {
        // 敵行動の場合、ターン終了とする（敵はOneMoreしない認識）
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
        CommonUtils.LogDebugLine(this, "JudgeTurnEnd()","プレイヤー。結果：" + _battleState);
    }

    /// <summary>
    /// 戦闘終了の判定処理
    /// </summary>
    public void JudgeBattleEnd()
    {
        bool playerWipeout = true;
        bool enemyWipeout = true;
        // 敵全滅判定
        foreach (BattleUnitEnemyBase unit in _objects.EnemyUnits)
        {
            if (!unit.IsDead)
            {
                enemyWipeout = false;
                break;
            }
        }

        // プレイヤー全滅判定
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
            // プレイヤーが全滅の場合、GameOver
            _battleState = BattleState.GameOver;
        }
        else if (enemyWipeout)
        {
            // プレイヤー生存、敵が全滅の場合、戦闘勝利
            _battleState = BattleState.Victory;
        }
        else
        {
            // プレイヤーと敵が両方生存の場合、戦闘継続し、ターン終了判定に移行
            _battleState = BattleState.JudgeTurnEnd;
        }
        Debug.Log("JudgeBattleEnd実行。結果：" + _battleState);
    }
}