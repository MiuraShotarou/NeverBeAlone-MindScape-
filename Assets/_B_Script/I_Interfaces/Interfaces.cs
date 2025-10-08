interface IAttackModifier
{
    float ModifyAttack(int level, float attack);
}

interface IAttackScaleModifier
{
    float ModifyAttackScale(int level, float attackScale);
}

interface IDefenseModifier
{
    float ModifyDefense(int level, float defense);
}

interface IDefenseScaleModifier
{
    float ModifyDefenseScale(int level, float defenseScale);
}

interface ICriticalRateModifier
{
    float ModifyCriticalRate(int level, float criticalRate);
}

interface IDexModifier
{
    float ModifyDex(int level, float dex);
}
///<summary>回復力</summary>
interface IRegenModifier
{
    float ModifyRegen(int level, float regen);
}
///<summary>回避率</summary>
interface IEvadeRateModifier
{
    float ModifyEvadeRate(int level, float evadeRate);
}