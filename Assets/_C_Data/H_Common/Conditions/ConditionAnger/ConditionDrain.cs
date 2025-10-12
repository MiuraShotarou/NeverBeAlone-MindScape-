using UnityEngine;

/// <summary>�h���C���i�{��j�^�����_���[�W * 0.1 �񕜂���</summary>
public class ConditionDrain : ConditionBase
{
    private Emotion _emotion = Emotion.Anger;
    [SerializeField] private int _damageDealt = 10;
    [SerializeField] private float _hpRestoreScale = 0.1f;

    /// <summary>�h���C���i�{��j�^�����_���[�W * 0.1 �񕜂���</summary>
    public ConditionDrain()
    {
        _condition = Condition.Drain;
        _name = "�h���C��";
        _type = ConditionActivationType.OnAttackExecute;
    }

    public override void ApplyCondition()
    {
        
    }

    public override void ReapplyCondition()
    {
        
    }

    public override void ActivateConditionEffect()
    {
        // TODO �������G�ɗ^�����_���[�W�������Ă���
        //_damageDealt = 
        int restoringHp = Mathf.RoundToInt(_damageDealt * _hpRestoreScale);
        restoringHp *= -1;
        _target.GetComponent<BattleUnitBase>().TakeDamage(restoringHp);
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "���������܂���");
    }

    public override void RemoveCondition()
    {
        
    }
}
