using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionIconData : MonoBehaviour
{
	// Dictionaryで、状態enumとUIの状態アイコンの紐づきを作る。
    // 「SerializedDictionary」はInspectorで編集できる、カスタマイズされたDictionaryクラス。
	// 「_B_Script/SerializedDictionary.cs」で定義されている。
    // 
	// 今回はgitにあげるためにコンポーネントを作ったのだが、別にこんな形式にする必要はなく、
	// どこかの管理コンポーネントに下記のようなメンバー変数を追加しておくと良い。
    [SerializeField, Header("Dictionary<状態enum, UIアイコン>")]
    public SerializedDictionary<Condition, GameObject> ConditionIconDictionary;
}