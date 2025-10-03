using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_UI : ColorPallet, IUI
{
    Animator _animator;
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
    
    void OnDisable()
    {
        _animator.enabled = false;
    }
    public void DeactiveAnimator()
    {
        //AnimEventのエラーを回避
    }
}