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

    public ModifyDamage ModifyDamage { get; set; }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
    }
}
