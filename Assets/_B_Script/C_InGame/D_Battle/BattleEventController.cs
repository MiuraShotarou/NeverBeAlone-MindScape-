using System;
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
    /// 敵を決定する
    /// </summary>
    public void DecideEnemy()
    {
        //<仕様>
        //潜入につき１体確定でボーナスエネミーが出現する。
        //出現パターン（毎回固定）+（強敵 + ボーナスエネミー がセットになった戦闘が指定したタイミングで確率によって差し込まれる）
        //ボス戦まで計14回（強敵 + ボーナスエネミーがあった場合は計15回）の雑魚戦を挟む
        
        //<設計>
        //GameDataManager からプランナーが設定した敵の出現リストを取得する
        
        //リストにConcatか？
        EnemyData[] enemyDataArray = GameDataManager.Instance.GetEnemyData(_objects.PlayerUnits[0].Progress); //未実装
        BattleUnitList = EnemyUnitBaseConverter(enemyDataArray);
        
    }

    private List<BattleUnitBase> EnemyUnitBaseConverter(EnemyData[] enemyDataArray) //Playerと統合したい
    {
        //enemyDataArrayの中身をBattleUnitEnemyBaseの内部変数にコピーする
        // EnemyDataの中身
        BattleUnitBase[] enemyUnitArray = new BattleUnitBase[enemyDataArray.Length];
        for (int i = 0; i < enemyDataArray.Length; i++)
        {
            enemyUnitArray[i].AttackBase = enemyDataArray[i].BaseAttack;
            // enemyUnitArray[i].AttackScaleBase = enemyDataArray[i].BaseAttackScale;
            enemyUnitArray[i].DefenseBase = enemyDataArray[i].BaseDefense;
            // enemyUnitArray[i].DefenseScaleBase = enemyDataArray[i].BaseDefenseScale;
            enemyUnitArray[i].AgilityBase = enemyDataArray[i].BaseAgility;
            enemyUnitArray[i].EvadeRateBase = enemyDataArray[i].BaseEvadeRate;
            enemyUnitArray[i].CriticalRateBase = enemyDataArray[i].BaseCriticalRate;
            enemyUnitArray[i].CriticalScaleBase = enemyDataArray[i].BaseCriticalScale;
            enemyUnitArray[i].HealScaleBase = enemyDataArray[i].BaseHealScale;
            enemyUnitArray[i].MaxHp = enemyDataArray[i].MaxHp;
            enemyUnitArray[i].Hp = enemyDataArray[i].Hp;
            enemyUnitArray[i].Progress = enemyDataArray[i].Progress;
            enemyUnitArray[i].IsDead = enemyDataArray[i].IsDead;
            enemyUnitArray[i].CurrentEmotion = enemyDataArray[i].CurrentEmotion;
            enemyUnitArray[i].SkillEffects = enemyDataArray[i].SkillEffects;
            enemyUnitArray[i].ConditionFlag = enemyDataArray[i].ConditionFlag;
            enemyUnitArray[i].EmotionLevels = enemyDataArray[i].EmotionLevels;
            enemyUnitArray[i].HasSkillDict = enemyDataArray[i].HasSkillDict;
            enemyUnitArray[i]._skill = enemyDataArray[i]._skill;
        }
        return null;
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
        Debug.Log(rst);
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
    /// <summary>
    /// QTE判定後に行動順を決定する。
    /// </summary>
    public void SortBattleUnits()
    {
        // QTE判定がMiss以外の場合
        if(_loopHandler.ResultQTE != ResultQTE.Miss)
        {
            // エネミーを先にターンテーブルに入れて、ソートする
            BattleUnitList.AddRange(_objects.EnemyUnits);
            BattleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>().AgilityBase - a.GetComponent<BattleUnitBase>().AgilityBase;
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
                return b.GetComponent<BattleUnitBase>().AgilityBase - a.GetComponent<BattleUnitBase>().AgilityBase;
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