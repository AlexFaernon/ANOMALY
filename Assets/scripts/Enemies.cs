using System;
using UnityEngine;

public class Enemy : IEnemy
{
    private int _hp = 5;

    public int MaxHP { get; set; }

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

    public ModifyReceivedDamage ModifyReceivedDamage { get; set; } = new ModifyReceivedDamage();

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
}
