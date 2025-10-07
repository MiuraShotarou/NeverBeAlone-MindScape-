using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class BattleEventController : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    // SortBattleUnitsが呼ばれないかぎりnull → そもそも削除したい
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

    void Start()
    {
        _loopHandler = _objects.BattleLoopHandler;
        _uiController = _objects.UIController;
        _director = _objects.PlayableDirector;
    }

    public void InitBattleData()
    {
        // TODO 初期化処理
        var PlayerUnit = BattleUnitList.Where(unit => unit is BattleUnitPlayer); //新規作成でPlayerUnitを作成しても良いかもね？
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
        _loopHandler.BattleState = BattleState.QTE;
    }
    
    /// <summary>
    /// 【DEBUG】 QTE開始
    /// </summary>
    public void QTEStart()
    {
        _uiController.ActiveQTEPanel();
        _loopHandler.ResultQTE = ResultQTE.Rolling;
    }
    public void QTEEnd(string rst)
    {
        switch (rst)
        {
            case "Miss":
                _loopHandler.ResultQTE = ResultQTE.Miss;
                break;
            case "Good":
                _loopHandler.ResultQTE = ResultQTE.Good;
                break;
            case "Excellent":
                _loopHandler.ResultQTE = ResultQTE.Excellent;
                break;
            default:
                break;
        }
        _uiController.DeactivateQTEPanel();
        _loopHandler.BattleState = BattleState.QTEFinished;
        // Debug.Log(_loopHandler.ResultQTE);
    }
    public void EnemyTakeAction()
    {
        // テスト用
        BattleUnitEnemyBase unit = (BattleUnitEnemyBase)_loopHandler.CurrentBattleUnit;
        if (!unit.IsDead)
        {
            unit.DoAttack1(_objects.PlayerUnits[0]);
        }
        else
        {
            CommonUtils.LogDebugLine(this, "EnemyTakeAction()", unit._unitName + "が死んでます。ターン終了。");
            _loopHandler.BattleState = BattleState.TurnEnd;
        }
    }
    /// <summary>
    /// Unit行動が開始する
    /// </summary>
    public void BattleUnitTakeAction()
    {
        CommonUtils.LogDebugLine(this, nameof(BattleUnitTakeAction), "UnitName: " + _loopHandler.CurrentBattleUnit._unitName);
        
        if (_loopHandler.CurrentBattleUnit is BattleUnitPlayer)
        {
            CommonUtils.LogDebugLine(this, nameof(BattleUnitTakeAction), "ShowCommandMenu.");
            ShowCommandMenu();
        }
        else
        {
            CommonUtils.LogDebugLine(this, nameof(BattleUnitTakeAction), "EnemyTakeAction.");
            EnemyTakeAction();
        }
    }

    public void InitTurnTable()
    {
        _uiController.ActiveBattleUI();
        //↓ここでUI制御を行うのは設計的によくない↓
        foreach (var unit in BattleUnitList)
        {
            unit.Icon.gameObject.SetActive(true);
            _uiController.AddImageAsFirstSibling(_uiController.TurnTable, unit.Icon);
        }
        _loopHandler.BattleState = BattleState.TurnStart;
    }
    public void UpdateTurnTable()
    {
        _uiController.SetImageAsLastSibling(_uiController.TurnTable, _loopHandler.CurrentBattleUnit.Icon);
    }
    
    public void SortBattleUnits()
    {
        // QTE判定がMiss以外の場合
        if(_loopHandler.ResultQTE != ResultQTE.Miss)
        {
            // エネミーを先にターンテーブルに入れて、ソートする
            BattleUnitList.AddRange(_objects.EnemyUnits);
            BattleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>().Dex - a.GetComponent<BattleUnitBase>().Dex;
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
                return b.GetComponent<BattleUnitBase>().Dex - a.GetComponent<BattleUnitBase>().Dex;
            });
            _loopHandler.BattleUnitQueue = new Queue<BattleUnitBase>(BattleUnitList);
        }
    }
    public void ShowCommandMenu()
    {
        _uiController.ShowCommandPanel();
        _uiController.ActiveBattleUI();
    }
    
    public void JudgeUnitsSurvive()
    {
        // 各Unitの死活判定を行う
        DoJudgeSurvival.Invoke();
        // 死活判定後、戦闘終了判定を行う
        _loopHandler.JudgeBattleEnd();
    }
}