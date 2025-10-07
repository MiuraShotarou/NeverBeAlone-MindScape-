using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BuildScene : MonoBehaviour
{
    public void OnBattleScenePressed()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void OnTitlePressed()
    {
        SceneManager.LoadScene("Title");
    }
    public void OnInGamePressed()
    {
        SceneManager.LoadScene("InGame");
    }
    public void OnSetUpPressed()
    {
        SceneManager.LoadScene("SetUp");
    }

    public void OnAddressableLoadPressed(string nameKey)
    {
        _ = AddressableLoad(nameKey);
    }
    public async Task AddressableLoad(string nameKey)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(nameKey);
        GameObject roadPrefab = await handle.Task;
        Instantiate(roadPrefab, new Vector3(0.537f, 1.887f, -8.738f), Quaternion.identity);
    }
}
