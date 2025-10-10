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
    // 行なうこと → テキスト管理の仕方を色んな人に聞いてみるべき
    public void ActiveText()
    {
        _activeText?.Invoke();
    }
    
    //クリックするとテキストが出現し、クリックするとテキストが消滅する？ → 違う、クリックするとテキストが消滅してその後にテキストが現れる。
    //しかし、こうすると一つの問題が生まれてしまう。それは、ハイドするときのアニメーションが余分に再生されてしまうということ。
    //でも、文字が無ければ大丈夫なのかもしれない。いや、でも、それはIntruductionだけの話で他のAnimationだとImage画像の消える部分が見えてしまったりすることがある。
    //
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
        Time.timeScale = 10;
        _textMeshProUGUI = _speacBubbleTextObject.GetComponent<TextMeshProUGUI>();
        _currentText = _speachBubbleTextAsset.text.Split('\n');
        _currentIndex = 0;
    }
    
    public void FadeOut()
    {
        _fadePanel.DOColor(Color.clear, 10);
    }

    public void OnShowIntroductionText()
    {
        _animator.Play(_showClientTextKey);
        _textMeshProUGUI.text = _currentText[_currentIndex];
        _currentIndex++;
    }

    public void OnHideIntroductionText()
    {
        _animator.Play(_hideClientTextKey);
        if (_currentText[_currentIndex].Length == 1) //何も書いていないなら
        {
            Time.timeScale = 1;
            FadeOut();
            DOVirtual.DelayedCall(10, InsertSpeechBubble);
            
        }
    }

    public void OnShowSpeechBubbleText()
    {
        Debug.Log("");
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
}