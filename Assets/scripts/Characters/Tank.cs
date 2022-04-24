using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Tank : Character
{
    public override IAbility BasicAbility { get; set; }
    public override IAbility FirstAbility { get; set; }
    public override IAbility SecondAbility { get; set; }
    public override IAbility Ultimate { get; set; }
    
    public Tank()
    {
        var icons = Resources.LoadAll<Sprite>("icons/Tank");
        BasicAbility = new CastProtect(icons[0]);
        FirstAbility = new CastStun(icons[1]);
        SecondAbility = new CastDeflect(icons[2]);
        Ultimate = new CastBerserk(icons[3]);
        
    }
    
    private class CastProtect : IAbility
    {
        public string Description { get; } = "Защита персонажа: во время хода выбирается персонаж, которого в течении одного хода, герой будет защищать. Если на него нападут, то герой получит урон вместо цели. (ради бога не кастуйте это на самого танка, там баг)";
        public int Cost { get; } = 0;
        public int Cooldown { get; } = 0;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public CastProtect(Sprite icon)
        {
            Icon = icon;
        }

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
        public string Description { get; } = "Оглушает цель, лишая ее права хода";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 2;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public CastStun(Sprite icon)
        {
            Icon = icon;
        }

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
        public string Description { get; } = "При получение персонажем урона, он наносит урон атакующему";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 3;
        public int TargetCount { get; } = 0;
        public Sprite Icon { get; }
        
        public CastDeflect(Sprite icon)
        {
            Icon = icon;
        }

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
        public string Description { get; } = "На время действия заменяет все способности на одну мощную атакующую способность. Замена способности не тратит ход персонажа";
        public int Cost { get; } = 4;
        public int Cooldown { get; } = 5;
        public int TargetCount { get; } = 0;
        public Sprite Icon { get; }
        
        public CastBerserk(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Berserk((ICharacter)unit, new BerserkAbility(Icon)));
            }
        }

        private class BerserkAbility : IAbility
        {
            public string Description { get; } = "Наносит 4 урона 3-ем целям";
            public int Cost { get; } = 0;
            public int Cooldown { get; } = 0;
            public int TargetCount { get; } = 3;
            public Sprite Icon { get; }
            
            public BerserkAbility(Sprite icon)
            {
                Icon = icon;
            }

            public void CastAbility(List<IUnit> units, IUnit source)
            {
                foreach (var unit in units)
                {
                    unit.TakeDamage(2, source);
                }
            }
        }
    }
}