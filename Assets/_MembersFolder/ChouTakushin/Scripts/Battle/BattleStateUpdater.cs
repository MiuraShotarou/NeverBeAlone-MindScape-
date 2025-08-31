using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateUpdater : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
    private BattleLoopHandler _loopHandler;

    void Start()
    {
        _loopHandler = _objects.BattleLoopHandler;
    }
    /// <summary>
    /// 戦闘状態を設定する
    /// </summary>
    /// <param name="state"></param>
    public void UpdateBattleState(BattleState state)
    {
        _loopHandler.BattleState = state;
    }
    /// <summary>
    /// 戦闘状態を設定し、コンポーネントに紐づくGameObjectを無効化する
    /// </summary>
    /// <param name="state"></param>
    public void UpdateBattleStateAndDeactivateThis(BattleState state)
    {
        _loopHandler.BattleState = state;
        gameObject.GetComponent<DeactivateController>().DeactivateThis();
    }

    /// <summary>
    /// 次の段階へ進む
    /// 【⚠危険！使わないかも】
    /// </summary>
    /// <param name="state"></param>
    public void ToNextState(BattleState state)
    {
        _loopHandler.BattleState = ++state;
    }
}
