using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI
{
    /// <summary>
    /// メソッドを実装する必要はないが、アニメーションが再生されたら必ずAnimatorのenableをfalseにするようプログラムを構築すること
    /// </summary>
    public void DeactiveAnimator();
}
