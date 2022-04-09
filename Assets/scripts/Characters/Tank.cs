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

    public ModifyDamage ModifyDamage { get; set; } = new ModifyDamage();
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
    public IAbility Ultimate { get; }
    
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
}