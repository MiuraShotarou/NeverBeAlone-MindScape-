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

    private string userId; // 端末ごとの一意ID

    void Start()
    {
        _uiController = _objects.UIController;
        _battleEvents = GetComponent<BattleEventController>();

        userId = SystemInfo.deviceUniqueIdentifier; // 宣言で代入できないのでここで

        SaveData loadedData = _saveManager.LoadFromLocal();
        _playerData.Exp = loadedData.playerExp;
        _playerData.Level = loadedData.playerLevel;
        _playerData.Hp = loadedData.playerHp;
        _playerData.EncountCount = loadedData.encountCount;

        var player = _objects.PlayerUnits[0];
        player.Hp = _playerData.Hp;
        player.ExpAmmount = _playerData.Exp;
        player.Level = _playerData.Level;
        player.EncountCount = _playerData.EncountCount;

        Debug.Log("ローカルJSON → ScriptableObject → BattleUnitPlayer 反映完了");

        // 戦闘開始前にオートセーブ（JSON → Firebase）
        SavePlayerDataToAll();

        _battleState = BattleState.Init;
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

                // 経験値・ステータス更新
                UpdatePlayerStatus();
                UpdatePlayerDataFromPlayers();

                // JSONはローカルにのみ保存（Firebase更新は戦闘前のオートセーブのみ）
                SavePlayerDataToLocalOnly();

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

    public void UpdatePlayerStatus()
    {
        int totalExp = 0;
        foreach (var enemy in _objects.EnemyUnits) totalExp += enemy.ExpReward;
        var player = _objects.PlayerUnits[0];
        player.ExpAmmount += totalExp;
    }

    private void UpdatePlayerDataFromPlayers()
    {
        var player = _objects.PlayerUnits[0];
        _playerData.Exp = player.ExpAmmount;
        _playerData.Level = player.Level;
        _playerData.Hp = player.Hp;
        _playerData.MaxHp = player.MaxHp;
        _playerData.EncountCount++;
    }

    private void SavePlayerDataToAll()
    {
        SaveData data = new SaveData
        {
            playerLevel = _playerData.Level,
            playerHp = _playerData.Hp,
            playerExp = _playerData.Exp,
            encountCount = _playerData.EncountCount
        };

        _saveManager.AutoSave(userId, data);
    }

    private void SavePlayerDataToLocalOnly()
    {
        SaveData data = new SaveData
        {
            playerLevel = _playerData.Level,
            playerHp = _playerData.Hp,
            playerExp = _playerData.Exp,
            encountCount = _playerData.EncountCount
        };

        _saveManager.SaveToLocal(data);
    }

    public void Retry()
    {
        SaveData loadedData = _saveManager.LoadFromLocal();
        _battleState = BattleState.Init;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}