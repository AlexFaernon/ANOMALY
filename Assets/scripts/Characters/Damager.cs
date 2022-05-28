using System.Collections.Generic;
using UnityEngine;

public sealed class Damager : Character
{
    public override string Name => "Маркус Кокс";

    public override string Info =>
        "Авантюрист который ищет наживы. Хоть Маркус человек не из высших слоев общества, он всю жизнь славился изворотливым характером, поэтому знал где и когда можно неплохо заработать. Пошел в \"Уничтожителей аномалий\" из-за хорошего дохода (и возможно опасности).";
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
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Выпад";
        public string Description => $"Наносит урон в <color=#E3B81B>{Damage}</color> единиц здоровья выбранной цели";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => 1;
        private int Damage => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
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
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Уязвимость";

        public string Description =>
            $"Увеличивает восприимчивость цели к урону на <color=#E3B81B>2 хода</color>. Получаемый ею урон будет увеличен на <color=#E3B81B>{AdditionalDamage}</color>";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 2, 3 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int AdditionalDamage => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
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
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Адреналин";
        public string Description => $"Восстанавливает <color=#E3B81B>{(AbilityUpgradeLevel == 1 ? "единицу здоровья" : "половину от нанесенного урона")}</color> при ударе.{(AbilityUpgradeLevel == 2 ? " Если персонаж не получил урона то на второй ход восстановление увеличится <color=#E3B81B>до 80%</color>" : "")}";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
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
                StatusSystem.StatusList.Add(new LifeSteal(unit, AbilityUpgradeLevel));
            }
        }
    }

    private class LotOfDamage : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Жар битвы";
        public string Description => $"Наносит урон в <color=#E3B81B>{Damage}</color> единиц здоровья выбранной цели ";
        public int Cost => new[] { 4, 5, 6 }[AbilityUpgradeLevel];
        public int Cooldown  => new[] { 5, 6, 7 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int Damage => new[] { 4, 6, 10 }[AbilityUpgradeLevel];
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