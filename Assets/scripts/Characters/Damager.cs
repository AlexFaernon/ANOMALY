using System.Collections.Generic;

public class Damager : Character
{
    public override IAbility BasicAbility { get; set; } = new AttackClass();
    public override IAbility FirstAbility { get; set; } = new CastDamageUp();
    public override IAbility SecondAbility { get; set; } = new LifeStealing();
    public override IAbility Ultimate { get; set; } = new LotOfDamage();
    
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
    
    private class CastDamageUp : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new AmplifyDamage(unit));
            }
        }
    }
    
    private class LifeStealing : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 0;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new LifeSteal(unit));
            }
        }
    }

    private class LotOfDamage : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(10, source);
            }
        }
    }
}