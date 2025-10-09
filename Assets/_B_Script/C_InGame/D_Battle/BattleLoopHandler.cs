using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoopHandler : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] SaveManager _saveManager;
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
        JSONToScritable();
        _battleState = BattleState.Init;
        InitializePlayersFromData();
    }

    void Update()
    {
        switch (_battleState)
        {
            case BattleState.Init:
                // SavePlayerData(); //add
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
            // エンカウント後はここから下の処理がターン経過ごとに一度だけ呼び出される
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

                // バトル終了後のステータス反映
                UpdatePlayerStatus();             // 経験値付与
                UpdatePlayerDataFromPlayers();    // BattleUnitPlayer → ScriptableObject

                // JSONに保存
                SavePlayerData();                 // ScriptableObject → JSON保存

                Debug.Log("戦闘終了後のデータをJsonに保存しました。");
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

        //ターンエンド時に発動する状態異常を発動
        unitToDequeue.OnConditionActivate(ConditionActivationType.OnTurnEnd);

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
        {
            totalExp += enemy.ExpReward;
        }

        var player = _objects.PlayerUnits[0];
        player.ExpAmmount += totalExp;
    }

    /// <summary>
    /// 戦闘直前に呼ぶ。ScriptableObject → BattleUnitPlayer に反映し、同時に Json に保存する
    /// </summary>
    private void InitializePlayersFromData()
    {
        var player = _objects.PlayerUnits[0];
        player.Hp = _playerData.Hp;
        player.MaxHp = _playerData.MaxHp;
        player.ExpAmmount = _playerData.Exp;
        player.Level = _playerData.Level;

        Debug.Log("BattleUnitPlayer初期化（ScriptableObjectから）: Exp=" + _playerData.Exp + ", HP=" + _playerData.Hp);

        // 戦闘直前の状態を Json に保存
        SavePlayerData();
    }

    /// <summary>
    /// ここでjsonに保存する
    /// </summary>
    public void SavePlayerData()
    {
        SaveData data = new SaveData
        {
            playerLevel = _playerData.Level,
            playerHp = _playerData.Hp,
            playerExp = _playerData.Exp,
            encountCount = _playerData.EncountCount
        };
        _saveManager.AutoSave(data); // これで戦闘直前のScriptableObjectデータをJsonに保存
        Debug.Log("戦闘直前のScriptableObjectデータをJsonに保存: Exp=" + _playerData.Exp + ", HP=" + _playerData.Hp);
    }

    private void UpdatePlayerDataFromPlayers()
    {
        var player = _objects.PlayerUnits[0];
        _playerData.Exp = player.ExpAmmount;
        _playerData.Level = player.Level;
        _playerData.Hp = player.Hp;
        _playerData.MaxHp = player.MaxHp;

        // ScriptableObject の値を直接インクリメント
        _playerData.EncountCount++;

        Debug.Log("PlayerData更新: Exp=" + _playerData.Exp + ", HP=" + _playerData.Hp + ", Count=" + _playerData.EncountCount);
    }

    public void Retry()
    {
        // JSONからロード
        SaveData loadedData = _saveManager.LoadGame();
        _battleState = BattleState.Init;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    void JSONToScritable()
    {
        // JSONからロード
        SaveData loadedData = _saveManager.LoadGame();

        // ScriptableObjectに反映
        _playerData.Level = loadedData.playerLevel;
        _playerData.Hp = loadedData.playerHp;
        _playerData.Exp = loadedData.playerExp;
        _playerData.EncountCount = loadedData.encountCount;

        // BattleUnitPlayer に反映
        var player = _objects.PlayerUnits[0];
        player.Hp = _playerData.Hp;
        player.ExpAmmount = _playerData.Exp;
        player.Level = _playerData.Level;

        Debug.Log("JSON → ScriptableObject → BattleUnitPlayer 反映完了");
    }

}
