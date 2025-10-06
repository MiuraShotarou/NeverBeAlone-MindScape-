using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/Create Omoiokomete")]
public sealed class Omoiokomete : SkillBase
{
    public override string SkillName => "Omoiokomete";
    public override string SkillDescription => "敵一体に強力な一撃を与える。";
    public override Tension NeedTension => Tension.Calm;
    public override SkillEffect SkillEffect => new OmoiokometeEffect();

}
public sealed class OmoiokometeEffect : SkillEffect//Base ,I……
{
    
}