using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public BattleLoopHandler BattleLoopHandler;
    [SerializeField] public BattleEventController BattleEventController;
    [SerializeField] public BattleUIController UIController = default;
    [SerializeField, Header("本番では使用しない")] public List<BattleUnitPlayer> PlayerUnits;
    [SerializeField, Header("本番では使用しない")] public List<BattleUnitEnemyBase> EnemyUnits;
    [SerializeField] public GameObject PlayableHandler = default;
    [SerializeField] public PlayableDirector PlayableDirector = default;
    [SerializeField] public AudioManager AudioManager = default;
    [SerializeField] private EmotionBase[] EmotionBaseArray; //GameDataManagerにてセットする予定
	public Dictionary<Emotion, EmotionBase> EmotionBaseDict;
    [SerializeField] private SkillBase[] SkillBaseArray;
    public Dictionary<string, SkillBase> SkillBaseDict;
    [SerializeField] private ConditionBase[] ConditionBaseArray;
	public Dictionary<Condition, ConditionBase> ConditionBaseDict;
    
    
    private void Awake()
    {
        if (SkillBaseArray != null)
        {
            EmotionBaseDict = EmotionBaseArray.ToDictionary(emotion => emotion.Emotion, emotion => emotion);
            SkillBaseDict = SkillBaseArray.ToDictionary(skill => skill.SkillEffectBase.Name, skill => skill);
            ConditionBaseDict = ConditionBaseArray.ToDictionary(condition => condition.Condition, condition => condition);
        }
#if UNITY_ANDROID || UNITY_IOS
        PlayerUnits.Clear();
        EnemyUnits.Clear();
        Debug.LogError($"BattleUnit系スクリプトをInspectorアタッチで参照しようとしたためどちらも空にしました。本番環境では両者ともScriptableObjectから動的参照するようにしてください");
#endif
    }
}