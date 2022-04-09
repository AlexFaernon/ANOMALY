using System.Collections.Generic;

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

    public bool CanMove { get; set; }

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

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(4);
            }
        }
    }
}