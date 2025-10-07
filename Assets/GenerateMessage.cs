using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
/// <summary>
/// Addressableからcsvデータを取得し文字列に変化して使えるようにするスクリプト。
/// 今は機能と情報の保持を統一しているが、いずれ最適化したい
/// </summary>
public class GenerateMessage : MonoBehaviour
{
    
    
    
    
    
    
    [SerializeField] TextAsset[] _csvArray;
    Dictionary<string, TextAsset> _csvDictionary;
    TextMeshProUGUI _tmp;
    string[] _textArray; //一つの大幕で表示される文章をすべて配列に格納している

    void Awake()
    {
        _csvDictionary = _csvArray.ToDictionary(csv => csv.name.Split('_')[0], csv => csv);
    }
    /// <summary>
    /// テスト用Startメソッド
    /// </summary>
    void Start()
    {
        //普通に代入すると、csvの中身がそのまま出力されてしまう
        //string( or int )をキーに指定したDictionaryを作成する
        _tmp = GetComponent<TextMeshProUGUI>();
        string key = _csvArray[0].name.Split('_')[0];
        _textArray = _csvDictionary[key].text.Split('\n');
    }
    /// <summary>
    /// Addresssableからのロードにアセットの名前を必要とする
    /// それさえも自動で行えるツールみたいなのが欲しい
    /// ロードしたオブジェクトを解放する処理を書いていない
    /// </summary>
    /// <returns></returns>
    IEnumerator CSVLoadAddressable(string key)
    {
        //アドレス名（Key）を指定しCCD（CloudContentDelivery）からCSVファイルを取得する
        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(key);
        //ロードが完了するまで処理を停止している
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TextAsset currentTextAsset = handle.Result;
            //ロードされたcsvファイルの中で改行があれば次の要素に代入する
            _textArray = currentTextAsset.text.Split('\n');
        }
        else
        {
            Debug.Log("ロードに失敗しました");
        }
        yield return null;
    }
}
