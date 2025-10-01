using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
/// <summary>
/// BattleUIController.csからOnEnable一度だけ呼び出される
/// </summary>
public sealed class QTE_UI : MonoBehaviour
{
    [Header("QTE発生タイミング（ランダム）")]
    [SerializeField] private float _timingMini;
    [SerializeField] private float _timingMax;
    [Header("QTE発生からのウェイトタイム"), Range(1f, 10f)] 
    [SerializeField] private float _waitTime;
    [Header("Pinの出現位置（Rotation）をランダムにするか否か")]
    [SerializeField] private bool _isPinGenerateRandum;
    [Header("QTE出現時間（ランダム）"), Range(0.1f, 2f)] 
    [SerializeField] private float _showMini;
    [Range(0.1f, 5f)]
    [SerializeField] private float _showMax;
    [Header("Good判定を取れる面積（ランダム）"), Range(0.1f, 1f)]
    [SerializeField] private float _circle_Good_FillAmountMini; //Imgae.fillAmount
    [Range(0.1f, 1f)]
    [SerializeField] private float _circle_Good_FillAmountMax;
    [Header("Excellent判定を取れる面積（固定値）"), Range(0.01f, 0.1f)]
    [SerializeField] private float[] _circle_Excellent_FillAmountArray;
    [Header("(C)QTE下にあるオブジェクトをすべてアタッチ")]
    [SerializeField] private GameObject i_QTE_Pin;
    [SerializeField] private GameObject i_QTE_Circle_Good;
    [SerializeField] private GameObject i_QTE_Circle_Excellent;
    [SerializeField] private ObjectManager _objectManager; //取得先がないので注意
    private Animator _animator;
    private PlayableGraph _playableGraph;
    private bool _isCanQTEEvent = false;
    private void Awake()
    {
        i_QTE_Pin = transform.GetChild(0).gameObject;
        i_QTE_Circle_Good = transform.GetChild(2).gameObject;
        i_QTE_Circle_Excellent = transform.GetChild(3).gameObject;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    /// <summary>
    /// ランダムなタイミングにAnimationEvnetを挿入し、QTEの発動タイミングを決定する
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
    /// QTEのランダム要素を決定し、最後にQTEのアニメーションを再生する。OnEnableにて生成されたAnimationEVentから一度だけ呼び出される。
    /// </summary>
    private void QTE()
    {
        Debug.Log("QTE");
        if (_playableGraph.IsValid())
        {
            _playableGraph.Stop();
            _playableGraph.Destroy();
        }
        //<ランダムで決める要素>
        //ピンの速度（QTE出現時間）
        float qteTime = Random.Range(_showMini, _showMax);
        //Good判定の面積を決定 → ※重み付けにする
        float goodFillAmount = Random.Range(_circle_Good_FillAmountMini, _circle_Good_FillAmountMax);
        //Excellentエリアの面積を配列の中から決定 → ※重み付けにする
        float excellentFillAmount = _circle_Excellent_FillAmountArray[Random.Range(0, _circle_Excellent_FillAmountArray.Length)];
        //Goodの出現位置
        float goodRightEdge = Random.Range(360 * _circle_Good_FillAmountMax, 360); //理不尽な場所に出現する可能性があるので最小値のみ制限を設ける
        //Excellentの出現位置
        float excellentRightEdge = Random.Range(goodRightEdge - 360 * goodFillAmount + 360 * excellentFillAmount, goodRightEdge); //Excellentの右端（出現位置） == (goodの左端 + 360 * excellentFillAmount), goodの右端
        //<ランダムに決まった値をオブジェクトやアニメーションクリップに反映>
        i_QTE_Circle_Good.transform.localEulerAngles = new Vector3(0, 0, goodRightEdge);
        i_QTE_Circle_Excellent.transform.localEulerAngles = new Vector3(0, 0, excellentRightEdge);
        i_QTE_Circle_Good.GetComponent<Image>().fillAmount = goodFillAmount;
        i_QTE_Circle_Excellent.GetComponent<Image>().fillAmount = excellentFillAmount;
        AnimationCurve curvePinRotationZ = AnimationCurve.Linear(_waitTime, 0f, _waitTime + qteTime, 0f + 360f);
        //<PlayableGraphの作成>
        //"QTE"という名前のついたanimationClipからコピーを新規作成
        AnimationClip animationClip = new AnimationClip();
        //QTE時間内にボタンが押されなかったときのAnimationEvent(Miss判定)を登録する
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = _waitTime + qteTime;
        animEvent.functionName = "JudgmentQTEResult";
        animEvent.stringParameter = "Miss";
        animationClip.AddEvent(animEvent);
        // animationClipにanimationCurveを挿入する
        animationClip.SetCurve(i_QTE_Pin.name, typeof(Transform), "localEulerAngles.z", curvePinRotationZ);
        _playableGraph = PlayableGraph.Create();
        AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(_playableGraph, animationClip);
        AnimationPlayableOutput animationPlayableOutput = AnimationPlayableOutput.Create(_playableGraph, "AnimOutput", _animator);
        animationPlayableOutput.SetSourcePlayable(animationClipPlayable);
        //再生
        _animator.enabled = true;
        _playableGraph.Play();
        //音源の再生（AnimationEventでも良い）
        _isCanQTEEvent = true;
    }
    /// <summary>
    /// 確率に重みをつける処理（フェーズ１終了時に追加でよい）。
    /// </summary>
    /// <returns></returns>
    private float AddWeighted()
    {
        return 0;
    }
    /// <summary>
    /// 最適化したいのであればここのメソッドを別MonoBehaviourにすること
    /// </summary>
    private void Update()
    {
        if (_isCanQTEEvent)
        {
            //実行環境がPCの時のみコンパイラが走る
// #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _isCanQTEEvent = false;
                JudgmentQTEResult();
            }
            //実行環境がモバイル端末の時のみコンパイラが走る
// #elif UNITY_ANDROID || UNITY_IOS
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                _isCanQTEEvent = false;
                JudgmentQTEResult();
            }
// #endif
        }
    }
    /// <summary>
    /// QTEPinの角度を取得し、QTE判定を取得する。Pinをストップさせるボタンを押した時に一度だけ呼び出される
    /// </summary>
    void JudgmentQTEResult()
    {
        string result = "";
        if (i_QTE_Pin.transform.rotation.eulerAngles.z <= i_QTE_Circle_Excellent.transform.rotation.eulerAngles.z
            &&
            i_QTE_Circle_Excellent.transform.rotation.eulerAngles.z - 360 * i_QTE_Circle_Excellent.GetComponent<Image>().fillAmount <= i_QTE_Pin.transform.rotation.eulerAngles.z)
        {
            result = "Excellent";
        }
        else if (i_QTE_Pin.transform.rotation.eulerAngles.z <= i_QTE_Circle_Good.transform.rotation.eulerAngles.z
                 &&
                 i_QTE_Circle_Good.transform.rotation.eulerAngles.z - 360 * i_QTE_Circle_Good.GetComponent<Image>().fillAmount <= i_QTE_Pin.transform.rotation.eulerAngles.z)
        {
            result = "Good";
        }
        else
        {
            result = "Miss";
        }
        _objectManager.BattleEventController.QTEEnd(result);
    }
    /// <summary>
    /// _objectManager.BattleEventController.QTEEnd後にメソッド経由で一度だけ呼び出される
    /// </summary>
    void OnDisable()
    {
        if (_playableGraph.IsValid())
        {
            _playableGraph.Stop();
            _playableGraph.Destroy();
        }
        _animator.enabled = false;
        //この後、QTE確定後の演出（Animation）は違うところで呼び出すこと
    }
}