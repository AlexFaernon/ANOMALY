using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public sealed class Medic : Character
{
    public override string Name => "Парацельсис";

    public override string Info =>
        "В прошлом Парацельсис рьяно служила рыцарским идеалам, следуя по стопам своего отца. Однако ее мечты были сломлены, когда во время заговора в ордене отца убили и она потеряла глаз. Девушка публично сломала свой меч, поклявшись никогда не возвращаться туда. Теперь Парацельсис нашла свое призвание в медицине и уничтожении аномалий.";
    public override IAbility BasicAbility { get; set; } = new CastHeal();
    public override IAbility FirstAbility { get; set; } = new Dispel();
    public override IAbility SecondAbility { get; set; } = new CastDelayedHealing();
    public override IAbility Ultimate { get; set; } = new MakeInvulnerability();
    
    [Serializable]
    private class CastHeal : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Первая помощь";
        public string Description => $"Восстанавливает <color=#E3B81B>{HealPower} ед. здоровья</color> <color=#E3B81B>{TargetCount}</color> цели(ям)";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => new[] { 1, 2, 2 }[AbilityUpgradeLevel];
        private int HealPower => new[] { 1, 1, 2 }[AbilityUpgradeLevel];

        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Medic/медик 1");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.Heal(HealPower);
            }
        }
    }

    [Serializable]
    private class Dispel : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Очищение ран";
        public string Description => "Снимает <color=#E3B81B>все эффекты</color> с целей";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 2, 3 }[AbilityUpgradeLevel];
        public int TargetCount => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Medic/медик 2");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var status in units.SelectMany(unit => StatusSystem.StatusList.ToList().Where(x => x.Target == unit)))
            {
                status.Dispel();
            }
        }
    }
    
    [Serializable]
    private class CastDelayedHealing : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Регенерация";

        public string Description =>
            $"{(LifeTake == 0 ? "И" : $"Забирает <color=#E3B81B>{LifeTake} ед. здоровья </color> в момент применения, и")}сцеляет по <color=#E3B81B>{HealPower} ед. здоровья</color> в течение <color=#E3B81B>трех последующих ходов</color>";
        public int Cost => new[] { 2, 3, 5 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 3, 3, 4 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int HealPower => new[] { 1, 2, 2 }[AbilityUpgradeLevel];
        private int LifeTake => new[] { 1, 2, 0 }[AbilityUpgradeLevel];
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Medic/медик 3");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new DelayedHealing(unit, HealPower, LifeTake));
            }
        }
    }

    [Serializable]
    private class MakeInvulnerability : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Name => "Не в мою смену!";
        public string Description => $"В течение <color=#E3B81B>3-ех ходов</color> здоровье цели не может упасть ниже <color=#E3B81B>{new object[] {1,3,"последней секции или ее остатка"}[AbilityUpgradeLevel]}</color>.{(AbilityUpgradeLevel == 2 ? " При наложении на врага в течение <color=#E3B81B>3 ходов убавляет по 1 ед. здоровья</color>" : "")}";
        public int Cost => new[] { 4, 5, 6 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 5, 5, 6 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        [NonSerialized] private Sprite _icon;

        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = Resources.Load<Sprite>("icons/Medic/медик 4");
                }

                return _icon;
            }
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                if (AbilityUpgradeLevel < 2 || AbilityUpgradeLevel == 2 && unit is ICharacter)
                {
                    StatusSystem.StatusList.Add(new Invulnerability(unit, AbilityUpgradeLevel));
                }
                else
                {
                    StatusSystem.StatusList.Add(new HPLoss(unit));
                }
            }
        }
    }
}