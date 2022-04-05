using System.Collections.Generic;
using UnityEngine.Events;

public class Hero : ICharacter
{
    private static int _hp = 10;
    private static int _mp = 10;

    public int HP
    {
        get => _hp;
        private set
        {
            _hp = value;
            EventAggregator.UpdateHP.Publish(this);
        }
    }
    public int MP
    {
        get => _mp;
        private set => _mp = value;
    }

    public ModifyDamage ModifyDamage { get; set; } = new ModifyDamage();

    public IAbility[] Abilities
    {
        get
        {
            return new[] { Ability, Ultimate };
        }
    }

    public IAbility Ability { get; } = new AttackClass();
    public IAbility Ultimate { get; } = new MakeInvulnerability();

    public void TakeDamage(int damage)
    {
        ModifyDamage.Damage = damage;
        ModifyDamage.Event.Invoke();
        HP -= ModifyDamage.Damage;
    }

    private class AttackClass : IAbility
    {
        public int Cost { get; } = 1;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 2;

        public void CastAbility(List<IUnit> units)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(4);
            }
        }
    }
    
    private class MakeInvulnerability : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units)
        {
            foreach (var unit in units)
            {
                BuffsClass.Buffs.Add(new Invulnerability(unit));
            }
        }
    }
}

public class ModifyDamage
{
    public int Damage { get; set; }
    public UnityEvent Event = new UnityEvent();
}