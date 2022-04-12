using System.Collections.Generic;
using System.Linq;

public class Medic : Character
{
    public override IAbility BasicAbility { get; set; } = new CastHeal();
    public override IAbility FirstAbility { get; set; } = new Dispel();
    public override IAbility SecondAbility { get; set; }
    public override IAbility Ultimate { get; set; } = new MakeInvulnerability();

    private class CastHeal : IAbility
    {
        public int Cost { get; } = 1;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 2;

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.Heal(2);
            }
        }
    }
    
    private class Dispel : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
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
        public int Cost { get; }
        public int Cooldown { get; }
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