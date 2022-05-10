using System;
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
        public int UpgradeLevel { get; set; } = 0;
        public string Description => $"Защита персонажа: во время хода выбирается персонаж, которого в течении одного хода, герой будет защищать. Если на него нападут, то герой получит {DamageReduction*100}% урон вместо цели. (ради бога не кастуйте это на самого танка, там баг)";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => 1;
        private double DamageReduction => new[] { 1, 0.8, 0.5 }[UpgradeLevel];
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
                StatusSystem.StatusList.Add(new Protect(unit, source, DamageReduction));
            }  
        }
    }
    
    private class CastStun : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;
        public string Description { get; } = "Оглушает цель, лишая ее права хода. Длительность 1 ход";
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
            new Action<List<IUnit>, IUnit>[] { BasicCast, BasicCast, UpgradedCast }[UpgradeLevel].Invoke(units, source);
        }

        private void BasicCast(List<IUnit> units, IUnit source)
        {
            StatusSystem.StatusList.Add(new Stun(units[0], new[] { 1, 2 }[UpgradeLevel]));
        }

        private void UpgradedCast(List<IUnit> units, IUnit source)
        {
            StatusSystem.StatusList.Add(new Stun(units[0], 2));
            StatusSystem.StatusList.Add(new Stun(units[0], 1));
        }
    }

    private class CastDeflect : IAbility //todo upgrade
    {
        public int UpgradeLevel { get; set; }
        public string Description { get; } = "При получение персонажем урона, он наносит урон в 1 хп атакующему. Длительность 2 хода";
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
                StatusSystem.StatusList.Add(new Deflect(unit, 1));
            }
        }
    }
    
    private class CastBerserk : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;
        public string Description => "На 3 хода заменяет все способности на одну мощную атакующую способность. Замена способности не тратит ход персонажа";
        public int Cost => new[] { 4, 5, 6 }[UpgradeLevel];
        public int Cooldown => new[] { 5, 5, 6 }[UpgradeLevel];
        public int TargetCount => 0;
        public Sprite Icon { get; }
        
        public CastBerserk(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Berserk((ICharacter)unit, new BerserkAbility(Icon, UpgradeLevel)));
            }
        }

        private class BerserkAbility : IAbility
        {
            public int UpgradeLevel { get; set; }
            public string Description => $"Наносит {Damage} урона 3-ем целям";
            public int Cost => 0;
            public int Cooldown => 0;
            public int TargetCount => 3;
            private int Damage => new[] { 2, 3, 5 }[UpgradeLevel];
            public Sprite Icon { get; }
            
            public BerserkAbility(Sprite icon, int upgradeLevel)
            {
                UpgradeLevel = upgradeLevel;
                Icon = icon;
            }

            public void CastAbility(List<IUnit> units, IUnit source)
            {
                foreach (var unit in units)
                {
                    unit.TakeDamage(Damage, source);
                }
            }
        }
    }
}