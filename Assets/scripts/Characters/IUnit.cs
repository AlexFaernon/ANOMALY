using System.Collections.Generic;

public interface IUnit
{
    public int HP { get; }
    public bool CanMove { get; set; }
    public ModifyReceivedDamage ModifyReceivedDamage { get; set; }
    public void TakeDamage(int damage, IUnit source);
    public void Heal(int heal);
}

public interface ICharacter : IUnit
{
    public int MP { get; }
    public IAbility[] Abilities { get; }
    public IAbility BasicAbility { get; }
    public IAbility FirstAbility { get; }
    public IAbility Ultimate { get; }
}

public interface IAbility
{
    public int Cost { get; }
    public int Cooldown { get; }
    public int TargetCount { get; }
    public abstract void CastAbility(List<IUnit> units, IUnit source);
}

public interface IEnemy : IUnit
{
    
}
