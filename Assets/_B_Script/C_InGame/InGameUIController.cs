/// <summary>
/// OnClick()で呼び出す前提
/// Panel毎に対応したManagerを作成し、対象Panelをシリアライズに渡す
/// </summary>

using UnityEngine;

public class InGamePanelController : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    void Start()
    {
        if (!_panel) Debug.LogError("Panelをアタッチしてください。");
    }

    public void ShowPanel()
    {
        _panel.SetActive(true);
    }

    public void HidePanel()
    {
        _panel.SetActive(false);
    }
}
