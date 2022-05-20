using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Medic : Character
{
    public override string Name => "Парацельсис";

    public override string Info =>
        "В прошлом Парацельсис рьяно служила рыцарским идеалам, следуя по стопам своего отца. Однако ее мечты были сломлены, когда во время заговора в ордене отца убили и она потеряла глаз. Девушка публично сломала свой меч, поклявшись никогда не возвращаться туда. Теперь Парацельсис нашла свое призвание в медицине и уничтожении аномалий";

    public override IAbility BasicAbility { get; set; }
    public override IAbility FirstAbility { get; set; }
    public override IAbility SecondAbility { get; set; }
    public override IAbility Ultimate { get; set; }

    public Medic()
    {
        var icons = Resources.LoadAll<Sprite>("icons/Medic");
        BasicAbility = new CastHeal(icons[0]);
        FirstAbility = new Dispel(icons[1]);
        SecondAbility = new CastDelayedHealing(icons[2]);
        Ultimate = new MakeInvulnerability(icons[3]);
    }

    private class CastHeal : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Description => $"Восстанавливает {HealPower} хп {TargetCount} цели(ям)";
        public int Cost => 0;
        public int Cooldown => 0;
        public int TargetCount => new[] { 1, 2, 2 }[AbilityUpgradeLevel];
        private int HealPower => new[] { 1, 1, 2 }[AbilityUpgradeLevel];

        public Sprite Icon { get; }
        
        public CastHeal(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.Heal(HealPower);
            }
        }
    }

    private class Dispel : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Description => "Снимает все эффекты с целей";
        public int Cost => new[] { 2, 3, 4 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 2, 2, 3 }[AbilityUpgradeLevel];
        public int TargetCount => new[] { 1, 2, 3 }[AbilityUpgradeLevel];
        public Sprite Icon { get; }
        
        public Dispel(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var status in units.SelectMany(unit => StatusSystem.StatusList.ToList().Where(x => x.Target == unit)))
            {
                status.Dispel();
            }
        }
    }
    
    private class CastDelayedHealing : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Description =>
            $"{(LifeTake == 0 ? "И" : $"Забирает {LifeTake} хп в момент применения, и")}сцеляет по {HealPower} хп в течении трех последующих ходов";
        public int Cost => new[] { 2, 3, 5 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 3, 3, 4 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        private int HealPower => new[] { 1, 2, 2 }[AbilityUpgradeLevel];
        private int LifeTake => new[] { 1, 2, 0 }[AbilityUpgradeLevel];
        public Sprite Icon { get; }
        
        public CastDelayedHealing(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new DelayedHealing(unit, HealPower, LifeTake));
            }
        }
    }

    private class MakeInvulnerability : IAbility
    {
        public int OverallUpgradeLevel { get; set; } = 0;
        public int AbilityUpgradeLevel => OverallUpgradeLevel / 2;
        public string Description => $"В течение 3-ех ходов здоровье цели не может упасть ниже {new object[] {1,3,"последней секции или ее остатка"}[AbilityUpgradeLevel]}.{(AbilityUpgradeLevel == 2 ? " При наложении на врага в течении 3 ходов убавляет по 1 хп" : "")}";
        public int Cost => new[] { 4, 5, 6 }[AbilityUpgradeLevel];
        public int Cooldown => new[] { 5, 5, 6 }[AbilityUpgradeLevel];
        public int TargetCount => 1;
        public Sprite Icon { get; }
        
        public MakeInvulnerability(Sprite icon)
        {
            Icon = icon;
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