using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Tank : Character
{
    public override string Name => "Теодор Роджерс";
    public override string Info =>
        "Теодор с детства занимался ремеслом. Его завораживали доспехи, их изящество, но в то же время прочность и стойкость. Когда Тео вырос, он стал одержимым идеей сделать идеальный доспех, которому будут нипочем никакие атаки исчадий аномалий. Ради этого он сам записался в \"уничтожителей\", чтобы тестировать свои доспехи и выявлять дефекты, делая новые, более близкие к идеалу.";
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
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Телохранитель";
        public string Description => $"В течение хода получает <color=#E3B81B>{DamageReduction*100}%</color> урона вместо цели. В случае использования на самого персонажа только снижает урон на <color=#E3B81B>{100 - DamageReduction*100}%</color>";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => 1;
        private double DamageReduction => new[] { 1, 0.8, 0.5 }[AbilityUpgradeLevel];
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
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Оглушение";
        public string Description => $"Оглушает <color=#E3B81B>{TargetCount}</color> цель(ей) на {(AbilityUpgradeLevel == 2 ? "<color=#E3B81B>1 и 2 хода</color> соответственно" : $"<color=#E3B81B>{new[] { 1, 2 }[AbilityUpgradeLevel]} ход(ов)</color>")}, лишая ее/их права хода. Длительность 1 ход";
        public int Cost => new[] { 2, 4, 5 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int TargetCount => new[] { 1, 1, 2 }[AbilityUpgradeLevel];
        public Sprite Icon { get; }
        
        public CastStun(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            new Action<List<IUnit>, IUnit>[] { BasicCast, BasicCast, UpgradedCast }[AbilityUpgradeLevel].Invoke(units, source);
        }

        private void BasicCast(List<IUnit> units, IUnit source)
        {
            StatusSystem.StatusList.Add(new Stun(units[0], new[] { 1, 2 }[AbilityUpgradeLevel]));
        }

        private void UpgradedCast(List<IUnit> units, IUnit source)
        {
            StatusSystem.StatusList.Add(new Stun(units[0], 2));
            StatusSystem.StatusList.Add(new Stun(units[1], 1));
        }
    }

    private class CastDeflect : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Контратака";
        public string Description => $"При получение персонажем урона, он наносит урон в <color=#E3B81B>{Damage}</color> хп атакующему. Длительность <color=#E3B81B>2 хода</color>";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => 3;
        public int TargetCount => 0;
        private int Damage => new[] { 1, 2, 4 }[AbilityUpgradeLevel];
        public Sprite Icon { get; }
        
        public CastDeflect(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Deflect(unit, Damage));
            }
        }
    }
    
    private class CastBerserk : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Берсерк";
        public string Description => "На <color=#E3B81B>3 хода</color> заменяет способность на одну мощную атакующую способность. <color=#E3B81B>Замена способности не тратит ход персонажа</color>";
        public int Cost => new[] { 4, 5, 6 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 5, 5, 6 }[AbilityUpgradeLevel];
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
                StatusSystem.StatusList.Add(new Berserk((ICharacter)unit, new BerserkAbility(Icon, AbilityUpgradeLevel)));
            }
        }

        private class BerserkAbility : IAbility
        {
            public int OverallUpgradeLevel { get; set; }
            public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
            public string Name => "Разрушение";
            public string Description => $"Наносит {Damage} урона 3-ем целям";
            public int Cost => 0;
            public int Cooldown => 0;
            public int TargetCount => 3;
            private int Damage => new[] { 2, 3, 5 }[AbilityUpgradeLevel];
            public Sprite Icon { get; }
            
            public BerserkAbility(Sprite icon, int upgradeLevel)
            {
                OverallUpgradeLevel = upgradeLevel;
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