using System.Collections.Generic;

public class Hero : Character
{
    public override IAbility BasicAbility { get; set; } = new AttackClass();
    public override IAbility FirstAbility { get; set; } = new AttackClass();
    public override IAbility SecondAbility { get; set; } = new AttackClass();
    public override IAbility Ultimate { get; set; } = new AttackClass();
    
    private class AttackClass : IAbility
    {
        public int Cost { get; } = 1;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 2;

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(4, source);
            }
        }
    }
}