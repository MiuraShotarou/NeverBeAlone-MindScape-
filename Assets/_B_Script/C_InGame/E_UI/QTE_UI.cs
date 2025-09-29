using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

/// <summary>
/// BattleUIController.csからOnEnable一度だけ呼び出される
/// </summary>
public sealed class QTE_UI : MonoBehaviour
{
    [Header("QTE発生タイミング（振れ幅）")]
    [SerializeField] private float _timingMini;
    [SerializeField] private float _timingMax;
    [Header("Pinの出現位置（Rotation）をランダムにするか否か")]
    [SerializeField] private bool _isPinGenerateRandum;
    [Header("QTE出現時間（振れ幅）"), Range(0.1f, 2f)] 
    [SerializeField] private float _showMini;
    [SerializeField] private float _showMax;
    [Header("Good判定を取れる面積（振れ幅）"), Range(0.1f, 1f)]
    [SerializeField] private float _circle_Good_FillAmountMini; //Imgae.fillAmount
    [SerializeField] private float _circle_Good_FillAmountMax;
    [Header("Excellent判定を取れる面積（固定値）"), Range(0.01f, 0.1f)]
    [SerializeField] private float[] _circle_Excellent_FillAmountArray;
    [Header("Good判定を取れる面積（振れ幅）")]
    [Header("(C)QTE下にあるオブジェクトをすべてアタッチ")]
    [SerializeField] GameObject i_QTE_Pin;
    [SerializeField] GameObject i_QTE_Circle_Miss;
    [SerializeField] GameObject i_QTE_Circle_Good;
    [SerializeField] GameObject i_QTE_Circle_Excellent;
    Animator _animator;
    PlayableGraph _playableGraph;
    private void Awake()
    {
        i_QTE_Pin = transform.GetChild(0).gameObject;
        i_QTE_Circle_Miss = transform.GetChild(1).gameObject;
        i_QTE_Circle_Good = transform.GetChild(2).gameObject;
        i_QTE_Circle_Excellent = transform.GetChild(3).gameObject;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    /// <summary>
    /// ランダムなタイミングにAnimationEvnetを挿入し、そこで初めてQTEが発動する
    /// </summary>
    private void OnEnable()
    {
        AnimationClip qteClip = new AnimationClip();
        AnimationEvent qteEvent = new AnimationEvent();
        //QTEの発生タイミング
        float timing = Random.Range(_timingMini, _timingMax);
        //発生タイミングをeventに登録
        qteEvent.time = timing;
        //関数の登録。文字列の検索でしか関数の呼び出しはできない
        qteEvent.functionName = "QTE"; // 呼ばれる関数名。該当するAnimatorがアタッチされているオブジェクトのMonoBehaviour付きコンポーネントをすべて確認し、文字列と一致する関数が見つかったらそれを呼び出す仕組み
        //clipにeventを追加
        qteClip.AddEvent(qteEvent);
        //PlayableGraphを作成
        _playableGraph = PlayableGraph.Create();
        //AnimationClipPlayableを作成
        AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(_playableGraph, qteClip);
        //AnimationPlayableOutputを作成してAnimatorと紐づけ
        AnimationPlayableOutput animationPlayableOutput = AnimationPlayableOutput.Create(_playableGraph, "AnimOutput", _animator);
        animationPlayableOutput.SetSourcePlayable(animationClipPlayable);
        //再生
        _animator.enabled = true;
        _playableGraph.Play();
    }
    /// <summary>
    /// OnEnableから生成されたAnimationEVentの発火で呼び出される
    /// </summary>
    private void QTE()
    {
        Debug.Log("QTE");
        //animationCurveの使い方
        // animationClip.SetCurve(
        //     relativePath: string,        // 対象のオブジェクトまでのパス
        //     type: System.Type,           // 対象コンポーネントの型
        //     propertyName: string,        // プロパティ名（"localPosition.x" など）
        //     curve: AnimationCurve        // アニメーションカーブ
        // );
        
        //<ランダムで決める要素>
        //ピンの速度（QTE出現時間）
        float qteTime = Random.Range(_showMini, _showMax);
        //Good判定の面積を決定 →　※重み付けにする
        float goodFillAmount = Random.Range(_circle_Good_FillAmountMini, _circle_Good_FillAmountMax);
        //Excellentエリアの面積を配列の中から決定 → ※重み付けにする
        float excellentFillAmount = _circle_Excellent_FillAmountArray[Random.Range(0, _circle_Excellent_FillAmountArray.Length)];
        //Goodの出現位置
        //Goodの出現位置に伴ってExellentの出現位置も決定する
        //
        //Excellentエリアの面積縮小に伴ってGoodエリアの出現位置制限（Random.Range(ExcellentAreaLeftEdge, EccellentAreaRightEdge)）
        
        
        int pinStartRotation = 0;
        AnimationCurve animationCurveX = AnimationCurve.Linear(0f, pinStartRotation, 1f, pinStartRotation + 360);//PinのrotZ
        // AnimationCurve animationCurveY = AnimationCurve.Linear(0f, _selectedPieceObj.transform.position.y, 1f, _targetSquere._SquerePiecePosition.y);
        // float adjustScale = _selectedPieceObj.transform.localScale.y + (_selectedSquere._SquereTilePos.y - _targetSquere._SquereTilePos.y) * 0.143f;
        // AnimationCurve animationCurveSX = AnimationCurve.Linear(0f, _selectedPieceObj.transform.localScale.x, 1f, adjustScale);
        // AnimationCurve animationCurveSY = AnimationCurve.Linear(0f, _selectedPieceObj.transform.localScale.y, 1f, adjustScale);
        // AnimationCurve animationCurveSZ = AnimationCurve.Linear(0f, _selectedPieceObj.transform.localScale.z, 1f, adjustScale);
        // //"Run"という名前のついたanimationClipからコピーを新規作成
        // AnimationClip animationClip = _selectedPieceRuntimeAnimator.animationClips.FirstOrDefault(clip => clip.name.Contains("Run"));
        // //新しく作成・編集したAnimationCurveをAnimationClipに代入する
        // animationClip.SetCurve("", typeof(Transform), "localPosition.x", animationCurveX);
        // animationClip.SetCurve("", typeof(Transform), "localPosition.y", animationCurveY);
        // animationClip.SetCurve("", typeof(Transform), "localScale.x", animationCurveSX);
        // animationClip.SetCurve("", typeof(Transform), "localScale.y", animationCurveSY);
        // animationClip.SetCurve("", typeof(Transform), "localScale.z", animationCurveSZ);
        //PlayableGraphを作成
        AnimationClip clip = new AnimationClip();
        AnimationEvent animEvent = new AnimationEvent();
        clip.AddEvent(animEvent); //いらないかも
        
        PlayableGraph playableGraph = PlayableGraph.Create();
        //AnimationClipPlayableを作成
        AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        //AnimationPlayableOutputを作成してAnimatorと連結
        AnimationPlayableOutput animationPlayableOutput = AnimationPlayableOutput.Create(playableGraph, "AnimOutput", _animator);
        animationPlayableOutput.SetSourcePlayable(animationClipPlayable);
        //再生
        _animator.enabled = true;//いらないかも
        playableGraph.Play();
        // if ()
    }
    /// <summary>
    /// 確率に重みをつける処理（フェーズ１終了時に追加でよい）。
    /// </summary>
    /// <returns></returns>
    float AddWeighted()
    {
        return 0;
    }
}