using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private ObjectManager _objects;
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
    [SerializeField] public Text _damageText;
    [SerializeField] public Canvas _UICanvas;
    
    protected BattleLoopHandler _loopHandler = default;
    protected Camera _mainCamera = default;
    
    protected Animator _animator = default;

    public delegate void JudgeSurvival();
    protected void Start()
    {
        OnStart();
    }

    /// <summary>
    /// Startメソッドが実行する処理
    /// </summary>
    protected virtual void OnStart()
    {
        _mainCamera = Camera.main;
        _loopHandler = _objects.BattleLoopHandler;
        _animator = gameObject.GetComponent<Animator>();
        _objects.BattleEventController.DoJudgeSurvival += DoJudgeSurvival;
    }
    void Update()
    {

    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public abstract void TakeDamage(int damage);

    public virtual void OnDeath()
    {
        Debug.Log(_unitName + "の死亡時効果発動.");
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    protected bool IsUnitDead()
    {
        return _hp <= 0;
    }
    /// <summary>
    /// 死亡判定を行う
    /// 死亡となった場合、その効果を実行する
    /// </summary>
    protected virtual void DoJudgeSurvival()
    {
        if (IsUnitDead())
        {
            _isDead = true;
            // Common.LogDebugLine(this, "DoJudgeSurvival()", _unitName + "死亡");
            _animator.Play("Death");
            OnDeath();
            _objects.BattleEventController.DoJudgeSurvival -= DoJudgeSurvival;
        }
        else
        {
            // Common.LogDebugLine(this, "DoJudgeSurvival()", _unitName + "生存");
        }
    }
    /// <summary>
    /// 攻撃を受ける時の処理
    /// </summary>
    public virtual void OnAttacked(float damage)
    {
        Common.LogDebugLine(this, "OnAttacked()", "Called. UnitName: " + _unitName);
        float finalDefense = CalculateFinalDefense();
        int finalDamage = CalculateFinalDamageTaken(damage);
        TakeDamage(finalDamage);
    }

    protected virtual int CalculateFinalDamageTaken(float damage)
    {
        // TODO 未実装
        int finalDamage = Mathf.CeilToInt(damage);
        return finalDamage;
    }

    protected virtual float CalculateFinalDefense()
    {
        return _defense;
    }
}
