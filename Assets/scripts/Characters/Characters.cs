using System.Collections.Generic;
using UnityEngine.Events;
public class ModifyReceivedDamage
{
    public int Damage { get; set; }
    public IUnit Source;
    public readonly UnityEvent Event = new UnityEvent();
}

public abstract class Character : ICharacter
{
    private int _hp = 10;
    private int _mp = 10;

    public virtual int HP
    {
        get => _hp;
        private set
        {
            _hp = value;
            EventAggregator.UpdateHP.Publish(this);
        }
    }

    public int HPSegmentLength { get; } = 3;

    public virtual int MP
    {
        get => _mp;
        private set => _mp = value;
    }
    
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
        HP += heal;
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