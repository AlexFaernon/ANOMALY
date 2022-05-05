using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public int MaxHP { get; }
    public int HP { get; }
    public bool CanMove { get; set; }
    public ModifyReceivedDamage ModifyReceivedDamage { get; set; }
    public void TakeDamage(int damage, IUnit source);
    public void Heal(int heal, bool canSurpassSegment = false);
}

public interface ICharacter : IUnit
{
    public int HPSegmentLength { get; }
    public int MaxMP { get; set; }
    public int MP { get; set; }
    public bool IsDead { get; set; }
    public Dictionary<AbilityType, IAbility> Abilities { get; }
    public IAbility Ultimate { get; set; }
}

public interface IAbility
{
    public string Description { get; }
    public int Cost { get; }
    public int Cooldown { get; }
    public int TargetCount { get; }
    public Sprite Icon { get; }
    public abstract void CastAbility(List<IUnit> units, IUnit source);
}

public interface IEnemy : IUnit
{
    
}
