using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone_UI : ColorPallet
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
        _animator.Play("OnEnable_Phone");
        //上のアニメーションが再生された後、「感情の譲渡」「協力者からの支援」が発動したタイミングに該当のアニメーションを再生する処理を書け
    }

    /// <summary>
    /// スワイプ操作をした時にTransition_PhoneAnimationクリップが再生される処理を書け。ちなみに、必ずしもアニメーションで表現しなくても良い
    /// </summary>
    void StartTransition_PhoneAnimation()
    {
        
    }
    void OnDisable()
    {
        _animator.enabled = false;
    }
}