using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Damager : Character
{
    public override string Name => "Маркус Кокс";

    public override string Info =>
        "Авантюрист, который ищет наживы. Хоть Маркус человек не из высших слоев общества, он всю жизнь славился изворотливым характером, поэтому знал, где и когда можно неплохо заработать. Пошел в \"Уничтожителей аномалий\" из-за хорошего дохода.";
    public override IAbility BasicAbility { get; set; } = new AttackClass();
    public override IAbility FirstAbility { get; set; } = new CastDamageUp();
    public override IAbility SecondAbility { get; set; } = new LifeStealing();
    public override IAbility Ultimate { get; set; } = new LotOfDamage();

    [Serializable]
    private class AttackClass : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Выпад";
        public string Description => $"Наносит урон в <color=#E3B81B>{Damage} ед.</color> здоровья выбранной цели";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => 1;
        private int Damage => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Dammager/дд 1");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.TakeDamage(Damage, source);
            }
        }
    }
    
    [Serializable]
    private class CastDamageUp : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Уязвимость";

        public string Description =>
            $"Увеличивает восприимчивость цели к урону на <color=#E3B81B>2 хода</color>. Получаемый ею урон будет увеличен на <color=#E3B81B>{AdditionalDamage} ед.</color>";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 2, 3 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int AdditionalDamage => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Dammager/дд 2");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new AmplifyDamage(unit, AdditionalDamage));
            }
        }
    }
    
    [Serializable]
    private class LifeStealing : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Адреналин";
        public string Description => $"Восстанавливает <color=#E3B81B>{(AbilityUpgradeLevel == 0 ? "1 ед. здоровья" : "половину от нанесенного урона")}</color> при ударе.{(AbilityUpgradeLevel == 2 ? " Если персонаж не получил урона, то на второй ход восстановление увеличится <color=#E3B81B>до 80%</color>" : "")}";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int TargetCount => 0;
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Dammager/дд 3");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new LifeSteal(unit, AbilityUpgradeLevel));
            }
        }
    }
    
    [Serializable]
    private class LotOfDamage : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Жар битвы";
        public string Description => $"Наносит урон в <color=#E3B81B>{Damage}  ед.</color> здоровья выбранной цели";
        public int Cost => new[] { 4, 5, 6 }[AbilityUpgradeLevel];
        public int Cooldown  => new[] { 5, 6, 7 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int Damage => new[] { 4, 6, 10 }[AbilityUpgradeLevel];
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Dammager/дд 4");
                }

                return _icon;
            }
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