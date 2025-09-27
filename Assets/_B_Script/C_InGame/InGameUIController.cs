/// <summary>
/// OnClick()�ŌĂяo���O��
/// Panel���ɑΉ�����Manager���쐬���A�Ώ�Panel���V���A���C�Y�ɓn��
/// </summary>

using UnityEngine;

public class InGamePanelController : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    void Start()
    {
        if (!_panel) Debug.LogError("Panel���A�^�b�`���Ă��������B");
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
