using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_UI : ColorPallet, IUI
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
        //条件に応じて、二つのアニメーションクリップのうち該当するほうのアニメーションだけ再生する処理を書け
        _animator.Play("OnEnable_EmotionChange_Status");
        // _animator.Play("OnEnable_UnEmotionChange_Status");
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
