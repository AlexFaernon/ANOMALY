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
    
    public int MP
    {
        get => _mp;
        private set
        {
            _mp = value;
        }
    }

    public IAbility Ability { get; } = new AttackClass();

    public void TakeDamage(int damage)
    {
        HP -= damage;
    }

    private class AttackClass : IAbility
    {
        public int Cost { get; } = 1;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 2;

        public void CastAbility(List<IUnit> units)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(2);
            }
        }
    }
}