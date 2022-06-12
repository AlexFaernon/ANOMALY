using System;
using System.Collections.Generic;
using UnityEngine.Events;
[Serializable]
public class ModifyReceivedDamage
{
    public int Damage { get; set; }
    public IUnit Source;
    public readonly UnityEvent Event = new UnityEvent();
}

[Serializable]
public abstract class Character : ICharacter
{

    private int _hp = 6;
    private int _mp = 6;
    private int _maxHP = 6;

    public int MaxHP
    {
        get => _maxHP;
        set
        {
            var prev = _maxHP;
            _maxHP = value;
            HP += value - prev;
        }
    }

    public virtual string Name { get; }
    public virtual string Info { get; }
    public int HPSegmentLength { get; set; } = 3;
    public int MaxMP { get; set; } = 6;

    public virtual int HP
    {
        get => _hp;
        private set
        {
            _hp = Math.Min(value, MaxHP);
            EventAggregator.UpdateHP.Publish(this);
        }
    }
    
    public virtual int MP
    {
        get => _mp;
        set
        {
            _mp = Math.Min(value, MaxMP);
            EventAggregator.UpdateMP.Publish(this);
        }
    }

    public bool IsDead { get; set; }
    public bool HP1Upgrade { get; set; }
    public bool HP2Upgrade { get; set; }
    public bool MP1Upgrade { get; set; }
    public bool MP2Upgrade { get; set; }

    private bool _canMove;

    public bool CanMove
    {
        get => _canMove;
        set
        {
            _canMove = value;
            EventAggregator.UpdateMovability.Publish(this);
        }
    }

    [NonSerialized] private ModifyReceivedDamage _modifyReceivedDamage;

    public ModifyReceivedDamage ModifyReceivedDamage => _modifyReceivedDamage ??= new ModifyReceivedDamage();
    
    public virtual void TakeDamage(int damage, IUnit source)
    {
        ModifyReceivedDamage.Source = source;
        ModifyReceivedDamage.Damage = damage;
        
        ModifyReceivedDamage.Event.Invoke();
        
        EventAggregator.UnitDamagedUnit.Publish(ModifyReceivedDamage.Damage, source, this);
        HP -= ModifyReceivedDamage.Damage;
    }

    public virtual void Heal(int heal, bool canSurpassSegment = false)
    {
        if (canSurpassSegment)
        {
            HP += heal;
            return;
        }
        
        var lastHPSegment = HP % HPSegmentLength;
        if (lastHPSegment == 0) return;
        var actualHeal = HPSegmentLength - lastHPSegment;
        HP += Math.Min(actualHeal, heal);
    }

    public Dictionary<AbilityType, IAbility> Abilities => new Dictionary<AbilityType, IAbility>
    {
        { AbilityType.Basic, BasicAbility },
        { AbilityType.First, FirstAbility },
        { AbilityType.Second, SecondAbility },
        { AbilityType.Ultimate, Ultimate }
    };
    public virtual IAbility BasicAbility { get; set; }
    public virtual IAbility FirstAbility { get; set; }
    public virtual IAbility SecondAbility { get; set; }
    public virtual IAbility Ultimate { get; set; }
}