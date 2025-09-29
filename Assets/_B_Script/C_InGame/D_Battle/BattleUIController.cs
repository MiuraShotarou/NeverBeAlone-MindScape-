using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// InGameSceneのUIを制御する。UI起動の原点（中枢）として機能させること
/// </summary>
public class BattleUIController : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManager;
    [SerializeField] public GameObject CommandPanel = default;
    [SerializeField] public GameObject TurnTable = default;
    [SerializeField] private Text _victoryText;
    [SerializeField] private Text _gameoverText;
    [SerializeField] private Text _startText = default;
    [SerializeField] private Text _targetSelectText = default;
    [Header("QTE_UI.cs")]
    [SerializeField] private GameObject _QTEPanel = default;
    [Header("TransitionPhoneController.cs")]
    [SerializeField] private GameObject _phonePanel = default;

    public void ShowStartText()
    {
        _startText.gameObject.SetActive(true);
    }
    public void DeactivateStartText()
    {
        _startText.gameObject.SetActive(false);
    }
    public void ShowTargetSelectText()
    {
        _targetSelectText.gameObject.SetActive(true);
    }
    public void DeactivateTargetSelectText()
    {
        _targetSelectText.gameObject.SetActive(false);
    }
    public void ShowVictoryText()
    {
        _victoryText.gameObject.SetActive(true);
    }
    public void DeactivateVictoryText()
    {
        _victoryText.gameObject.SetActive(true);
    }
    public void ShowGameOverText()
    {
        _gameoverText.gameObject.SetActive(true);
    }
    public void DeactivateGameOverText()
    {
        _victoryText.gameObject.SetActive(true);
    }
    /// <summary>
    /// BattleState.QTE 時にマイフレーム呼び出される
    /// </summary>
    public void ActuiveQTEPanel()
    {
        _QTEPanel.SetActive(true);
    }
    /// <summary>
    /// QTEEnd関数の最後にて一度だけ呼び出される
    /// </summary>
    public void DeactivateRoulettePanel()
    {
        _QTEPanel.SetActive(false);
    }
    public void ShowCommandPanel()
    {
        CommandPanel.SetActive(true);
    }
    public void DeactivateCommandPanel()
    {
        CommandPanel.SetActive(false);
    }
    public void ShowTurnTable()
    {
        TurnTable.SetActive(true);
    }
    public void DeactivateTurnTable()
    {
        TurnTable.SetActive(false);
    }

    public void AddImageAsFirstSibling(GameObject parent, Image image)
    {
        image.transform.SetParent(parent.transform);
        image.transform.SetAsFirstSibling();
    }

    public void SetImageAsLastSibling(GameObject parent, Image image)
    {
        image.transform.SetAsLastSibling();
    }
}