using System.Collections.Generic;
using System.Linq;
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
            return new[] { BasicAbility, FirstAbility, Ultimate };
        }
    }

    public IAbility BasicAbility { get; } = new AttackClass();
    public IAbility FirstAbility { get; } = new AttackClass();
    public IAbility Ultimate { get; } = new AttackClass();

    public void TakeDamage(int damage)
    {
        ModifyDamage.Damage = damage;
        ModifyDamage.Event.Invoke();
        HP -= ModifyDamage.Damage;
    }

    public void Heal(int heal)
    {
        HP += heal;
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
}

public class Medic : ICharacter
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
            return new[] { BasicAbility, FirstAbility, Ultimate };
        }
    }

    public IAbility BasicAbility { get; } = new CastHeal();
    public IAbility FirstAbility { get; } = new Dispel();
    public IAbility Ultimate { get; } = new MakeInvulnerability();

    public void TakeDamage(int damage)
    {
        ModifyDamage.Damage = damage;
        ModifyDamage.Event.Invoke();
        HP -= ModifyDamage.Damage;
    }

    public void Heal(int heal)
    {
        HP += heal;
    }

    private class CastHeal : IAbility
    {
        public int Cost { get; } = 1;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 2;

        public void CastAbility(List<IUnit> units)
        {
            foreach (var unit in units)
            {
                unit.Heal(2);
            }
        }
    }
    
    private class Dispel : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units)
        {
            foreach (var status in units.SelectMany(unit => BuffsClass.Buffs.ToList().Where(x => x.Target == unit)))
            {
                status.Dispel();
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

public class Tank : ICharacter
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
    
    public ModifyDamage ModifyDamage { get; set; }
    public void TakeDamage(int damage)
    {
        ModifyDamage.Damage = damage;
        ModifyDamage.Event.Invoke();
        HP -= damage;
    }

    public void Heal(int heal)
    {
        HP += heal;
    }

    public int MP
    {
        get => _mp;
        private set => _mp = value;
    }
    
    public IAbility[] Abilities
    {
        get
        {
            return new[] { BasicAbility, FirstAbility, Ultimate };
        }
    }
    public IAbility BasicAbility { get; }
    public IAbility FirstAbility { get; }
    public IAbility Ultimate { get; }
}

public class ModifyDamage
{
    public int Damage { get; set; }
    public readonly UnityEvent Event = new UnityEvent();
}