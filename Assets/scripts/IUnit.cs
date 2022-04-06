using System.Collections.Generic;

public interface IUnit
{
    public int HP { get; }
    public ModifyDamage ModifyDamage { get; set; }
    public void TakeDamage(int damage);
    public void Heal(int heal);
}

public interface ICharacter : IUnit
{
    public int MP { get; }
    public IAbility[] Abilities { get; }
    public IAbility Ability { get; }
    public IAbility Ultimate { get; }
}

public interface IAbility
{
    public int Cost { get; }
    public int Cooldown { get; }
    public int TargetCount { get; }
    public abstract void CastAbility(List<IUnit> units);
}

public interface IEnemy : IUnit
{
    
}
