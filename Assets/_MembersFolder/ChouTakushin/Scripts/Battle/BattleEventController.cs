using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleEventController : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    [HideInInspector] public List<BattleUnitBase> BattleUnitList = new List<BattleUnitBase>();
    private BattleLoopHandler _loopHandler;
    private BattleUIController _uiController = default;
    private PlayableDirector _director;

    /// <summary>
    /// デリゲート
    /// 　戦闘Unitの死活判定処理
    /// </summary>
    public delegate void JudgeSurvival();
    public JudgeSurvival DoJudgeSurvival;
    // Start is called before the first frame update
    void Start()
    {
        _loopHandler = _objects.BattleLoopHandler;
        _uiController = _objects.UIController;
        _director = _objects.PlayableDirector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void InitBattleData()
    {
        // TODO 初期化処理
        _loopHandler.BattleState = BattleState.Intro;
    }
    /// <summary>
    /// 戦闘導入演出を再生する
    /// </summary>
    /// <returns></returns>
    public void PlayBattleIntro()
    {
        _uiController.ShowStartText();
        _director.Play();
    }

    /// <summary>
    /// 戦闘導入演出終了時に呼び出される
    /// </summary>
    public void OnBattleIntroFinished()
    {
        _uiController.DeactivateStartText();
        _loopHandler.BattleState = BattleState.Roulette;
    }
    
    /// <summary>
    /// 【DEBUG】 QTE開始
    /// </summary>
    public void PlayRoulette()
    {
        _uiController.ShowRoulettePanel();
        _loopHandler.RouletteResult = RouletteStatus.Rolling;
    }
    public void SetRouletteResult(string rst)
    {
        switch (rst)
        {
            case "Miss":
                _loopHandler.RouletteResult = RouletteStatus.Miss;
                break;
            case "Good":
                _loopHandler.RouletteResult = RouletteStatus.Good;
                break;
            case "Excellent":
                _loopHandler.RouletteResult = RouletteStatus.Excellent;
                break;
            default:
                break;
        }
        _uiController.DeactivateRoulettePanel();
        _loopHandler.BattleState = BattleState.RouletteEnd;
    }
    public void EnemyTakeAction()
    {
        // テスト用
        BattleUnitEnemyBase unit = (BattleUnitEnemyBase)_loopHandler.CurrentBattleUnit;
        if (!unit._isDead)
        {
            unit.DoAttack1(_objects.PlayerUnits[0]);
        }
        else
        {
            Common.LogDebugLine(this, "EnemyTakeAction()", unit._unitName + "が死んでます。ターン終了。");
            _loopHandler.BattleState = BattleState.TurnEnd;
        }
    }
    /// <summary>
    /// Unit行動が開始する
    /// </summary>
    public void BattleUnitTakeAction()
    {
        Common.LogDebugLine(this, nameof(BattleUnitTakeAction), "UnitName: " + _loopHandler.CurrentBattleUnit._unitName);
        
        if (_loopHandler.CurrentBattleUnit is BattleUnitPlayer)
        {
            Common.LogDebugLine(this, nameof(BattleUnitTakeAction), "ShowCommandMenu.");
            ShowCommandMenu();
        }
        else
        {
            Common.LogDebugLine(this, nameof(BattleUnitTakeAction), "EnemyTakeAction.");
            EnemyTakeAction();
        }
    }

    public void OnPlayerChooseAction()
    {
        _uiController.DeactivateCommandPanel();
        _uiController.ShowTargetSelectText();
        _loopHandler.BattleState = BattleState.WaitForTargetSelect;
    }
    public void InitTurnTable()
    {
        _uiController.ShowTurnTable();
        foreach (var unit in BattleUnitList)
        {
            unit._icon.gameObject.SetActive(true);
            _uiController.AddImageAsFirstSibling(_uiController.TurnTable, unit._icon);
        }
        _loopHandler.BattleState = BattleState.TurnStart;
    }
    public void UpdateTurnTable()
    {
        _uiController.SetImageAsLastSibling(_uiController.TurnTable, _loopHandler.CurrentBattleUnit._icon);
    }
    
    public void SortBattleUnits()
    {
        // QTE判定がMiss以外の場合
        if(_loopHandler.RouletteResult != RouletteStatus.Miss)
        {
            // エネミーを先にターンテーブルに入れて、ソートする
            BattleUnitList.AddRange(_objects.EnemyUnits);
            BattleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            // プレイヤーを戦闘に差し込む
            BattleUnitList.InsertRange(0, _objects.PlayerUnits);
            _loopHandler.BattleUnitQueue = new Queue<BattleUnitBase>(BattleUnitList);
        }
        else
        {
            // QTE判定がMissの場合、
            // エネミーとプレイヤーを同じリストに入れて、ソートする
            BattleUnitList.AddRange(_objects.EnemyUnits);
            BattleUnitList.InsertRange(0, _objects.PlayerUnits);
            BattleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            _loopHandler.BattleUnitQueue = new Queue<BattleUnitBase>(BattleUnitList);
        }
    }
    public void ShowCommandMenu()
    {
        _uiController.ShowCommandPanel();
    }
    
    public void JudgeUnitsSurvive()
    {
        // 各Unitの死活判定を行う
        DoJudgeSurvival.Invoke();
        // 死活判定後、戦闘終了判定を行う
        _loopHandler.JudgeBattleEnd();
    }
}
