using System.Collections.Generic;
using UnityEngine.Assertions;

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

    public bool CanMove { get; set; }

    public ModifyReceivedDamage ModifyReceivedDamage { get; set; } = new ModifyReceivedDamage();
    public void TakeDamage(int damage, IUnit source)
    {
        ModifyReceivedDamage.Source = source;
        ModifyReceivedDamage.Damage = damage;
        ModifyReceivedDamage.Event.Invoke();
        HP -= ModifyReceivedDamage.Damage;
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

    public IAbility BasicAbility { get; } = new CastProtect();
    public IAbility FirstAbility { get; } = new CastStun();
    public IAbility Ultimate { get; } = new CastDeflect();
    
    private class CastProtect : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                Assert.IsNotNull(source);
                StatusEffects.Effects.Add(new Protect(unit, source));
            }  
        }
    }
    
    private class CastStun : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusEffects.Effects.Add(new Stun(unit));
            }
        }
    }

    private class CastDeflect : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusEffects.Effects.Add(new Deflect(unit));
            }
        }
    }
}