using System;
using UnityEngine;

public abstract class Enemy : IEnemy
{
    private int _hp;

    public virtual int MaxHP { get; set; }

    public int HP
    {
        get => _hp;
        private set
        {
            _hp = Math.Min(value, MaxHP);
            EventAggregator.UpdateHP.Publish(this);
        }
    }

    public bool CanMove { get; set; }

    public Enemy()
    {
        HP = MaxHP;
    }

    public ModifyReceivedDamage ModifyReceivedDamage { get; } = new ModifyReceivedDamage();

    public void TakeDamage(int damage, IUnit source)
    {
        ModifyReceivedDamage.Source = source;
        ModifyReceivedDamage.Damage = damage;
        
        ModifyReceivedDamage.Event.Invoke();
        
        EventAggregator.UnitDamagedUnit.Publish(ModifyReceivedDamage.Damage, source, this);
        HP -= ModifyReceivedDamage.Damage;
        Debug.Log(HP);
    }

    public void Heal(int heal, bool canSurpassSegment = false)
    {
        HP += heal;
    }

    public virtual int Attack { get; }
}

public sealed class Weakling : Enemy
{
    public override int MaxHP { get; set; } = 3;
    public override int Attack => 1;
}

public sealed class FatBoy : Enemy
{
    public override int MaxHP { get; set; } = 6;
    public override int Attack => 1;
}

public sealed class Killer : Enemy
{
    public override int MaxHP { get; set; } = 4;
    public override int Attack => 2;
}
