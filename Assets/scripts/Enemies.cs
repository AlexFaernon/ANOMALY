using UnityEngine;

public class Enemy : IEnemy
{
    private int _hp = 5;

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
        
        EventAggregator.DamageDealtByUnit.Publish(ModifyReceivedDamage.Damage, source);
        HP -= ModifyReceivedDamage.Damage;
        Debug.Log(HP);
    }

    public void Heal(int heal)
    {
        HP += heal;
    }
}
