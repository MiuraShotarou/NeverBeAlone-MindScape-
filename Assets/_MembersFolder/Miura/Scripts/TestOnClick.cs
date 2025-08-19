using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestOnClick : MonoBehaviour
{
    public void OnInGamePressed()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void OnSetUpPressed()
    {
        SceneManager.LoadScene("SetUpScene");
    }
}
