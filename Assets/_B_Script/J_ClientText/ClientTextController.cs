using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClientTextController : MonoBehaviour
{ 
    [SerializeField] private TextAsset _introductionTextAsset;
    [SerializeField] private TextAsset _speachBubbleTextAsset;
    [SerializeField] private TextAsset _phoneTextAsset;
    [SerializeField] private GameObject _introductionTextObject;
    [SerializeField] private GameObject _speacBubbleTextObject;
    [SerializeField] private GameObject _phoneTextobject;
    [SerializeField] Image _fadePanel;
    [SerializeField] Image _PhonePanel;
    TextMeshProUGUI _textMeshProUGUI;
    private Action _activeText;
    private Animator _animator;
    private string _showClientTextKey;
    private string _hideClientTextKey;
    private string[] _currentText;
    private int _currentIndex = 0;
    private void Awake()
    {
        Time.timeScale = 10;
        _animator = GetComponent<Animator>();
        InsertIntroduction();
    }
    
    //クリックするとテキストが出現し、クリックするとテキストが消滅する？ → 違う、クリックするとテキストが消滅してその後にテキストが現れる。
    //しかし、こうすると一つの問題が生まれてしまう。それは、ハイドするときのアニメーションが余分に再生されてしまうということ。
    //でも、文字が無ければ大丈夫なのかもしれない。いや、でも、それはIntruductionだけの話で他のAnimationだとImage画像の消える部分が見えてしまったりすることがある。
    //
    // ステートパターン方式で書く事ができそう
    public void InsertIntroduction()
    {
        _showClientTextKey = "ShowIntroductionText";
        _hideClientTextKey = "HideIntroductionText";
        Time.timeScale = 10;
        _textMeshProUGUI = _introductionTextObject.GetComponent<TextMeshProUGUI>();
        _currentText = _introductionTextAsset.text.Split('\n');
        _currentIndex = 0;
    }

    public void InsertSpeechBubble()
    {
        _showClientTextKey = "ShowSpeechBubbleText";
        _hideClientTextKey = "HideSpeechBubbleText";
        _textMeshProUGUI = _speacBubbleTextObject.GetComponent<TextMeshProUGUI>();
        _currentText = _speachBubbleTextAsset.text.Split('\n');
        _currentIndex = 0;
    }
    
    public void FadeIn()
    {
        _fadePanel.DOColor(Color.white, 10);
    }
    
    public void FadeOut()
    {
        _fadePanel.DOColor(Color.clear, 10);
    }
    /// <summary>
    /// Client用のテキストをAnimationEventで表示させる
    /// </summary>
    public void AnimEventShowIntroductionText()
    {
        _animator.Play(_showClientTextKey);
        _textMeshProUGUI.text = _currentText[_currentIndex];
        _currentIndex++;
    }
    /// <summary>
    /// Client用のテキストをOnClickから非表示にする
    /// </summary>
    public void OnHideIntroductionText()
    {
        _animator.Play(_hideClientTextKey);
        if (_currentText[_currentIndex].Length == 1) //何も書いていないなら
        {
            Time.timeScale = 1;
            FadeOut();
            DOVirtual.DelayedCall(12, InsertSpeechBubble);
            DOVirtual.DelayedCall(13, StartClientAnimation); //15秒後にAnimationを再生する
        }
    }

    public void OnShowSpeechBubbleText()
    {
        _animator.Play(_showClientTextKey);
        _textMeshProUGUI.text = _currentText[_currentIndex];
        _currentIndex++;
    }

    public void OnHideSpeechBubbleText()
    {
        if (_currentText[_currentIndex].Length == 1) //何も書いていないなら
        {
            Time.timeScale = 1;
            FadeOut();
            InsertSpeechBubble();
        }
        _animator.Play(_hideClientTextKey);
    }
    /// <summary>
    /// Client用のUIアニメーターを再生する
    /// </summary>
    private void StartClientAnimation()
    {
        _animator.Play(_hideClientTextKey);
    }

    public void ActivePhone()
    {
        _animator.Play("ActivePhone");
    }

    public void InactivePhone()
    {
        _animator.Play("InactivePhone");
    }
}