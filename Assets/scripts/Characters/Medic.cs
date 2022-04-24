using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Medic : Character
{
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
        public string Description { get; } = "��������������� ���� ������� ��������";
        public int Cost { get; } = 0;
        public int Cooldown { get; } = 1;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public CastHeal(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                unit.Heal(1);
            }
        }
    }

    private class CastDelayedHealing : IAbility
    {
        public string Description { get; } = "�������� ���� ������� �� � ������ ����������, �������� �� ������ �� � ������� ���� ����������� �����";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 3;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public CastDelayedHealing(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new DelayedHealing(unit));
            }
        }
    }
    
    private class Dispel : IAbility
    {
        public string Description { get; } = "������� ��� ������� � ����";
        public int Cost { get; } = 2;
        public int Cooldown { get; } = 2;
        public int TargetCount { get; } = 1;
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
    
    private class MakeInvulnerability : IAbility
    {
        public string Description { get; } = "������������ �� ��������� ����� - �� ����� ��� �������� �� ����� ������ ���� 1";
        public int Cost { get; } = 4;
        public int Cooldown { get; } = 5;
        public int TargetCount { get; } = 1;
        public Sprite Icon { get; }
        
        public MakeInvulnerability(Sprite icon)
        {
            Icon = icon;
        }

        public void CastAbility(List<IUnit> units, IUnit source)
        {
            foreach (var unit in units)
            {
                StatusSystem.StatusList.Add(new Invulnerability(unit));
            }
        }
    }
}