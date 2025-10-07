using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_UI : ColorPallet, IUI
{
    [SerializeField] private ObjectManager _objectManager;
    [SerializeField] private GameObject[] i_SkillSlot;
    [SerializeField] private GameObject i_SkillDescription;
    [SerializeField] private GameObject i_FinishingBlowSlot;
    [SerializeField] private GameObject[] i_TurnTableSlot;
    private Animator _animator;
    private string _chooseSkillName;
    private GameObject _targetObj = null;
    private BattleUnitPlayer _battleUnitPlayer = null;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    void OnEnable()
    {
        _animator.enabled = true;
        _animator.Play("OnEnable_Skill");
    }

    // (C)CommandButtoからのOnClickで呼び出される。→ そのうち、(P)Skill > (B)SkillSlotの押下で呼び出したい
    public void OnSkillButtonPressed(string chooseSkillName)
    {
        // どの技を選んだのかフィールドに残しておく
        _chooseSkillName = chooseSkillName;
        // 下のメソッドはChouUIテスト用に残してあるだけで、本実装用のメソッドを作ったら置き換えてしまって欲しい。
        // スキル選択UIからターゲット選択UIに切り替わる処理を書く。Animationを使ってもSetActiveを使っても実装の仕方は問わない。
        _objectManager.UIController.DeactivateCommandPanel();
        _objectManager.UIController.ShowTargetSelectText();
        _objectManager.BattleLoopHandler.BattleState = BattleState.WaitForTargetSelect;
    }
    public void SetTargetObject(GameObject targetObj) => _targetObj = targetObj;

    void OnDisable()
    {
        _animator.enabled = false;
        if (_objectManager.PlayerUnits[0])
        {
            _objectManager.PlayerUnits[0].DecidePlayerMove(_chooseSkillName, _targetObj);
        }
    }
    public void DeactiveAnimator()
    {
        //AnimEventのエラーを回避
    }
}