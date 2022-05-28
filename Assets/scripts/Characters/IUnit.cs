using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public int MaxHP { get; set; }
    public int HP { get; }
    public bool CanMove { get; set; }
    public ModifyReceivedDamage ModifyReceivedDamage { get; }
    public void TakeDamage(int damage, IUnit source);
    public void Heal(int heal, bool canSurpassSegment = false);
}

public interface ICharacter : IUnit
{
    public string Name { get; }
    public string Info { get; }
    public int HPSegmentLength { get; set; }
    public int MaxMP { get; set; }
    public int MP { get; set; }
    public bool IsDead { get; set; }
    public bool HP1Upgrade { get; set; }
    public bool HP2Upgrade { get; set; }
    public bool MP1Upgrade { get; set; }
    public bool MP2Upgrade { get; set; }
    public Dictionary<AbilityType, IAbility> Abilities { get; }
    public IAbility Ultimate { get; set; }
}

public interface IAbility
{
    public int OverallUpgradeLevel { get; set; }
    public string Name { get; }
    public string Description { get; }
    public int Cost { get; }
    public int Cooldown { get; }
    public int TargetCount { get; }
    public Sprite Icon { get; }
    public abstract void CastAbility(List<IUnit> units, IUnit source);
}

public interface IEnemy : IUnit
{
    public int Attack { get; }
}
