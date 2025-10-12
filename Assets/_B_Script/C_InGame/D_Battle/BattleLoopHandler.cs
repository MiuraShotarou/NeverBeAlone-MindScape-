using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoopHandler : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private SaveManager _saveManager;
    private BattleUIController _uiController = default;

    public Queue<BattleUnitBase> BattleUnitQueue = default;
    [HideInInspector] public BattleUnitBase CurrentBattleUnit = null;
    private bool _playerOneMoreFlg = false;
    private BattleState _battleState;
    private ResultQTE _resultQte = ResultQTE.Stop;
    private BattleEventController _battleEvents;

    public BattleState BattleState { get => _battleState; set => _battleState = value; }
    public ResultQTE ResultQTE { get => _resultQte; set => _resultQte = value; }
    public bool PlayerOneMoreFlg { get => _playerOneMoreFlg; set => _playerOneMoreFlg = value; }

    private void Start()
    {
        _uiController = _objects.UIController;
        _battleEvents = GetComponent<BattleEventController>();

        // Firebase UID 初回取得後にローカルデータ読み込み＆戦闘前オートセーブ
        _saveManager.EnsureSignedIn(() =>
        {
            SaveData loadedData = _saveManager.LoadFromLocal();
            _playerData.Exp = loadedData.PlayerExp;
            _playerData.Level = loadedData.PlayerLevel;
            _playerData.Hp = loadedData.PlayerHp;
            _playerData.EncounterCount = loadedData.EncounterCount;

            var player = _objects.PlayerUnits[0];
            player.Hp = _playerData.Hp;
            player.ExpAmount = _playerData.Exp;
            player.Level = _playerData.Level;
            player.Progress = _playerData.EncounterCount;

            Debug.Log("ローカルJSON → ScriptableObject → BattleUnitPlayer 反映完了");

            // 戦闘前オートセーブ（Firebase にも書き込み）
            _saveManager.AutoSave(CreateSaveDataFromPlayer());
        });

        _battleState = BattleState.Init;
    }

    private void Update()
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
                _battleEvents.DecideEnemy();
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
            case BattleState.TurnStart: //ここ以下のステートはターンごとに呼び出される
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

                // 経験値・ステータス更新
                UpdatePlayerStatus();
                UpdatePlayerDataFromPlayers();

                // JSON はローカルのみ保存（Firebase は戦闘前オートセーブ済み）
                _saveManager.SaveToLocal(CreateSaveDataFromPlayer());

                Debug.Log("戦闘終了後のデータをローカルJSONに保存完了");
                break;
            case BattleState.GameOver:
                _battleState = BattleState.Busy;
                _uiController.ShowGameOverText();
                break;
        }
    }

    public void InitializeOnTurnStart() => _playerOneMoreFlg = false;

    public void EndTurn()
    {
        BattleUnitBase unitToDequeue = BattleUnitQueue.Dequeue();
        unitToDequeue.OnConditionActivate(ConditionActivationType.OnTurnEnd);
        BattleUnitQueue.Enqueue(unitToDequeue);
        _battleState = BattleState.TurnStart;
    }

    public void JudgeTurnEnd()
    {
        if (CurrentBattleUnit is BattleUnitEnemyBase)
        {
            _battleState = BattleState.TurnEnd;
            return;
        }
        _battleState = _playerOneMoreFlg ? BattleState.TurnStart : BattleState.TurnEnd;
    }

    public void JudgeBattleEnd()
    {
        bool playerWipeout = true;
        bool enemyWipeout = true;

        foreach (var unit in _objects.EnemyUnits)
            if (!unit.IsDead) { enemyWipeout = false; break; }

        foreach (var unit in _objects.PlayerUnits)
            if (!unit.IsDead) { playerWipeout = false; break; }

        if (playerWipeout) _battleState = BattleState.GameOver;
        else if (enemyWipeout) _battleState = BattleState.Victory;
        else _battleState = BattleState.JudgeTurnEnd;
    }

    /// <summary>
    /// PlayerBaseの値にバトル後のデータを代入
    /// </summary>
    public void UpdatePlayerStatus()
    {
        int totalExp = 0;
        foreach (var enemy in _objects.EnemyUnits)
        {
            totalExp += enemy.ExpReward;
        }

        // ここに追加
        var player = _objects.PlayerUnits[0];
        player.ExpAmount += totalExp;
        player.Progress.EncounterCount++;
    }

    /// <summary>
    /// 現在のPlayerBaseをScriptableObjectのデータに代入
    /// </summary>
    private void UpdatePlayerDataFromPlayers()
    {
        var player = _objects.PlayerUnits[0];
        _playerData.Exp = player.ExpAmount;
        _playerData.Level = player.Level;
        _playerData.Hp = player.Hp;
        _playerData.MaxHp = player.MaxHp;
        _playerData.EncounterCount = player.Progress;
    }

    /// <summary>
    /// JSON用クラスに入れる
    /// </summary>
    private SaveData CreateSaveDataFromPlayer()
    {
        var player = _objects.PlayerUnits[0];
        return new SaveData
        {
            PlayerLevel = player.Level,
            PlayerHp = player.Hp,
            PlayerExp = player.ExpAmount,
            EncounterCount = player.Progress
        };
    }

    /// <summary>
    /// OnClick()等で呼べばもう一回やり直せる 勝利後に呼ぶならデータ更新される
    /// </summary>
    public void Retry()
    {
        SaveData loadedData = _saveManager.LoadFromLocal();
        _battleState = BattleState.Init;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
