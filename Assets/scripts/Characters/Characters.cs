using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ModifyReceivedDamage
{
    public int Damage { get; set; }
    public IUnit Source;
    public readonly UnityEvent Event = new UnityEvent();
}

public abstract class Character : ICharacter
{

    private int _hp = 6;
    private int _mp = 6;

    public int MaxHP { get; } = 6;
    public int HPSegmentLength { get; } = 3;
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

    public bool CanMove { get; set; }
    public ModifyReceivedDamage ModifyReceivedDamage { get; set; } = new ModifyReceivedDamage();
    public virtual void TakeDamage(int damage, IUnit source)
    {
        ModifyReceivedDamage.Source = source;
        ModifyReceivedDamage.Damage = damage;
        
        ModifyReceivedDamage.Event.Invoke();
        
        EventAggregator.DamageDealtByUnit.Publish(ModifyReceivedDamage.Damage, source);
        HP -= ModifyReceivedDamage.Damage;
    }

    public virtual void Heal(int heal)
    {
        var lastHPSegment = HP % HPSegmentLength;
        var actualHeal = HPSegmentLength - lastHPSegment;
        HP += Math.Min(actualHeal, MaxHP - HP);
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