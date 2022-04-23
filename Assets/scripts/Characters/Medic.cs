using System.Collections.Generic;
using System.Linq;

public class Medic : Character
{
    public override IAbility BasicAbility { get; set; } = new CastHeal();
    public override IAbility FirstAbility { get; set; } = new Dispel();
    public override IAbility SecondAbility { get; set; } = new CastDelayedHealing();
    public override IAbility Ultimate { get; set; } = new MakeInvulnerability();

    private class CastHeal : IAbility
    {
        public string Description { get; }
        public int Cost { get; } = 0;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 1;

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.Heal(2);
            }
        }
    }

    private class CastDelayedHealing : IAbility
    {
        public string Description { get; }
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 3;
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new DelayedHealing(unit));
            }
        }
    }
    
    private class Dispel : IAbility
    {
        public string Description { get; }
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 2;
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var status in units.SelectMany(unit => StatusSystem.StatusList.ToList().Where(x => x.Target == unit)))
            {
                status.Dispel();
            }
        }
    }
    
    private class MakeInvulnerability : IAbility
    {
        public string Description { get; }
        public int Cost { get; } = 4;
        public int Cooldown { get; } = 5;
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Invulnerability(unit));
            }
        }
    }
}