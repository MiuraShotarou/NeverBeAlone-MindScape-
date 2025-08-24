using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class BattleHandler : MonoBehaviour
{
    public List<GameObject> EnemyUnits = default;
    public List<GameObject> PlayerUnits = default;
    private List<GameObject> _battleUnitList = new List<GameObject>();
    private Queue<GameObject> _battleUnitQueue = new Queue<GameObject>();
    private int _battleUnitCount = 0;
    private bool _playerOneMoreFlg = false;


    private bool _enemyOneMore = false;
    private bool _isHandlerBusy = false;

    private BattleState _battleState;
    [SerializeField] private GameObject _roulettePanel = default;
    [SerializeField] private GameObject _startText = default;
    [SerializeField] private GameObject _targetSelectText = default;
    [SerializeField] private GameObject _commandPanel = default;
    [SerializeField] private GameObject _playableHandler = default;
    [SerializeField] private GameObject _turnTable = default;
    [SerializeField] PlayableDirector _director = default;

    private GameObject _selectedUnit = null; 
    private bool _isBattleOver = false;
    private RouletteStatus _rouletteResult = RouletteStatus.Stop;
    
    public BattleState BattleState
    {
        get { return _battleState; }
        set { _battleState = value; }
    }
    public bool PlayerOneMoreFlg
    {
        get => _playerOneMoreFlg;
        set => _playerOneMoreFlg = value;
    }

    public bool EnemyOneMore
    {
        get => _enemyOneMore;
        set => _enemyOneMore = value;
    }

    /// <summary>
    /// QTE判定結果
    /// </summary>
    public enum RouletteStatus
    {
        Stop, Rolling, Miss, Good, Excellent
    }

    public enum BattleStatus
    {
        Wait, Busy
    }
    
    void Start()
    {
        _battleUnitCount = EnemyUnits.Count + PlayerUnits.Count;
        _battleState = BattleState.Init;
    }

    void Update()
    {
        // if (_isHandlerBusy) return;

        switch (_battleState)
        {
            case BattleState.Init:
                _battleState = BattleState.Busy;
                // 初期設定
                InitBattleData();
                break;
            case BattleState.Intro:
                _battleState = BattleState.Busy;
                // 戦闘開始Animation再生
                PlayBattleIntro();
                break;
            case BattleState.Roulette:
                _battleState = BattleState.WaitForCommand;
                // QTE実行
                PlayRoulette();
                break;
            case BattleState.RouletteEnd:
                _battleState = BattleState.Busy;
                // 行動順決定
                SortBattleUnits();
                // ターンテーブルUI表示
                InitTurnTable();
                break;
            case BattleState.TurnStart:
                _battleState = BattleState.WaitForCommand;
                // 行動Unit取得
                GameObject unit = _battleUnitQueue.Peek();
                BattleUnitTakeAction(unit);
                break;
            case BattleState.TurnEnd:
                _battleState = BattleState.Busy;
                // ターン終了処理
                EndTurn();
                break;
            case BattleState.BattleEnd:
                break;
            case BattleState.Busy:
            case BattleState.WaitForCommand:
            default:
                break;
        }
    }

    /*
    IEnumerator BattleLoop()
    {
        // 初期データ用意
        InitBattleData();

        // 戦闘導入演出
        PlayBattleIntro();

        // QTE実行
        PlayRoulette();

        // 行動順決定
        SortBattleUnits();

        // QTE後の演出
        // UIのTurnTableを更新
        UpdateTurnTable(_battleUnits);

        // 戦闘キャラクターのindex
        int battleUnitIndex = 0;
        while (!_isBattleOver)
        { 
            GameObject unitGo = _battleUnits[battleUnitIndex];
            BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
            if(unit is BattleUnitPlayer)
            {
                // プレイヤー行動の場合、メニューを表示して、操作を受け付ける
                BattleUnitActionBase unitAction = null;
                ShowCommandMenu((p) => { unitAction = p; });
                //PlayerChooseCommand();
                yield return new WaitUntil(() => unitAction != null);
                Debug.Log("Player Action Selected");
                //PlayerDoUnitAction();
            }
            else
            {
                // プレイヤー以外が行動する場合、生存状態である場合だけAI設定に従って行動する
                if (!unit._isDead)
                {
                    
                }
            }
            if (battleUnitIndex >= _battleUnitCount - 1)
            {
                battleUnitIndex = 0;
            }
            else
            {
                // One More条件を満たさない場合、次のユニットが行動する
                if (!_playerOneMore)
                {
                    battleUnitIndex++;
                }
            }
        }
        //yield return null;
    }
    */

    void InitBattleData()
    {
        // TODO 初期化処理
        _battleState = BattleState.Intro;
    }
    /// <summary>
    /// 戦闘導入演出を再生する
    /// </summary>
    /// <returns></returns>
    void PlayBattleIntro()
    {
        _startText.SetActive(true);
        _director.Play();
    }

    /// <summary>
    /// 戦闘導入演出終了時に呼び出される
    /// </summary>
    public void OnBattleIntroFinished()
    {
        _startText.SetActive(false);
        _battleState = BattleState.Roulette;
    }
    
    /// <summary>
    /// 【DEBUG】 QTE開始
    /// </summary>
    public void PlayRoulette()
    {
        _roulettePanel.SetActive(true);
        _rouletteResult = RouletteStatus.Rolling;
    }
    public void SetRouletteResult(string rst)
    {
        switch (rst)
        {
            case "Miss":
                _rouletteResult = RouletteStatus.Miss;
                break;
            case "Good":
                _rouletteResult = RouletteStatus.Good;
                break;
            case "Excellent":
                _rouletteResult = RouletteStatus.Excellent;
                break;
            default:
                break;
        }
        _roulettePanel.SetActive(false);
        _battleState = BattleState.RouletteEnd;
    }

    void SortBattleUnits()
    {
        // QTE判定がMiss以外の場合
        if(_rouletteResult != RouletteStatus.Miss)
        {
            // エネミーを先にターンテーブルに入れて、ソートする
            _battleUnitList.AddRange(EnemyUnits);
            _battleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            // プレイヤーを戦闘に差し込む
            _battleUnitList.InsertRange(0, PlayerUnits);
            _battleUnitQueue = new Queue<GameObject>(_battleUnitList);
        }
        else
        {
            // QTE判定がMissの場合、
            // エネミーとプレイヤーを同じリストに入れて、ソートする
            _battleUnitList.AddRange(EnemyUnits);
            _battleUnitList.InsertRange(0, PlayerUnits);
            _battleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            _battleUnitQueue = new Queue<GameObject>(_battleUnitList);
        }
        //
        // foreach (GameObject unit in _battleUnitList)
        // {
        //     Debug.Log("Unit Name: " + unit.GetComponent<BattleUnitBase>()._unitName);
        // }
    }
    /// <summary>
    /// メニュー表示し、プレイヤー
    /// </summary>
    /// <param name="setUnitAction"></param>
    void PlayerChooseCommand(Action<BattleUnitActionBase> setUnitAction)
    {
        BattleUnitActionBase rst = new BattleUnitActionBase();
        setUnitAction.Invoke(rst);
    }
    void ShowCommandMenu()
    {
        _commandPanel.SetActive(true);
    }
    
    /// <summary>
    /// TODO TurnTableの更新Animation再生
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayTurnTableUpdate()
    {
        return null;
    }

    public void InitTurnTable()
    {
        _turnTable.SetActive(true);
        foreach (var unitGo in _battleUnitList)
        {
            BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
            unit._icon.gameObject.SetActive(true);
            unit._icon.transform.SetParent(_turnTable.transform);
            unit._icon.transform.SetAsFirstSibling();
        }
        _battleState = BattleState.TurnStart;
    }
    public void UpdateTurnTable(GameObject unitGo)
    {
        unitGo.GetComponent<BattleUnitBase>()._icon.transform.SetAsLastSibling();
    }

    void BattleUnitTakeAction(GameObject unitGo)
    {
        BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
        if (unit is BattleUnitPlayer)
        {
            ShowCommandMenu();
        }
        else
        {
            EnemyTakeAction(unitGo);
        }
    }

    public void OnPlayerChooseAction()
    {
        DeactiveCommandPanel();
        ActiveTargetSelectText();
        _battleState = BattleState.WaitForTargetSelect;
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    public void EndTurn()
    {
        // 行動隊列の先頭unitを行動配列の最後尾に移動
        GameObject unitToDequeue = _battleUnitQueue.Dequeue();
        _battleUnitQueue.Enqueue(unitToDequeue);

        // ターンテーブル更新
        UpdateTurnTable(unitToDequeue);

        _battleState = BattleState.TurnStart;
    }
    
    public void DeactiveCommandPanel()
    {
        _commandPanel.SetActive(false);
    }
    public void ActiveTargetSelectText()
    {
        _targetSelectText.SetActive(true);
    }
    public void DeactiveTargetSelectText()
    {
        _targetSelectText.SetActive(false);
    }
    public void EnemyTakeAction(GameObject unitGo)
    {
        // テスト用
        unitGo.GetComponent<BattleUnitEnemyBase>().DoAttack1(PlayerUnits[0]);
    }
    
    public void TestInitializer()
    {
    }
}
