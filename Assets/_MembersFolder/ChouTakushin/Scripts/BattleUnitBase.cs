using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleUnitBase : MonoBehaviour
{
    [Header("デバッグ用")]
    [SerializeField] public string _unitName;

    [SerializeField] public int _hp;
    [SerializeField] public int _sp;
    [SerializeField] public int _dex;
    [SerializeField] public int _attack;
    [SerializeField] public int _defense;
    [SerializeField] public bool _isDead;
    [SerializeField] public float _attackAdjustment;
    [SerializeField] public float _defenseAdjustment;
    [SerializeField] public Image _icon;
    
    [SerializeField] protected BattleHandler _battleHandler = default;
    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        // TODO UIでダメージを表示
        
        Debug.Log(_unitName + "がダメージを受ける： " + damage);
    }
}
