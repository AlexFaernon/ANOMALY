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
        public string Description { get; } = "Наносит урон в 2 хп выбранной цели";
        public int Cost { get; } = 0;
        public int Cooldown { get; } = 0;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public AttackClass(Sprite icon)
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
    
    private class CastDamageUp : IAbility
    {
        public string Description { get; } = "Увеличивает восприимчивость цели к урону. Получаемый ею урон будет увеличен на 2 хода";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 2;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }

        public CastDamageUp(Sprite icon)
        {
            Icon = icon;
        }

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
        public string Description { get; } = "На время действия при нанесении персонажем урона, восстанавливает ему здоровье в половину от нанесенного урона. Длительность 2 хода";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 2;
        public int TargetCount { get; } = 0;
        public Sprite Icon { get; }
        
        public LifeStealing(Sprite icon)
        {
            Icon = icon;
        }

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
        public string Description { get; } = "Наносит урон в 4 хп выбранной цели ";
        public int Cost { get; } = 4;
        public int Cooldown { get; } = 5;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public LotOfDamage(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(4, source);
            }
        }
    }
}