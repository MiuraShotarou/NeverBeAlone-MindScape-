using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public BattleLoopHandler BattleLoopHandler;
    [SerializeField] public BattleEventController BattleEventController;
    [SerializeField] public BattleUIController UIController = default;
    [SerializeField] public List<BattleUnitPlayer> PlayerUnits;
    [SerializeField] public List<BattleUnitEnemyBase> EnemyUnits;
    [SerializeField] public GameObject PlayableHandler = default;
    [SerializeField] public PlayableDirector PlayableDirector = default;
}