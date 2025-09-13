interface IAttackModifier
{
    float ModifyAttack(float attack);
}

interface IAttackScaleModifier
{
    float ModifyAttackScale(float attackScale);
}
    
interface IDefenseModifier
{
    float ModifyDefense(float defense);
}
interface IDefenseScaleModifier
{
    float ModifyDefenseScale(float defenseScale);
}

interface ICriticalRateModifier
{
    float ModifyCriticalRate(float criticalRate);
}

interface IDexModifier
{
    float ModifyDex(float dex);
}

interface IRegenModifier
{
    float ModifyRegen(float regen);
}

interface IEvadeRateModifier
{
    float ModifyEvadeRate(float evadeRate);
}