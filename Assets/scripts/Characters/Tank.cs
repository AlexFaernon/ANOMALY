using System.Collections.Generic;
using UnityEngine.Assertions;

public class Tank : Character
{
    public override IAbility BasicAbility { get; set; } = new CastProtect();
    public override IAbility FirstAbility { get; set; } = new CastStun();
    public override IAbility SecondAbility { get; set; } = new CastDeflect();
    public override IAbility Ultimate { get; set; } = new CastBerserk();
    
    private class CastProtect : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                Assert.IsNotNull(source);
                StatusSystem.StatusList.Add(new Protect(unit, source));
            }  
        }
    }
    
    private class CastStun : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 1;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Stun(unit));
            }
        }
    }

    private class CastDeflect : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 0;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Deflect(unit));
            }
        }
    }
    
    private class CastBerserk : IAbility
    {
        public int Cost { get; }
        public int Cooldown { get; }
        public int TargetCount { get; } = 0;
        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Berserk((ICharacter)unit,new BerserkAbility()));
            }
        }

        private class BerserkAbility : IAbility
        {
            public int Cost { get; }
            public int Cooldown { get; }
            public int TargetCount { get; } = 3;
            public void CastAbility(List<IUnit> units, IUnit source)
            {
                foreach (var unit in units)
                {
                    unit.TakeDamage(5, source);
                }
            }
        }
    }
}