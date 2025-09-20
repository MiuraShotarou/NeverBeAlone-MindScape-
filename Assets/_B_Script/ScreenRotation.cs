using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRotation : MonoBehaviour
{
    /// <summary>
    /// 縦向き
    /// </summary>
    [SerializeField] bool _portrait = false;
    /// <summary>
    /// 上下逆
    /// </summary>
    [SerializeField] bool _upsideDown = false;
    /// <summary>
    /// 横向き（反時計回り）
    /// </summary>
    [SerializeField] bool _left = true;
    /// <summary>
    /// 横向き（時計回り）
    /// </summary>
    [SerializeField] bool _right = true;

    ///<summary>
    /// 画面の向きを設定する。
    /// </summary>
    void SetRotation()
    {
        Screen.autorotateToPortrait = _portrait;
        Screen.autorotateToPortraitUpsideDown = _upsideDown;
        Screen.autorotateToLandscapeLeft = _left;
        Screen.autorotateToLandscapeRight = _right;

        // 一方向に固定する場合はScreenOrientation.LandscapeLeftまたはLandscapeRightを代入
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // void Start()
    // {

    // }

    // void Update()
    // {
    //     SetRotation();
    // }
}
