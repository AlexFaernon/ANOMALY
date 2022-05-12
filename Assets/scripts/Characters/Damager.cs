using System.Collections.Generic;
using UnityEngine;

public sealed class Damager : Character
{
    public override IAbility BasicAbility { get; set; }
    public override IAbility FirstAbility { get; set; }
    public override IAbility SecondAbility { get; set; }
    public override IAbility Ultimate { get; set; }

    public Damager()
    {
        var icons = Resources.LoadAll<Sprite>("icons/Dammager");
        BasicAbility = new AttackClass(icons[0]);
        FirstAbility = new CastDamageUp(icons[1]);
        SecondAbility = new LifeStealing(icons[2]);
        Ultimate = new LotOfDamage(icons[3]);
    }
    
    private class AttackClass : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;
        public string Description => $"Наносит урон в {Damage} хп выбранной цели";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => 1;
        private int Damage => new[] { 1, 2, 3 }[UpgradeLevel];
        public Sprite Icon { get; }
        
        public AttackClass(Sprite icon)
        {
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
    
    private class CastDamageUp : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;

        public string Description =>
            $"Увеличивает восприимчивость цели к урону на 2 хода. Получаемый ею урон будет увеличен на {AdditionalDamage}";
        public int Cost => new[] { 2, 3, 4 }[UpgradeLevel];
        public int Cooldown => new[] { 2, 2, 3 }[UpgradeLevel];
        public int TargetCount => 1;
        private int AdditionalDamage => new[] { 1, 2, 3 }[UpgradeLevel];
        public Sprite Icon { get; }

        public CastDamageUp(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new AmplifyDamage(unit, AdditionalDamage));
            }
        }
    }
    
    private class LifeStealing : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;
        public string Description => "На время действия при нанесении персонажем урона восстанавливает ему здоровье. Длительность 2 хода";
        public int Cost => new[] { 2, 3, 4 }[UpgradeLevel];
        public int Cooldown => new[] { 2, 3, 4 }[UpgradeLevel];
        public int TargetCount => 0;
        public Sprite Icon { get; }
        
        public LifeStealing(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new LifeSteal(unit, UpgradeLevel));
            }
        }
    }

    private class LotOfDamage : IAbility
    {
        public int UpgradeLevel { get; set; } = 0;
        public string Description => $"Наносит урон в {Damage} хп выбранной цели ";
        public int Cost => new[] { 4, 5, 6 }[UpgradeLevel];
        public int Cooldown  => new[] { 5, 6, 7 }[UpgradeLevel];
        public int TargetCount => 1;
        private int Damage => new[] { 4, 6, 10 }[UpgradeLevel];
        public Sprite Icon { get; }
        
        public LotOfDamage(Sprite icon)
        {
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